using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class Settings : MonoBehaviour
    {
        /// <summary>
        /// 定数を定義する
        /// </summary>

        // Define Settings Parameta
        // 0番目は現状のデータとして使うため、12n+1の数にする
        public const int SAVE_DATA_NUM = 85;
        // 1ページに表示するセーブデータの数
        public const int SAVE_SLOT_NUM_OF_PAGE = 12;

        // セーブに用いるjsonファイルの名前
        public const string SAVE_FILE_NAME_USER_SETTINGS         = "user-settings";
        public const string SAVE_FILE_NAME_REBUILD_DATA          = "rebuild-data";
        public const string SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT = "developer-define-dict";
        public const string SAVE_FILE_NAME_SCENARIO_CALLSTACK     = "scenario-callstack";

        // NewGame時に読み込むシナリオ
        public const string FIRSTLOAD_SCENARIO_ON_NEWGAME = "Main";

        // バックログの最大保有数
        public const int BACKLOG_MAX_HOLD_NUM = 60;
        // 1行あたりの文字の最大数
        public const int MAX_CHAR_NUM_PER_LINE = 30;
        
        // テキストスピードの最小Fixedフレーム数(0.005s * n)
        public const int TEXTSPEED_MIN_FIXED_FRAME_NUM = 1;
        // テキストスピードの最大Fixedフレーム数(0.005s * n)
        public const int TEXTSPEED_MAX_FIXED_FRAME_NUM = 20;

        // スクリーンショットを撮影するカメラ
        public const string SCREENSHOT_CAMERA_NAME = "1_ScreenShotCamera";
        // スクリーンショット画像のサイズ
        public static Vector2Int SCREENSHOT_SIZE = new Vector2Int(640, 360);
        // スクリーンショット画像を保存するフォルダ名
        public const string SCREENSHOT_SAVE_FOLDER = "savedata-thumbnail";

        // 1スロットに表示する最大文字数
        public const int SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_SLOT = 26;
        // 詳細画面に表示する最大文字数
        public const int SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_DETAIL = 75;

        // シーン名
        public const string SCENE_MAIN = "Main";

        public const string SCENE_TITLE = "Title";

        // Resourcesから読みだすパス
        public const string RESOURCES_PATH_SE = "SE/";

        // SEの同時再生数
        public const int SE_MAX_NUM = 10;

        /// <summary>
        /// ゲーム進行状態を保存する
        /// </summary>

        // Titleシーンでどのモードを選んで、Mainシーンに移行したかを保存しておく
        public enum LOAD_MODE
        {
            UNDEFINED,
            NEW_GAME,
            CONTINUE,
            LOAD
        }
        public static LOAD_MODE LoadMode = LOAD_MODE.UNDEFINED;
        
    }
}