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
        // 復元用データ(配置しているキャラクターや背景など)
        [SerializeField] private RebuildDatas _rebuildDatas;
        // callstack
        [SerializeField] private ScenarioCallStacks _scenarioCallStacks;
        // ユーザー定義変数
        [SerializeField] private DeveloperDefineVariables _developerVariables;
        // プレイヤーの設定パラメータ(音量やテキストスピードなど)
        [SerializeField] private UserSettings _userSettings;

        // 現在のゲームデータを保存するコンポーネント
        public SaveCurrentToPlayingData _saveCurrent { get; private set; }
        // ロードしたデータから画面情報を復元するコンポーネント
        public RebuildFromLoadData _rebuildFromLoadData { get; private set; }

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
            _saveCurrent = GetComponent<SaveCurrentToPlayingData>();
            _rebuildFromLoadData = GetComponent<RebuildFromLoadData>();

            // シーン読み込みごとに初期化する関数の設定
            InitWhenSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        void InitWhenSceneLoad(Scene nextScene, LoadSceneMode mode)
        {
            // 参照をMainシーンで読み込みごとに再取得するようにする
            if(nextScene.name == Settings.SCENE_MAIN){
                _saveCurrent.Init();
                _rebuildFromLoadData.Init();
            }

            // 再度設定しておく
            SceneManager.sceneLoaded += InitWhenSceneLoad;
        }

        // Localsストレージからjsonファイルを読み込んでそれぞれの変数に代入する
        private void LoadJsonFromLocalStorage()
        {
            _rebuildDatas = JsonSaveLoader<RebuildDatas>.Load(Settings.SAVE_FILE_NAME_REBUILD_DATA);
            _scenarioCallStacks = JsonSaveLoader<ScenarioCallStacks>.Load(Settings.SAVE_FILE_NAME_SCENARIO_CALLSTACK);
            _developerVariables = JsonSaveLoader<DeveloperDefineVariables>.Load(Settings.SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT);
            _userSettings = JsonSaveLoader<UserSettings>.Load(Settings.SAVE_FILE_NAME_USER_SETTINGS);

            // セーブデータが存在しないときは新しくデータを作成してjsonファイルとして保存する
            CreateNewFileAndSaveJsonFile();
        }

        // セーブデータをjsonファイルに保存する
        private void SaveJsonToLocalStorage()
        {
            JsonSaveLoader<RebuildDatas>.Save(_rebuildDatas, Settings.SAVE_FILE_NAME_REBUILD_DATA);
            JsonSaveLoader<ScenarioCallStacks>.Save(_scenarioCallStacks, Settings.SAVE_FILE_NAME_SCENARIO_CALLSTACK);
            JsonSaveLoader<DeveloperDefineVariables>.Save(_developerVariables, Settings.SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT);
            JsonSaveLoader<UserSettings>.Save(_userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
        }

        // セーブデータが存在しないときは新しくデータを作成してjsonファイルとして保存する
        private void CreateNewFileAndSaveJsonFile() 
        {
            if (_rebuildDatas == null)
            {
                // セーブデータ数分複製してリストに入れる
                _rebuildDatas = new RebuildDatas();
                for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
                {
                    RebuildData data = new RebuildData();
                    data.Num = i;
                    _rebuildDatas.Each.Add(data);
                }
                JsonSaveLoader<RebuildDatas>.Save(_rebuildDatas, Settings.SAVE_FILE_NAME_REBUILD_DATA);
            }
            if (_scenarioCallStacks == null)
            {
                // セーブデータ数分複製してリストに入れる
                _scenarioCallStacks = new ScenarioCallStacks();
                for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
                {
                    ScenarioCallStack data = new ScenarioCallStack();
                    data.Num = i;
                    _scenarioCallStacks.Each.Add(data);
                }
                JsonSaveLoader<ScenarioCallStacks>.Save(_scenarioCallStacks, Settings.SAVE_FILE_NAME_SCENARIO_CALLSTACK);
            }
            if (_developerVariables == null)
            {
                // セーブデータ数分複製してリストに入れる
                _developerVariables = new DeveloperDefineVariables();
                for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
                {
                    DeveloperDefineVariable data = new DeveloperDefineVariable();
                    _developerVariables.Each.Add(data);
                }
                JsonSaveLoader<DeveloperDefineVariables>.Save(_developerVariables, Settings.SAVE_FILE_NAME_DEVELOPER_DEFINE_DICT);
            }
            if (_userSettings == null)
            {
                // 音量などはゲーム全体で持つ値なので複製しない
                _userSettings = new UserSettings();
                JsonSaveLoader<UserSettings>.Save(_userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
            }
        }

        // 保持しているセーブデータを全てリセットする（データ消去用）
        private void ResetSaveDataOnSaveDataHolder()
        {
            // セーブデータ数分複製してリストに入れる
            _rebuildDatas = new RebuildDatas();
            for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
            {
                RebuildData data = new RebuildData();
                data.Num = i;
                _rebuildDatas.Each.Add(data);
            }
            // セーブデータ数分複製してリストに入れる
            _scenarioCallStacks = new ScenarioCallStacks();
            for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
            {
                ScenarioCallStack data = new ScenarioCallStack();
                data.Num = i;
                _scenarioCallStacks.Each.Add(data);
            }
            // セーブデータ数分複製してリストに入れる
            _developerVariables = new DeveloperDefineVariables();
            for (int i = 0; i < Settings.SAVE_DATA_NUM; i++)
            {
                DeveloperDefineVariable data = new DeveloperDefineVariable();
                _developerVariables.Each.Add(data);
            }
            // 音量などはゲーム全体で持つ値なので複製しない
            _userSettings = new UserSettings();
            JsonSaveLoader<UserSettings>.Save(_userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
        }

        // ================
        // SAVE LOAD 処理用
        // ================

        // 指定したスロット番号に現在保持しているデータをディープコピーする（SAVE処理用）
        public void DeepCopyFromCurrentToSlotNum(int slotnum)
        {
            // 復元用データ
            _rebuildDatas.Each[slotnum] = _rebuildDatas.Each[PlayingDataSlotNum].DeepCopy();
            _rebuildDatas.Each[slotnum].Num = slotnum;
            // callstask
            _scenarioCallStacks.Each[slotnum] = _scenarioCallStacks.Each[PlayingDataSlotNum].DeepCopy();
            _scenarioCallStacks.Each[slotnum].Num = slotnum;
            // ユーザー定義変数
            _developerVariables.Each[slotnum] = _developerVariables.Each[PlayingDataSlotNum].DeepCopy();
        }
        
        // 指定したスロット番号から現在保持しているデータにディープコピーする
        public void DeepCopyFromSlotNumToCurrent(int slotnum)
        {
            _rebuildDatas.Each[PlayingDataSlotNum] = _rebuildDatas.Each[slotnum].DeepCopy();
            _rebuildDatas.Each[PlayingDataSlotNum].Num = PlayingDataSlotNum;
            _scenarioCallStacks.Each[PlayingDataSlotNum] = _scenarioCallStacks.Each[slotnum].DeepCopy();
            _scenarioCallStacks.Each[PlayingDataSlotNum].Num = PlayingDataSlotNum;
            _developerVariables.Each[PlayingDataSlotNum] = _developerVariables.Each[slotnum].DeepCopy();
        }

        // スロット番号Nに現在のデータをセーブする
        public void SaveCurrentToSlotNum(int slotnum)
        {
            // 現在のゲームの状態をセーブデータとして保存できるように0番目のデータに更新する
            _saveCurrent.SaveCurrent(slotnum);
            // 0番目に置いてあるデータをN番目にディープコピーする
            DeepCopyFromCurrentToSlotNum(slotnum);
            // Jsonファイルとしてローカルストレージに書き込む
            SaveJsonToLocalStorage();
        }
        // スロット番号Nからデータをロードしてゲームを再生する
        public async void LoadAndStartScenarioFromSlotNum(int slotnum)
        {
            // N番目のデータが新規データの場合、何もしない
            if (_rebuildDatas.Each[slotnum].ScenarioName == "") return;

            // N番目のデータから現在のデータにデータを上書きする
            DeepCopyFromSlotNumToCurrent(slotnum);
            // 復元データから表示キャラクターや背景情報などを復元する
            await _rebuildFromLoadData.RebuildAsync();

            GameObject scenarioManager = GameObject.Find("ScenarioManager");

            // 復元データからシナリオ名と進行ページを取得する
            string scenarioName = _rebuildDatas.Each[PlayingDataSlotNum].ScenarioName;
            int scenarioPage = _rebuildDatas.Each[PlayingDataSlotNum].ScenarioPage;
            
            // ScenarioManagerのシナリオ進行をストップし
            // ScenarioManagerで新しくシナリオをスタートする
            // 進行ページが0以外の時は-1をしてシナリオをスタートさせる
            scenarioPage = (scenarioPage == 0 ? 0 : scenarioPage -2 );
            scenarioManager.GetComponent<ScenarioManager>().StartScenario(scenarioName, scenarioPage);
            
            // LOAD画面を非表示にする
            scenarioManager.GetComponent<KeyInputManager>().ExitLoad();
        }

        // Titleシーンで続きから始める際に、スロット番号Nを保存する処理
        public void SaveSlotNumOnLoadTitle(int slotnum)
        {
            // N番目のデータが新規データの場合、何もしない
            if (_rebuildDatas.Each[slotnum].ScenarioName == "") return;
            // slotnumを保存する
            slotNumOnLoadFromTitle = slotnum;
            // Titleシーンから遷移してきたフラグを立てる
            Settings.LoadMode = Settings.LOAD_MODE.LOAD;
            // Mainシーンに移行する
            SceneManager.LoadScene("Main");
        }

        // Titleシーンから遷移して時にデータをLoadする
        public void StartScenarioLoadMode()
        {
            LoadAndStartScenarioFromSlotNum(slotNumOnLoadFromTitle);
        }

        // 続きからを選択した場合に、0番目のデータを読みだしてスタートする
        public void StartScenarioContinueMode()
        {
            LoadAndStartScenarioFromSlotNum(0);
        }

        // =======================================
        // SaveCurrentToPlayingDataから呼び出して使う関数群
        // =======================================
        // 現在のデータを読みだして、更新に用いる
        public RebuildData GetPlayingRebuildData()
        {
            return _rebuildDatas.Each[0];
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
            return Mathf.Abs(_userSettings.TextSpeed - Settings.TEXTSPEED_MAX_FIXED_FRAME_NUM) + Settings.TEXTSPEED_MIN_FIXED_FRAME_NUM;
        }
        public int GetWaitTimeSkipToScenarioManagerConvert()
        {
            int time = _userSettings.SkipWaitTime;
            return (int)((100.0f - time) * 10.0f);
        }
        public int GetWaitTimeAutoToScenarioManagerConvert()
        {
            int time = _userSettings.AutoWaitTime;
            return (int)(time * 10.0f * 5.0f);
        }

        // 現在読み込んでいるセーブスロットのCallStackに追加する
        public void AddScenarioCallStack(string scenarioName, int pageNum)
        {
            ScenarioData scenarioData = new ScenarioData(scenarioName, pageNum);
            _scenarioCallStacks.Each[PlayingDataSlotNum].scenarioDatas.Add(scenarioData);
        }
        // 現在読み込んでいるセーブスロットのCallStackの末尾を取得して返す
        public ScenarioData PopScenarioCallStack()
        {
            List<ScenarioData> scenarioDatas = _scenarioCallStacks.Each[PlayingDataSlotNum].scenarioDatas;
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
            return _userSettings.TextSpeed;
        }
        public int GetWaitTimeSkipToSettingLayerVolumeAdjust()
        {
            return _userSettings.SkipWaitTime;
        }
        public int GetWaitTimeAutoToSettingLayerVolumeAdjust()
        {
            return _userSettings.AutoWaitTime;
        }
        public int GetVolumeMasterToSettingLayerVolumeAdjust()
        {
            return _userSettings.VolumeMaster;
        }
        public int GetVolumeBGMToSettingLayerVolumeAdjust()
        {
            return _userSettings.VolumeBGM;
        }
        public int GetVolumeSEToSettingLayerVolumeAdjust()
        {
            return _userSettings.VolumeSE;
        }
        // Set
        public void SetTextSpeedFromSettingLayerVolumeAdjust(int textSpeedFrame)
        {
            _userSettings.TextSpeed = textSpeedFrame;
        }
        public void SetWaitTimeSkipFromSettingLayerVolumeAdjust(int waitTimeSkipMs)
        {
            _userSettings.SkipWaitTime = waitTimeSkipMs;
        }
        public void SetWaitTimeAutoFromSettingLayerVolumeAdjust(int waitTimeAutoMs)
        {
            _userSettings.AutoWaitTime = waitTimeAutoMs;
        }
        public void SetVolumeMasterFromSettingLayerVolumeAdjust(int volumeRate)
        {
            _userSettings.VolumeMaster = volumeRate;
        }
        public void SetVolumeBGMFromSettingLayerVolumeAdjust(int volumeRate)
        {
            _userSettings.VolumeBGM = volumeRate;
        }
        public void SetVolumeSEFromSettingLayerVolumeAdjust(int volumeRate)
        {
            _userSettings.VolumeSE = volumeRate;
        }

        // 設定画面の退出時にセーブする
        // セーブデータをjsonファイルに保存する
        public void SaveUserSettingsToLocalStorage()
        {
            JsonSaveLoader<UserSettings>.Save(_userSettings, Settings.SAVE_FILE_NAME_USER_SETTINGS);
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

            string screenshotFileName = _rebuildDatas.Each[slotnum].ScreenShotFile;
            // スクリーンショットが未定義の時はアクセスしないでnullを返却する
            if (screenshotFileName == "") return screenshotSprite;

            string filePath = $"{Application.persistentDataPath}/{Settings.SCREENSHOT_SAVE_FOLDER}/{_rebuildDatas.Each[slotnum].ScreenShotFile}";
            try
            {
                var rawData = System.IO.File.ReadAllBytes(filePath);
                Texture2D texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(rawData);
                screenshotSprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
            }
            catch (FileNotFoundException e)
            {
                Debug.LogErrorFormat("{0} : Image File {1} is not exist.", e, filePath);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.LogErrorFormat("{0} : Access to the path {1} is denied.", e, filePath);
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
            string mainText = _rebuildDatas.Each[slotnum].TextMain;

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
            string mainText = _rebuildDatas.Each[slotnum].TextMain;

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
            return _rebuildDatas.Each[slotnum].Date;
        }
        // Chapterを返す
        public string GetChapterFromSlotNum(int slotnum)
        {
            // アクセスしたスロット番号が指定値を超える場合には何もしない
            if (slotnum < 0 && slotnum >= Settings.SAVE_DATA_NUM) return "";
            return _rebuildDatas.Each[slotnum].Chapter;
        }

        // デバッグ用
        // Unityエディター上で、シナリオファイルをデバッグする際に無限にcallstackが増えてしまうので、実行前にcallstackを消す
        public void ResetCurrentCallStack()
        {
            _scenarioCallStacks.Each[0].scenarioDatas.Clear();
        }

        // TitleシーンからNewGameをするときに
        // SaveDataHolderの0番目のデータのRebuildData、CallStacksを消す
        public void ResetDataForContinue()
        {
            // 初期化
            _scenarioCallStacks.Each[0].Num = 0;
            _scenarioCallStacks.Each[0].scenarioDatas.Clear();
            // 初期化
            _rebuildDatas.Each[0] = new RebuildData();
        }

        // Scenarioファイルからユーザー定義変数を書き換えできるようにする
        public DeveloperDefineVariable DevDefVariable{
            get 
            {
                return _developerVariables.Each[0];
            }
            set
            {
                _developerVariables.Each[0] = value;
            }
        }
    }
}