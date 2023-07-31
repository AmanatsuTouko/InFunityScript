using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class Settings : MonoBehaviour
    {
        // Define Settings Parameta
        // 0番目は現状のデータとして使うため、12n+1の数にする
        public static int SAVE_DATA_NUM = 85;
        // 1ページに表示するセーブデータの数
        public static int SAVE_SLOT_NUM_OF_PAGE = 12;

        // セーブに用いるjsonファイルの名前
        public static string SAVE_FILE_NAME_USER_SETTINGS         = "user-settings";
        public static string SAVE_FILE_NAME_REBUILD_DATA          = "rebuild-data";
        public static string SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT = "developer-define-dict";
        public static string SAVE_FILE_NAME_SCENARIO_CALLSTACK     = "scenario-callstack";

        // NewGame時に読み込むシナリオ
        public static string FIRSTLOAD_SCENARIO_ON_NEWGAME = "Main";

        // バックログの最大保有数
        public static int BACKLOG_MAX_HOLD_NUM = 60;
        // 1行あたりの文字の最大数
        public static int MAX_CHAR_NUM_PER_LINE = 30;
        
        // テキストスピードの最小Fixedフレーム数(0.005s * n)
        public static int TEXTSPEED_MIN_FIXED_FRAME_NUM = 1;
        // テキストスピードの最大Fixedフレーム数(0.005s * n)
        public static int TEXTSPEED_MAX_FIXED_FRAME_NUM = 20;

        // スクリーンショットを撮影するカメラ
        public static string SCREENSHOT_CAMERA_NAME = "1_ScreenShotCamera";
        // スクリーンショット画像のサイズ
        public static Vector2Int SCREENSHOT_SIZE = new Vector2Int(640, 360);
        // スクリーンショット画像を保存するフォルダ名
        public static string SCREENSHOT_SAVE_FOLDER = "savedata-thumbnail";

        // 1スロットに表示する最大文字数
        public static int SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_SLOT = 26;
        // 詳細画面に表示する最大文字数
        public static int SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_DETAIL = 75;
    }
}