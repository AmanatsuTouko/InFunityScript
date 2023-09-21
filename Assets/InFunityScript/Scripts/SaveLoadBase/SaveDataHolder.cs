using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FThingSoftware.InFunityScript
{
    // jsonファイルから読み込んでセーブデータを保持するクラス
    public class SaveDataHolder : MonoBehaviour
    {
        public static SaveDataHolder I { get; private set; }

        // 保持するデータ変数
        [SerializeField] private int PlayingDataSlotNum = 0;
        [SerializeField] private RebuildDatas rebuildDatas;
        [SerializeField] private ScenarioCallStacks scenarioCallStacks;
        [SerializeField] private DeveloperDefineVariables developerVariables;
        [SerializeField] private UserSettings userSettings;

        // 現在のゲームデータを保存するコンポーネント
        public SaveCurrentToPlayingData saveCurrent { get; private set; }
        // ロードしたデータから画面情報を復元するコンポーネント
        public RebuildFromLoadData rebuildFromLoadData { get; private set; }

        // Titleシーンからロードした際に、どのセーブデータをロードしたかを保存する
        private int slotNumOnLoadFromTitle = 0;

        private void Awake()
        {
            // シングルトンの設定
            if (I == null)
            {
                I = this;
                // 初回起動時以降はオブジェクトをシーン読み込み時に廃棄しないようにする
                DontDestroyOnLoad(this);
                // ローカルストレージからJSONファイルを読み込んで、変数に反映させる
                LoadJsonFromLocalStorage();
            }
            else
            {
                Destroy(this.gameObject);
            }

            // 同じGameObjectに付与されたコンポーネントの取得
            saveCurrent = GetComponent<SaveCurrentToPlayingData>();
            rebuildFromLoadData = GetComponent<RebuildFromLoadData>();
        }

        // Localsストレージからjsonファイルを読み込んでそれぞれの変数に代入する
        private void LoadJsonFromLocalStorage()
        {
            rebuildDatas = JsonSaveLoader<RebuildDatas>.Load(Settings.SAVE_FILE_NAME_REBUILD_DATA);
            scenarioCallStacks = JsonSaveLoader<ScenarioCallStacks>.Load(Settings.SAVE_FILE_NAME_SCENARIO_CALLSTACK);
            developerVariables = JsonSaveLoader<DeveloperDefineVariables>.Load(Settings.SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT);
            userSettings = JsonSaveLoader<UserSettings>.Load(Settings.SAVE_FILE_NAME_USER_SETTINGS);

            // セーブデータが存在しないときは新しくデータを作成してjsonファイルとして保存する
            CreateNewFileAndSaveJsonFile();
        }

        // セーブデータをjsonファイルに保存する
        private void SaveJsonToLocalStorage()
        {
            JsonSaveLoader<RebuildDatas>.Save(rebuildDatas, Settings.SAVE_FILE_NAME_REBUILD_DATA);
            JsonSaveLoader<ScenarioCallStacks>.Save(scenarioCallStacks, Settings.SAVE_FILE_NAME_SCENARIO_CALLSTACK);
            JsonSaveLoader<DeveloperDefineVariables>.Save(developerVariables, Settings.SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT);
            JsonSaveLoader<UserSettings>.Save(userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
        }

        // セーブデータが存在しないときは新しくデータを作成してjsonファイルとして保存する
        private void CreateNewFileAndSaveJsonFile() 
        {
            if (rebuildDatas == null)
            {
                // セーブデータ数分複製してリストに入れる
                rebuildDatas = new RebuildDatas();
                for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
                {
                    RebuildData data = new RebuildData();
                    data.Num = i;
                    rebuildDatas.Each.Add(data);
                }
                JsonSaveLoader<RebuildDatas>.Save(rebuildDatas, Settings.SAVE_FILE_NAME_REBUILD_DATA);
            }
            if (scenarioCallStacks == null)
            {
                // セーブデータ数分複製してリストに入れる
                scenarioCallStacks = new ScenarioCallStacks();
                for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
                {
                    ScenarioCallStack data = new ScenarioCallStack();
                    data.Num = i;
                    scenarioCallStacks.Each.Add(data);
                }
                JsonSaveLoader<ScenarioCallStacks>.Save(scenarioCallStacks, Settings.SAVE_FILE_NAME_SCENARIO_CALLSTACK);
            }
            if (developerVariables == null)
            {
                // セーブデータ数分複製してリストに入れる
                developerVariables = new DeveloperDefineVariables();
                for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
                {
                    DeveloperDefineVariable data = new DeveloperDefineVariable();
                    developerVariables.Each.Add(data);
                }
                JsonSaveLoader<DeveloperDefineVariables>.Save(developerVariables, Settings.SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT);
            }
            if (userSettings == null)
            {
                // 音量などはゲーム全体で持つ値なので複製しない
                userSettings = new UserSettings();
                JsonSaveLoader<UserSettings>.Save(userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
            }
        }

        // 保持しているセーブデータを全てリセットする（データ消去用）
        private void ResetSaveDataOnSaveDataHolder()
        {
            // セーブデータ数分複製してリストに入れる
            rebuildDatas = new RebuildDatas();
            for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
            {
                RebuildData data = new RebuildData();
                data.Num = i;
                rebuildDatas.Each.Add(data);
            }
            // セーブデータ数分複製してリストに入れる
            scenarioCallStacks = new ScenarioCallStacks();
            for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
            {
                ScenarioCallStack data = new ScenarioCallStack();
                data.Num = i;
                scenarioCallStacks.Each.Add(data);
            }
            // セーブデータ数分複製してリストに入れる
            developerVariables = new DeveloperDefineVariables();
            for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
            {
                DeveloperDefineVariable data = new DeveloperDefineVariable();
                developerVariables.Each.Add(data);
            }
            // 音量などはゲーム全体で持つ値なので複製しない
            userSettings = new UserSettings();
            JsonSaveLoader<UserSettings>.Save(userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
        }

        // ================
        // SAVE LOAD 処理用
        // ================

        // 指定したスロット番号に現在保持しているデータをディープコピーする（SAVE処理用）
        public void DeepCopyFromCurrentToSlotNum(int slotnum)
        {
            rebuildDatas.Each[slotnum] = rebuildDatas.Each[PlayingDataSlotNum].DeepCopy();
            rebuildDatas.Each[slotnum].Num = slotnum;
            scenarioCallStacks.Each[slotnum] = scenarioCallStacks.Each[PlayingDataSlotNum].DeepCopy();
            developerVariables.Each[slotnum] = developerVariables.Each[PlayingDataSlotNum].DeepCopy();
        }
        
        // 指定したスロット番号から現在保持しているデータにディープコピーする
        public void DeepCopyFromSlotNumToCurrent(int slotnum)
        {
            rebuildDatas.Each[PlayingDataSlotNum] = rebuildDatas.Each[slotnum].DeepCopy();
            rebuildDatas.Each[PlayingDataSlotNum].Num = PlayingDataSlotNum;
            scenarioCallStacks.Each[PlayingDataSlotNum] = scenarioCallStacks.Each[slotnum].DeepCopy();
            scenarioCallStacks.Each[PlayingDataSlotNum].Num = PlayingDataSlotNum;
            developerVariables.Each[PlayingDataSlotNum] = developerVariables.Each[slotnum].DeepCopy();
        }

        // スロット番号Nに現在のデータをセーブする
        public void SaveCurrentToSlotNum(int slotnum)
        {
            // 現在のゲームの状態をセーブデータとして保存できるように0番目のデータに更新する
            saveCurrent.SaveCurrent(slotnum);
            // 0番目に置いてあるデータをN番目にディープコピーする
            DeepCopyFromCurrentToSlotNum(slotnum);
            // Jsonファイルとしてローカルストレージに書き込む
            SaveJsonToLocalStorage();
        }
        // スロット番号Nからデータをロードしてゲームを再生する
        public void LoadAndStartScenarioFromSlotNum(int slotnum)
        {
            // N番目のデータが新規データの場合、何もしない
            if (rebuildDatas.Each[slotnum].ScenarioName == "") return;

            // N番目のデータから現在のデータにデータを上書きする
            DeepCopyFromSlotNumToCurrent(slotnum);
            // 復元データから表示キャラクターや背景情報などを復元する
            rebuildFromLoadData.Rebuild();
            // 復元データからシナリオ名と進行ページを取得する
            string scenarioName = rebuildDatas.Each[PlayingDataSlotNum].ScenarioName;
            int scenarioPage = rebuildDatas.Each[PlayingDataSlotNum].ScenarioPage;
            // ScenarioManagerのシナリオ進行をストップし
            // ScenarioManagerで新しくシナリオをスタートする
            // 進行ページが0以外の時は-1をしてシナリオをスタートさせる
            scenarioPage = (scenarioPage == 0 ? 0 : scenarioPage -2 );
            GameObject scenarioManager = GameObject.Find("ScenarioManager");
            scenarioManager.GetComponent<ScenarioManager>().StartScenario(scenarioName, scenarioPage);
            
            // LOAD画面を非表示にする
            scenarioManager.GetComponent<KeyInputManager>().ExitLoad();
        }

        // Titleシーンで続きから始める際に、スロット番号Nを保存する処理
        public void SaveSlotNumOnLoadTitle(int slotnum)
        {
            // N番目のデータが新規データの場合、何もしない
            if (rebuildDatas.Each[slotnum].ScenarioName == "") return;
            // slotnumを保存する
            slotNumOnLoadFromTitle = slotnum;
            // Titleシーンから遷移してきたフラグを立てる
            Settings.LoadFromTitleScene = true;
            // Mainシーンに移行する
            SceneManager.LoadScene("Main");
        }

        // Titleシーンから遷移して時にデータをLoadする
        public void StartScenarioFromTitleLoad()
        {
            // TitleからロードしてきたフラグをOFFにする
            Settings.LoadFromTitleScene = false;
            LoadAndStartScenarioFromSlotNum(slotNumOnLoadFromTitle);
        }

        // =======================================
        // SaveCurrentToPlayingDataから呼び出して使う関数群
        // =======================================
        // 現在のデータを読みだして、更新に用いる
        public RebuildData GetPlayingRebuildData()
        {
            return rebuildDatas.Each[0];
        }

        // =======================================
        // ScenarioManagerから呼び出して使う関数群
        // =======================================

        // ScenarioManagerにデータの値をそのまま使えるように整形して渡す
        // 単位はミリ秒
        // テキストに関しては1～20の値を取るようにして×5の値を表示する
        // 数値×0.005s(FixedUpdate)待機するようにする
        public int GetTextSpeedToScenarioManagerConvert()
        {
            // 最大を20,最小を1FixedFrameとする
            // セーブデータには、20(最大に高速)、1(最大に高速)として保存するため変換を行い
            // 20を引いた絶対値に1を足す
            return Mathf.Abs(userSettings.TextSpeed - Settings.TEXTSPEED_MAX_FIXED_FRAME_NUM) + Settings.TEXTSPEED_MIN_FIXED_FRAME_NUM;
        }
        public int GetWaitTimeSkipToScenarioManagerConvert()
        {
            int time = userSettings.SkipWaitTime;
            return (int)((100.0f - time) * 10.0f);
        }
        public int GetWaitTimeAutoToScenarioManagerConvert()
        {
            int time = userSettings.AutoWaitTime;
            return (int)(time * 10.0f * 5.0f);
        }

        // 現在読み込んでいるセーブスロットのCallStackに追加する
        public void AddScenarioCallStack(string scenarioName, int pageNum)
        {
            ScenarioData scenarioData = new ScenarioData(scenarioName, pageNum);
            scenarioCallStacks.Each[PlayingDataSlotNum].scenarioDatas.Add(scenarioData);
        }
        // 現在読み込んでいるセーブスロットのCallStackの末尾を取得して返す
        public ScenarioData PopScenarioCallStack()
        {
            List<ScenarioData> scenarioDatas = scenarioCallStacks.Each[PlayingDataSlotNum].scenarioDatas;
            // コールスタックが空の場合にはnullを返す
            if (scenarioDatas.Count == 0) return null;
            int lastIndex = scenarioDatas.Count - 1;
            // 複製して戻り値とする
            ScenarioData data = new ScenarioData(scenarioDatas[lastIndex].Name, scenarioDatas[lastIndex].Page);
            // ScenarioData data = scenarioDatas[lastIndex].DeepCopy();
            // 末尾のコールスタックを削除する
            scenarioDatas.RemoveAt(lastIndex);

            return data;
        }

        // ================================================
        // SettingLayerVolumeAdjustから呼び出して使う関数群
        // ================================================
        // Get
        public int GetTextSpeedToSettingLayerVolumeAdjust()
        {
            return userSettings.TextSpeed;
        }
        public int GetWaitTimeSkipToSettingLayerVolumeAdjust()
        {
            return userSettings.SkipWaitTime;
        }
        public int GetWaitTimeAutoToSettingLayerVolumeAdjust()
        {
            return userSettings.AutoWaitTime;
        }
        public int GetVolumeMasterToSettingLayerVolumeAdjust()
        {
            return userSettings.VolumeMaster;
        }
        public int GetVolumeBGMToSettingLayerVolumeAdjust()
        {
            return userSettings.VolumeBGM;
        }
        public int GetVolumeSEToSettingLayerVolumeAdjust()
        {
            return userSettings.VolumeSE;
        }
        // Set
        public void SetTextSpeedFromSettingLayerVolumeAdjust(int textSpeedFrame)
        {
            userSettings.TextSpeed = textSpeedFrame;
        }
        public void SetWaitTimeSkipFromSettingLayerVolumeAdjust(int waitTimeSkipMs)
        {
            userSettings.SkipWaitTime = waitTimeSkipMs;
        }
        public void SetWaitTimeAutoFromSettingLayerVolumeAdjust(int waitTimeAutoMs)
        {
            userSettings.AutoWaitTime = waitTimeAutoMs;
        }
        public void SetVolumeMasterFromSettingLayerVolumeAdjust(int volumeRate)
        {
            userSettings.VolumeMaster = volumeRate;
        }
        public void SetVolumeBGMFromSettingLayerVolumeAdjust(int volumeRate)
        {
            userSettings.VolumeBGM = volumeRate;
        }
        public void SetVolumeSEFromSettingLayerVolumeAdjust(int volumeRate)
        {
            userSettings.VolumeSE = volumeRate;
        }

        // 設定画面の退出時にセーブする
        // セーブデータをjsonファイルに保存する
        public void SaveUserSettingsToLocalStorage()
        {
            JsonSaveLoader<UserSettings>.Save(userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
        }

        // ================================================
        // SaveSlotから呼び出して使う関数群
        // ================================================
        // スロット番号からスクショのSpriteを返す
        public Sprite GetSpriteFromSlotNum(int slotnum)
        {
            // アクセスしたスロット番号が指定値を超える場合には何もしない
            if (slotnum >= Settings.SAVE_DATA_NUM) return null;

            // pathから画像データを読みだしてspriteを返す
            Sprite screenshotSprite = null;

            string screenshotFileName = rebuildDatas.Each[slotnum].ScreenShotFile;
            // スクリーンショットが未定義の時はアクセスしないでnullを返却する
            if (screenshotFileName == "") return screenshotSprite;

            string filePath = $"{Application.persistentDataPath}/{Settings.SCREENSHOT_SAVE_FOLDER}/{rebuildDatas.Each[slotnum].ScreenShotFile}";
            try
            {
                var rawData = System.IO.File.ReadAllBytes(filePath);
                Texture2D texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(rawData);
                screenshotSprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
            }
            catch (FileNotFoundException e)
            {
                Debug.LogErrorFormat("Error: Image File {0} is not exist.", filePath);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogErrorFormat("UnauthorizedAccessException: Access to the path {0} is denied.", filePath);
            }
            return screenshotSprite;
        }
        // スロット番号を返す
        public string GetSlotNumTextFromSlotNum(int slotnum)
        {
            // 1の時、01-01をリターンする
            // 12の時、01-12をリターンする
            // 13の時、02-01をリターンする
            int firstDigit = (slotnum - 1) / Settings.SAVE_SLOT_NUM_OF_PAGE + 1;
            int secondDigit = slotnum % Settings.SAVE_SLOT_NUM_OF_PAGE;
            if (secondDigit == 0) secondDigit = Settings.SAVE_SLOT_NUM_OF_PAGE;

            return $"{firstDigit.ToString("D2")}-{secondDigit.ToString("D2")}";
        }

        // MainTextを1つの小さいセーブスロット用に短くして返す
        public string GetMainTextForOneSlotFromSlotNum(int slotnum)
        {
            // アクセスしたスロット番号が指定値を超える場合には何もしない
            if (slotnum >= Settings.SAVE_DATA_NUM) return null;

            // データの読み出し
            string mainText = rebuildDatas.Each[slotnum].TextMain;

            // 指定文字数以下の場合はそのまま返す
            if (mainText.Length <= Settings.SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_SLOT) return mainText;

            // 指定文字数まで削って返す
            List<char> shotedText = new List<char>();
            for(int i=0; i<Settings.SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_SLOT; i++)
            {
                shotedText.Add(mainText[i]);
            }
            return string.Join("", shotedText);
        }
        // MainTextを詳細画面用に短くして返す
        public string GetMainTextForDetailFromSlotNum(int slotnum)
        {
            // アクセスしたスロット番号が指定値を超える場合には何もしない
            if (slotnum >= Settings.SAVE_DATA_NUM) return null;

            // データの読み出し
            string mainText = rebuildDatas.Each[slotnum].TextMain;

            // 指定文字数以下の場合はそのまま返す
            if (mainText.Length <= Settings.SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_DETAIL) return mainText;

            // 指定文字数まで削って返す
            List<char> shotedText = new List<char>();
            for (int i = 0; i < Settings.SAVE_SLOT_MAIN_TEXT_MAX_NUM_FOR_DETAIL; i++)
            {
                shotedText.Add(mainText[i]);
            }
            return string.Join("", shotedText);
        }
        // 時刻を返す
        public string GetDateFromSlotNum(int slotnum)
        {
            // アクセスしたスロット番号が指定値を超える場合には何もしない
            if (slotnum >= Settings.SAVE_DATA_NUM) return null;
            return rebuildDatas.Each[slotnum].Date;
        }
        // Chapterを返す
        public string GetChapterFromSlotNum(int slotnum)
        {
            // アクセスしたスロット番号が指定値を超える場合には何もしない
            if (slotnum < 0 && slotnum >= Settings.SAVE_DATA_NUM) return "";
            return rebuildDatas.Each[slotnum].Chapter;
        }
    }
}