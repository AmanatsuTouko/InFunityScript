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
        [SerializeField] GameObject _characterPrefab;
        public GameObject CharaLayer;

        public async UniTask CharaShow(string charaName, string[] facetype, float time, float posx = 0, float posy = 0, bool reverse = false)
        {
            // 既に同じキャラクターがCharaLayerにある場合には実行しない
            if (IsAlreadyExistChara(charaName))
            {
                Debug.LogErrorFormat(
                    $"Error: chara_show<{charaName}>({charaName}.{facetype[0]})\n" +
                    $"Character Prefab {charaName} is already exist. Can't show {charaName} more.");
                return;
            }

            // Prefabを用いてインスタンスの作成
            GameObject prefab = Instantiate(_characterPrefab);
            // 親オブジェクトをCanvasにする
            prefab.transform.SetParent(CharaLayer.transform, false);
            // オブジェクトの名前を変更する
            prefab.name = charaName;

            // charaのクラスをAddComponent
            Type charaClass = Type.GetType(charaName);
            var character = (Character)prefab.AddComponent(charaClass);

            // 初期値の代入
            character.setParameter();

            // データ保存用に、どの表情を設定しているかを記憶しておく
            character.Images = facetype;

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
            if (reverse) await CharaReverse(charaName, reverse, time, true);

            // 座標の指定があった場合
            if (posx != 0 || posy != 0)
            {
                Transform transform = prefab.GetComponent<Transform>();

                transform.localPosition = new Vector2(
                    posx == 0 ? character.Pos.x : posx, 
                    posy == 0 ? character.Pos.y : posy
                );
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

        // n秒かけて透明度を255->0にする
        private async UniTask AlphaDecreaceRefTime(Image image, float time)
        {
            float alpha = 255;
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                float deltaAlpha = 255.0f / (time / Time.deltaTime);
                alpha -= deltaAlpha;
                if (alpha < 0)
                {
                    image.color = new Color32(255, 255, 255, 0);
                    break;
                }
                image.color = new Color32(255, 255, 255, (byte)(int)alpha);
            }
        }

        private async UniTask AlphaDecreaceRefTime(List<Image> images, float time)
        {
            float alpha = 255;
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                float deltaAlpha = 255.0f / (time / Time.deltaTime);
                alpha -= deltaAlpha;
                if (alpha < 0)
                {
                    foreach(var image in images)
                    {
                        image.color = new Color32(255, 255, 255, 0);
                    }
                    break;
                }
                foreach (var image in images)
                {
                    image.color = new Color32(255, 255, 255, (byte)(int)alpha);
                }
            }
        }

        // キャラクターが既にCharaLayerに存在しているか
        private bool IsAlreadyExistChara(string charaName)
        {
            try
            {
                GameObject charaNameObj = CharaLayer.transform.Find(charaName).gameObject;
                return true;
            }
            catch (NullReferenceException e)
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

        public async UniTask CharaFace(string charaName, string[] facetype, float time)
        {
            // キャラクターが居ない場合には生成する
            if (!IsAlreadyExistChara(charaName))
            {
                Debug.LogError(
                    $"Error: chara_face<{charaName}>({charaName}.{facetype[0]})\n" +
                    $"Character {charaName} is not exist. Create {charaName} Character Object.");
                await CharaShow(charaName: charaName, facetype: facetype, time: time, reverse: false);
            }

            // キャラクターをCanvasから探す
            GameObject charaObj = CharaLayer.transform.Find(charaName).gameObject;

            var backImage = charaObj.transform.GetChild(0).GetComponent<Image>();
            var frontImage = charaObj.transform.GetChild(1).GetComponent<Image>();

            // データ保存用に、どの表情を設定しているかを記憶しておく
            charaObj.GetComponent<Character>().Images = facetype;

            // FrontImageを設定する

            // facetypeからspriteを読みだす
            var character = charaObj.GetComponent<Character>();
            List<Sprite> sprites = character.GetSpritesFromFacetype(facetype);
            // facetypeに定義されている複数枚の画像を合成する
            Sprite sprite = await SynthesisMultipleSprite(sprites);
            // frontImageに合成したSpriteを代入する
            frontImage.sprite = sprite;

            // n秒かけて透明度を戻して見えるようにする
            // 画像生成処理フレームは、1フレームの時間が長くなってしまうので1フレーム待つ
            await UniTask.Yield(PlayerLoopTiming.Update);
            await AlphaIncreaceRefTime(frontImage, time);

            // FrontImageをBackImageに異動させる
            backImage.sprite = frontImage.sprite;

            // FrontImageを削除する
            frontImage.sprite = null;
            frontImage.color = new Color32(255, 255, 255, 0);
        }

        public async UniTask CharaHide(string charaName, float time)
        {
            // キャラクターが居ない場合には何もしない
            if (!IsAlreadyExistChara(charaName))
            {
                Debug.LogError(
                    $"Error: chara_hide<{charaName}>()\n" +
                    $"Character {charaName} is not exist. Can't hide {charaName} Character Object.");
                return;
            }

            // キャラクターをCanvasから探す
            GameObject charaObj = CharaLayer.transform.Find(charaName).gameObject;
            var backImage = charaObj.transform.GetChild(0).GetComponent<Image>();

            // N秒かけて透明度を下げる
            // 画像生成処理フレームは、1フレームの時間が長くなってしまうので1フレーム待つ
            await UniTask.Yield(PlayerLoopTiming.Update);
            await AlphaDecreaceRefTime(backImage, time);

            // オブジェクトを削除する
            Destroy(charaObj);
        }

        public async UniTask CharaHideAll(float time)
        {
            // キャラクターがない場合には何もしない
            int childCount = CharaLayer.transform.childCount;
            if (childCount == 0)
            {
                return;
            }

            // 全員のオブジェクトを取得する
            List<GameObject> charaObjs = new List<GameObject>();
            List<Image> images = new List<Image>();
            for (int i=0; i<childCount; i++)
            {
                var obj = CharaLayer.transform.GetChild(i).gameObject;
                charaObjs.Add(obj);

                var backImage = obj.transform.GetChild(0).GetComponent<Image>();
                images.Add(backImage);
            }

            // N秒かけて透明度を下げる
            await UniTask.Yield(PlayerLoopTiming.Update);
            await AlphaDecreaceRefTime(images, time);

            // オブジェクトを削除する
            foreach(var obj in charaObjs)
            {
                Destroy(obj);
            }
        }

        public async UniTask CharaReverse(string charaName, bool reverse, float time, bool clockwiseRotation)
        {
            // キャラクターが居ない場合には何もしない
            if (!IsAlreadyExistChara(charaName))
            {
                Debug.LogError(
                    $"Error: chara_reverse<{charaName}>()\n" +
                    $"Character {charaName} is not exist. Can't reverse {charaName} Character Object.");
                return;
            }

            // キャラクターをCanvasから探す
            GameObject charaObj = CharaLayer.transform.Find(charaName).gameObject;
            var rot = charaObj.transform.localRotation;
            
            // n秒かけて回転させる
            await RotationYRefTime(charaObj.transform, reverse, time, clockwiseRotation);
        }

        // n秒かけてRotationのY軸を変化させる
        private async UniTask RotationYRefTime(Transform targetTransform, bool reverse, float time, bool clockwiseRotation)
        {
            // 反転解除命令で既に通常の場合は何もしない
            if(Mathf.Abs(targetTransform.rotation.y) == 0 && reverse == false)
            {
                return;
            }
            // 反転命令で既に反転している場合は何もしない
            if(Mathf.Abs(targetTransform.rotation.y) == 180 && reverse == true)
            {
                return;
            }

            // Y軸の回転値の初期値
            float initRotY = 0;
            // 回転の向き
            int minusOrPlus = 1;

            // 反転命令で、右回りの場合
            if(reverse && clockwiseRotation)
            {
                initRotY = 0;
                minusOrPlus = 1;
            }
            // 反転命令で、左回りの場合
            else if(reverse && !clockwiseRotation)
            {
                initRotY = 0;
                minusOrPlus = -1;
            }
            // 反転解除命令で、右回りの場合
            else if(!reverse && clockwiseRotation)
            {
                initRotY = -180;
                minusOrPlus = 1;
            }
            // 反転解除命令で、左回りの場合
            else if(!reverse && !clockwiseRotation)
            {
                initRotY = 180;
                minusOrPlus = -1;
            }

            var rot = targetTransform.localRotation;
            float rotY = initRotY;

            // 累積値
            float sumRot = 0;
            
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                
                float deltaRotY = 180.0f / (time / Time.deltaTime) * minusOrPlus;
                rotY += deltaRotY;

                // 終了条件
                sumRot += deltaRotY;
                if(MathF.Abs(sumRot) >= 180)
                {
                    rotY = reverse ? 180 : 0;
                    targetTransform.localRotation = Quaternion.Euler(rot.x, rotY, rot.z);
                    break;
                }

                targetTransform.localRotation = Quaternion.Euler(rot.x, rotY, rot.z);
            }
        }

        public async UniTask CharaMove(string charaName, float time, float posx, float posy, Easing.Ease easing, bool absolute)
        {
            // キャラクターが居ない場合には何もしない
            if (!IsAlreadyExistChara(charaName))
            {
                Debug.LogError(
                    $"Error: chara_move<{charaName}>()\n" +
                    $"Character {charaName} is not exist. Can't move {charaName} Character Object.");
                return;
            }

            // キャラクターをCanvasから探す
            GameObject charaObj = CharaLayer.transform.Find(charaName).gameObject;
            var transform = charaObj.transform;

            // イージング関数の取得
            var Ease = Easing.GetEasing(easing);

            // 現在点と移動先の設定
            Vector2 startPos = transform.localPosition;
            Vector2 endPos;

            // 絶対座標の時
            if(absolute)
            {
                endPos = new Vector2(posx, posy);
            }
            // 相対座標の時
            else
            {
                endPos = startPos + new Vector2(posx, posy);
            }

            // N秒かけて移動させる
            Vector2 subPos = endPos - startPos;
            float e = 0;
            while(true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                e += Time.deltaTime / time;
                if(e >= 1.0f)
                {
                    transform.localPosition = endPos;
                    break;
                }
                Vector2 nextPos = startPos + Ease(e) * subPos;
                transform.localPosition = nextPos;
            }       
        }
    }
}

