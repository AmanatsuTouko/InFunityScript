using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace FThingSoftware.InFunityScript
{
    public class ScenarioCommandCharacter : MonoBehaviour
    {
        public GameObject CharacterPrefab;
        public GameObject CharaLayer;

        public async UniTask CharaShow(string charaName, string[] facetype, float time, float posx = 0, float posy = 0, bool reverse = false)
        {
            // 既に同じキャラクターがCharaLayerにある場合には実行しない
            if (IsAlreadyExistChara(charaName)) return;

            // Prefabを用いてインスタンスの作成
            GameObject prefab = Instantiate(CharacterPrefab);
            // 親オブジェクトをCanvasにする
            prefab.transform.SetParent(CharaLayer.transform, false);
            // オブジェクトの名前を変更する
            prefab.name = charaName;

            // charaのクラスをAddComponent
            Type charaClass = Type.GetType(charaName);
            var character = (Character)prefab.AddComponent(charaClass);

            // 初期値の代入
            character.setParameter();
            
            // facetypeからspriteを読みだす
            List<Sprite> sprites = character.GetSpritesFromFacetype(facetype);
            // facetypeに定義されている複数枚の画像を合成する
            Sprite sprite = await SynthesisMultipleSprite(sprites);
            // backImageに合成したSpriteを代入する
            Image image = prefab.transform.GetChild(0).GetComponent<Image>();
            image.sprite = sprite;

            // サイズと位置の初期化を行う
            character.SetDefault();

            // 反転の指定がある場合
            // if (reverse) CharaReverse(charaName, 0));
            // 座標の指定があった場合
            if (!(posx == 0 && posy == 0))
            {
                RectTransform transform = prefab.GetComponent<RectTransform>();
                transform.anchoredPosition += new Vector2(posx, posy);
            }

            // n秒かけて透明度を戻して見えるようにする
            // 画像生成処理フレームは、1フレームの時間が長くなってしまうので1フレーム待つ
            await UniTask.Yield(PlayerLoopTiming.Update);
            await AlphaIncreaceRefTime(image, time);
        }

        // n秒かけて透明度を0->255にする
        private async UniTask AlphaIncreaceRefTime(Image image, float time)
        {
            float alpha = 0;
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                float deltaAlpha = 255.0f / (time / Time.deltaTime);
                alpha += deltaAlpha;
                if (alpha > 255)
                {
                    image.color = new Color32(255, 255, 255, 255);
                    break;
                }
                image.color = new Color32(255, 255, 255, (byte)(int)alpha);
            }
        }

        // キャラクターが既にCharaLayerに存在しているか
        private bool IsAlreadyExistChara(string charaName)
        {
            bool flag = true;
            try
            {
                GameObject charaNameObj = CharaLayer.transform.Find(charaName).gameObject;
            }
            catch (NullReferenceException e)
            {
                flag = false;
            }
            if (flag)
            {
                Debug.LogErrorFormat("Error: Character Prefab {0} is already exist. Can't show {0} more.", charaName);
                return true;
            }
            else
            {
                return false;
            }
        }

        // 複数枚の画像を合成して一枚のSpriteを返す関数
        private async UniTask<Sprite> SynthesisMultipleSprite(List<Sprite> sprites)
        {
            // 1枚の場合はそのまま返す
            if (sprites.Count == 1) return sprites[0];
            
            // 出力用Texture2Dを作成する
            Texture2D synthesisTexture = new Texture2D(sprites[0].texture.width, sprites[0].texture.height, TextureFormat.ARGB32, false);

            // 一枚目をコピーする
            Graphics.CopyTexture(sprites[0].texture, synthesisTexture);
            // 複数枚の画像を合成する
            for (int i = 1; i < sprites.Count; i++)
            {
                synthesisTexture = await GetSynthesisTexture2D(synthesisTexture, sprites[i].texture);
            }

            // Spriteに変換して返す
            return Sprite.Create(synthesisTexture, new Rect(0, 0, synthesisTexture.width, synthesisTexture.height), Vector2.zero);
        }

        // 2枚のTexture2Dを合成する関数
        private async UniTask<Texture2D> GetSynthesisTexture2D(Texture2D baseTexture, Texture2D blendTexture)
        {
            // 任意のフレームの描画処理が終わるまで待機する
            await UniTask.WaitForEndOfFrame();

            // 出力用Texture2D
            Texture2D synthesisTexture = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.ARGB32, false);

            // RenderTextureを作成して書き込む
            var renderTextureA = RenderTexture.GetTemporary(synthesisTexture.width, synthesisTexture.height);
            Graphics.Blit(baseTexture, renderTextureA);
            var renderTextureB = RenderTexture.GetTemporary(synthesisTexture.width, synthesisTexture.height);
            Graphics.Blit(blendTexture, renderTextureB);

            // Texture2Dを作成してRenderTextureから書き込む
            //現在のRenderTextureをキャッシュする
            var preRT = RenderTexture.active;

            RenderTexture.active = renderTextureA;
            var TextureA = new Texture2D(synthesisTexture.width, synthesisTexture.height);
            TextureA.ReadPixels(new Rect(0, 0, synthesisTexture.width, synthesisTexture.height), 0, 0);
            TextureA.Apply();

            RenderTexture.active = renderTextureB;
            var TextureB = new Texture2D(synthesisTexture.width, synthesisTexture.height);
            TextureB.ReadPixels(new Rect(0, 0, synthesisTexture.width, synthesisTexture.height), 0, 0);
            TextureB.Apply();

            // キャッシュしたRenderTextureを元に戻す
            RenderTexture.active = preRT;

            // RenderTextureの解除
            RenderTexture.ReleaseTemporary(renderTextureA);
            RenderTexture.ReleaseTemporary(renderTextureB);

            // 2枚の画像を合成する
            var pixels = TextureA.GetPixels();
            var blendPixels = TextureB.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                var basePixel = pixels[i];
                var blendPixel = blendPixels[i];

                var baseAlpha = basePixel.a - blendPixel.a;
                var blendAlpha = blendPixel.a;

                var r = basePixel.r * baseAlpha + blendPixel.r * blendAlpha;
                var g = basePixel.g * baseAlpha + blendPixel.g * blendAlpha;
                var b = basePixel.b * baseAlpha + blendPixel.b * blendAlpha;

                // alpha値 加算合成
                var a = MathF.Min(1.0f, baseAlpha + blendAlpha);

                // どちらも透明度が0の時は、alpha値に0を指定する
                if (basePixel.a == 0 && blendPixel.a == 0) a = 0.0f;

                // 合成後のピクセルの設定
                pixels[i] = new Color(r, g, b, a);
            }
            synthesisTexture.SetPixels(pixels);
            synthesisTexture.Apply();

            return synthesisTexture;
        }
    }
}

