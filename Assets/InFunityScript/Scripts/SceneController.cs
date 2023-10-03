using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.SceneManagement;

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
#if UNITY_EDITOR            UnityEditor.EditorApplication.isPlaying = false;
#else            Application.Quit();
#endif        }
    }
}