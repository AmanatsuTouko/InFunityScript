using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SettingsLayerWindowSizeChange : MonoBehaviour
    {
        // フルスクリーンへの切り替え
        public void ChangeFullScreen()
        {
            if (Screen.fullScreen) return;
            Screen.SetResolution(Screen.width, Screen.height, true);
        }
        // ウィンドウモードへの切り替え
        public void ChangeWindowed()
        {
            if (!Screen.fullScreen) return;
            Screen.SetResolution(Screen.width, Screen.height, false);
        }
        // 画面サイズの変更
        public void WindowSizeTo_1920_1080()
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);            
        }
        public void WindowSizeTo_1280_720()
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
        public void WindowSizeTo_960_540()
        {
            Screen.SetResolution(960, 540, Screen.fullScreen);
        }
        public void WindowSizeTo_640_360()
        {
            Screen.SetResolution(640, 360, Screen.fullScreen);
        }
    }
}