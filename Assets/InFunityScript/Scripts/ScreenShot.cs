using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FThingSoftware.InFunityScript
{
    [ExecuteInEditMode]
    public class ScreenShot : MonoBehaviour
    {
        public static IEnumerator Capture(string filename)
        {
            // スクリーンショット画像のサイズを設定
            var size = Settings.SCREENSHOT_SIZE;
            var render = new RenderTexture(size.x, size.y, 24);
            var texture = new Texture2D(size.x, size.y, TextureFormat.RGB24, false);

            // カメラを取得
            var camera = GameObject.Find(Settings.SCREENSHOT_CAMERA_NAME).GetComponent<Camera>();

            // カメラの描画タイプをbaseに変更する（URPのOverlayモードでは上手くキャプチャできないため）
            var cameraData = camera.GetUniversalAdditionalCameraData();
            bool isChangedRenderTypeOverLayToBase = false;
            if(cameraData.renderType != CameraRenderType.Base)
            {
                cameraData.renderType = CameraRenderType.Base;
                isChangedRenderTypeOverLayToBase = true;
            }

            try
            {
                // カメラ画像を RenderTexture に描画
                camera.targetTexture = render;
                camera.Render();

                // RenderTexture の画像を読み取る
                RenderTexture.active = render;
                texture.ReadPixels(new Rect(0, 0, size.x, size.y), 0, 0);
                texture.Apply();
            }
            finally
            {
                camera.targetTexture = null;
                RenderTexture.active = null;
            }

            // フォルダーがない場合は新規作成
            if (!File.Exists($"{Application.persistentDataPath}/{Settings.SCREENSHOT_SAVE_FOLDER}"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/{Settings.SCREENSHOT_SAVE_FOLDER}");
            }

            // PNG 画像としてファイル保存
            File.WriteAllBytes($"{Application.persistentDataPath}/{Settings.SCREENSHOT_SAVE_FOLDER}/{filename}.png", texture.EncodeToPNG());

            // カメラの描画タイプをoverlayに戻す
            if(isChangedRenderTypeOverLayToBase) cameraData.renderType = CameraRenderType.Overlay;

            yield return 0;
        }
    }
}
