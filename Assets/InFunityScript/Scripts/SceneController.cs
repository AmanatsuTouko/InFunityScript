﻿using System.Collections;

namespace FThingSoftware.InFunityScript
{
    public class SceneController : MonoBehaviour
    {
        // Titleに戻る
        public void ChangeTitleScene()
        {
            SceneManager.LoadScene("Title");
        }

        // ゲームを終了する
        public void ExitGame()
        {
#if UNITY_EDITOR
#else
#endif
    }
}