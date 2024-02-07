using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class TitleButtonClick : MonoBehaviour
    {
        [SerializeField] GameObject LoadLayer;

        [Header("Buttons")]
        [SerializeField] Button _newgameButton;
        [SerializeField] Button _loadgameButton;
        [SerializeField] Button _continuegameButton;
        [SerializeField] Button _settingsDisplayButton;
        [SerializeField] Button _licenseDisplayButton;
        [SerializeField] Button _exitgameButton;
        
        private void Awake()
        {
            // ボタンクリック時の挙動の設定
            RegisterOnClickButtonMethod();
            // Titleシーンから移行したときの、モードフラグをOFFにしておく
            Settings.LoadMode = Settings.LOAD_MODE.UNDEFINED;
        }

        // ボタンクリック時の挙動の設定を行う
        private void RegisterOnClickButtonMethod()
        {
            _newgameButton.onClick.AddListener( () => OnClickNewGame() );
            _loadgameButton.onClick.AddListener( () => OnClickLoadGame() );
            _continuegameButton.onClick.AddListener( () => OnClickContinueGame() );
            _settingsDisplayButton.onClick.AddListener( () => OnClickDisplaySettings() );
            _licenseDisplayButton.onClick.AddListener( () => OnClickDisplayLicense() );
            _exitgameButton.onClick.AddListener( () => OnClickExitGame() );
        }

        // Titleシーンのボタンから呼び出す関数一覧
        // 新しく始める
        public void OnClickNewGame()
        {
            // SaveDataHolderの0番目のデータのRebuildData、CallStacksを消す
            SaveDataHolder.I.ResetDataForContinue();
            // NewGameフラグをONにする
            Settings.LoadMode = Settings.LOAD_MODE.NEW_GAME;
            // シーンを移行する
            SceneManager.LoadScene(Settings.SCENE_MAIN);
        }

        // セーブデータをロードして続きから始める
        public void OnClickLoadGame()
        {
            // Load画面の表示
            LoadLayer.SetActive(true);
            LoadLayer.GetComponent<SaveLoadLayer>().UpdateSaveSlotsDetailAll();

            // 選択が終わった場合に0番目のデータにディープコピーを行う


            // シーンを移行してゲームスタート
        }

        //  前回の続きから始める
        public void OnClickContinueGame()
        {
            // 前回セーブした際の0番目のデータから復元してゲームを開始するためにContinueGameフラグをONにする
            Settings.LoadMode = Settings.LOAD_MODE.CONTINUE;
            // シーンを移行する
            SceneManager.LoadScene(Settings.SCENE_MAIN);
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
