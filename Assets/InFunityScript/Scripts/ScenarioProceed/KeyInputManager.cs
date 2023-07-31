using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class KeyInputManager : MonoBehaviour
    {
        // シナリオコマンドを纏めておくクラス
        private ScenarioCommands sc;
        // シナリオ進行に用いる真偽値変数を纏めておくクラス
        private ScenarioBooleans sb;
        // 画面表示の切り替えを行うクラス
        private SystemPanelsController systemPanelsController;

        private void Awake()
        {
            // 基本的なコマンド以外のシナリオコマンドは別ファイルに記載し、読みだすようにする
            sc = GetComponent<ScenarioCommands>();
            // シナリオ進行に用いる真偽値は別ファイルに定義して管理する
            sb = GetComponent<ScenarioBooleans>();
            // 画面表示の切り替えを行うクラスを取得する
            systemPanelsController = GetComponent<SystemPanelsController>();
        }

        // キー入力を受け付ける
        private void Update()
        {
            // テキスト更新中にクリックした場合は、全文を表示する
            if (IsClick() && sb.isAddingText) sb.isStopAddingTextAndDisplayAllText = true;

            // スキップとオートの入力を検知する
            if (IsInputForAuto()) SwitchAutoMode();
            if (IsInputForSkip()) SwitchSkipMode();

            // 右クリックでスキップ/オート/バックログ/設定画面を解除する
            if (IsCancelClick()) { ExitAutoMode(); ExitSkipMode(); ExitBackLog(); ExitSettings(); }

            // デバッグ用
            // 画面サイズの変更
            if (Input.GetKeyDown(KeyCode.O)) Screen.SetResolution(1280, 720, false);
            if (Input.GetKeyDown(KeyCode.P)) Screen.SetResolution(1920, 1080, true);

            // Bボタンでバックログの表示/非表示の切り替え
            if (Input.GetKeyDown(KeyCode.B) && !sb.isDisplaySettings) { SwitchBacklog(); ExitAutoMode(); ExitSkipMode(); }
            // Nボタンで設定画面の表示/非表示の切り替え
            if (Input.GetKeyDown(KeyCode.N) && !sb.isDisplayBacklog) { SwitchSettings(); ExitAutoMode(); ExitSkipMode(); }
            // Kボタンでセーブ画面の表示/非表示の切り替え
            if (Input.GetKeyDown(KeyCode.K) && !sb.isDisplaySettings && !sb.isDisplayBacklog) { ExitLoad(); SwitchSavePanel(); ExitAutoMode(); ExitSkipMode(); }
            // Lボタンでセーブ画面の表示/非表示の切り替え
            if (Input.GetKeyDown(KeyCode.L) && !sb.isDisplaySettings && !sb.isDisplayBacklog) { ExitSave(); SwitchLoadPanel(); ExitAutoMode(); ExitSkipMode(); }
        }
        
        public bool IsClick()
        {
            // 各種キー入力でクリック判定をONにする
            // マウススクロールでもクリック判定をONにする
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Mouse ScrollWheel") < 0;
        }
        private bool IsInputForSkip()
        {
            return Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.RightCommand) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
        }
        private bool IsInputForAuto()
        {
            return Input.GetKeyDown(KeyCode.A);
        }
        // 右クリックで解除できるようにする
        private bool IsCancelClick()
        {
            return Input.GetMouseButtonDown(1);
        }

        // システム画面（バックログ、設定画面、セーブロード画面）を表示しているかどうか
        private bool IsDisplaySystemPanel()
        {
            return sb.isDisplayBacklog || sb.isDisplaySettings;
        }

        // スキップの切り替え
        private void SwitchSkipMode()
        {
            if (sb.isSkipping)
            {
                ExitSkipMode();
            }
            else
            {
                sb.isSkipping = true;
                sb.isNextPageForce_WhenPressSkipOrAuto_WhileWaitClick = true;
                //OnSkipModeObj.SetActive(true);
            }
        }
        // オートの切り替え
        public void SwitchAutoMode()
        {
            if (sb.isAutoPlaying)
            {
                ExitAutoMode();
            }
            else
            {
                sb.isAutoPlaying = true;
                sb.isNextPageForce_WhenPressSkipOrAuto_WhileWaitClick = true;
                //OnAutoModeObj.SetActive(true);
            }
        }
        // スキップ/オートモード終了
        private void ExitSkipMode()
        {
            sb.isSkipping = false;
            //OnSkipModeObj.SetActive(false);
        }
        private void ExitAutoMode()
        {
            sb.isAutoPlaying = false;
            //OnAutoModeObj.SetActive(false);
        }

        // バックログの切り替え
        public void SwitchBacklog()
        {
            if (sb.isDisplayBacklog)
            {
                sb.isDisplayBacklog = false;
                systemPanelsController.BacklogLayerSetActive(false);
                systemPanelsController.TextWindowLayerSetActive(true);
            }
            else
            {
                sb.isDisplayBacklog = true;
                systemPanelsController.BacklogLayerSetActive(true);
                systemPanelsController.TextWindowLayerSetActive(false);
            }
        }
        private void ExitBackLog()
        {
            sb.isDisplayBacklog = true;
            SwitchBacklog();
        }

        // 設定画面への切り替え
        public void SwitchSettings()
        {
            if (sb.isDisplaySettings)
            {
                sb.isDisplaySettings = false;
                systemPanelsController.SettingsLayerSetActive(false);
                sb.isMouseOverOnSystemButtons = false;
            }
            else
            {
                sb.isDisplaySettings = true;
                systemPanelsController.SettingsLayerSetActive(true);
            }
        }
        private void ExitSettings()
        {
            sb.isDisplaySettings = true;
            SwitchSettings();
        }

        // Save画面への切り替え
        public void SwitchSavePanel()
        {
            if (sb.isDisplaySaveLoadPanel)
            {
                sb.isDisplaySaveLoadPanel = false;
                systemPanelsController.SaveLayerSetActive(false);
            }
            else
            {
                sb.isDisplaySaveLoadPanel = true;
                systemPanelsController.SaveLayerSetActive(true);
            }
        }
        // Load画面への切り替え
        public void SwitchLoadPanel()
        {
            if (sb.isDisplaySaveLoadPanel)
            {
                sb.isDisplaySaveLoadPanel = false;
                systemPanelsController.LoadLayerSetActive(false);
            }
            else
            {
                sb.isDisplaySaveLoadPanel = true;
                systemPanelsController.LoadLayerSetActive(true);
            }
        }
        private void ExitSave()
        {
            // SAVE画面でない時は何もしない
            if (systemPanelsController.saveloadLayer.GetComponent<SaveLoadLayer>().mode != SaveLoadLayer.MODE.SAVE) return;
            sb.isDisplaySaveLoadPanel = false;
            systemPanelsController.SaveLayerSetActive(false);
        }
        // LOAD画面を終了する
        // SaveDataHolder.LoadAndStartScenarioFromSlotNum からロード時にLoad画面を非表示にするために呼び出す
        public void ExitLoad()
        {
            // LOAD画面でない時は何もしない
            if (systemPanelsController.saveloadLayer.GetComponent<SaveLoadLayer>().mode != SaveLoadLayer.MODE.LOAD) return;
            sb.isDisplaySaveLoadPanel = false;
            systemPanelsController.LoadLayerSetActive(false);
        }
    }
}
