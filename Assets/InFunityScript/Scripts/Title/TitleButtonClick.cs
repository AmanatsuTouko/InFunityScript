using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FThingSoftware.InFunityScript
{
    public class TitleButtonClick : MonoBehaviour
    {
        [SerializeField] GameObject LoadLayer;


        private void Start()
        {
            // Titleシーンから移行したときの、モードフラグをOFFにしておく
            Settings.LoadFromTitleScene = false;
            Settings.NewGameFromTitleScene = false;
        }

        // Titleシーンのボタンから呼び出す関数一覧
        // 新しく始める
        public void OnClickNewGame()
        {
            // SaveDataHolderの0番目のデータのRebuildData、CallStacksを消す
            SaveDataHolder.I.ResetDataForContinue();
            // NewGameフラグをONにする
            Settings.NewGameFromTitleScene = true;
            // シーンを移行する
            SceneManager.LoadScene(Settings.SCENE_MAIN);
        }

        // 続きから始める
        public void OnClickContinueGame()
        {
            // 前回セーブした際の0番目のデータから復元してゲームを開始するためにContinueGameフラグをONにする

            // シーンを移行する
        }

        // 前回の続きから始める
        public void OnClickLoadGame()
        {
            // Load画面の表示
            LoadLayer.SetActive(true);
            LoadLayer.GetComponent<SaveLoadLayer>().UpdateSaveSlotsDetailAll();

            // 選択が終わった場合に0番目のデータにディープコピーを行う


            // シーンを移行してゲームスタート
        }

        // 設定の表示
        public void OnClickDisplaySettings()
        {
         
        }

        // ライセンスの表示
        public void OnClickDisplayLicense()
        {

        }

        // ゲーム終了
        public void OnClickExitGame()
        {
            //UnityEditorの場合は、実行を終了する
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            // ビルドした実機環境ではゲームを終了してWindowを閉じる
#else
            Application.Quit();
#endif
        }
    }
}
