using System.Collections;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace FThingSoftware.InFunityScript
{
    public class ScenarioManager : MonoBehaviour
    {
        // ゲーム編集中にMainシーンで起動したとき、初めに読み込むシナリオファイル名
        [SerializeField] string _debugScenario = "Main";
        [SerializeField] int _debugScenarioPage = 0;

        // 現在再生しているシナリオコンポーネント
        private Scenario playingScenarioComponent;
        // 現在のシナリオのページ数
        public int pageNum = 0;
        // ロードに用いるページ数
        private int remainSkipNum = 0;

        // ラベル探索に用いるシナリオコンポ―ネント
        private Scenario labelGetScenarioComponent;
        // ラベル探索に用いるページ数
        private int labelPageNum = 0;
        // 探索するラベル名
        private string findLabelName = "";

        // Text,Skip,Auto時の待ち時間
        public int WAIT_TIME_TEXT { get; private set; }
        private int WAIT_TIME_SKIP = 0;
        private int WAIT_TIME_AUTO = 0;

        // キー入力を纏めて担うクラス
        private KeyInputManager keyInput;
        // シナリオコマンドを纏めておくクラス
        private ScenarioCommands sc;
        // シナリオ進行に用いる真偽値変数を纏めておくクラス
        private ScenarioBooleans sb;

        private void Awake()
        {
            // キー入力は別のクラスに纏める
            keyInput = GetComponent<KeyInputManager>();
            // 基本的なコマンド以外のシナリオコマンドは別ファイルに記載し、読みだすようにする
            sc = GetComponent<ScenarioCommands>();
            // シナリオ進行に用いる真偽値は別ファイルに定義して管理する
            sb = GetComponent<ScenarioBooleans>();
        }

        private void Start()
        {
            // Text,Skip,Auto時の待ち時間をセーブデータから反映させる
            UpdateWaitTimeFromSaveData();

            // シナリオのスタート
            switch (Settings.LoadMode)
            {
                // Titleシーンから
                // 初めからを選択したとき
                case Settings.LOAD_MODE.NEW_GAME:
                    StartScenario(Settings.FIRSTLOAD_SCENARIO_ON_NEWGAME, 0);
                    break;
                // 続きからを選択したとき
                case Settings.LOAD_MODE.CONTINUE:
                    SaveDataHolder.I.StartScenarioContinueMode();
                    break;
                // Loadしてきたとき
                case Settings.LOAD_MODE.LOAD:
                    SaveDataHolder.I.StartScenarioLoadMode();
                    break;
                // それ以外の時
                default:
                // 主にUnityエディターからデバッグ用に実行した時
                # if UNITY_EDITOR
                    // callStackをリセットしてからスタート
                    SaveDataHolder.I.ResetCurrentCallStack();
                    StartScenario(_debugScenario, _debugScenarioPage);
                # endif
                    break;
            }
            
            // Titleからどのモードを選択してシーン移行したかを初期化する
            Settings.LoadMode = Settings.LOAD_MODE.UNDEFINED;
        }

        // Text,Skip,Auto時の待ち時間をセーブデータから反映させる
        public void UpdateWaitTimeFromSaveData()
        {
            WAIT_TIME_TEXT = SaveDataHolder.I.GetTextSpeedToScenarioManagerConvert();
            WAIT_TIME_AUTO = SaveDataHolder.I.GetWaitTimeAutoToScenarioManagerConvert();
            WAIT_TIME_SKIP = SaveDataHolder.I.GetWaitTimeSkipToScenarioManagerConvert();
        }

        // ==========================================
        // 以下、基本的な進行を行うためのシナリオ命令
        // ==========================================

        // 次のシーンファイルを設定して開始する
        public void StartScenario(string scenarioName, int nowPageNum = 0)
        {
            // ページ進行度のリセット
            // callReturnで帰るときには、nowPageをコールする前の位置まで復元する
            this.pageNum = nowPageNum;
            this.remainSkipNum = nowPageNum;

            Type scenarioClass = Type.GetType(scenarioName);
            
            if (scenarioClass != null)
            {
                // 現在再生しているシナリオコンポーネントのCancelTokenをOFFにする
                playingScenarioComponent?.tokenSource.Cancel();
                // 現在再生しているシナリオコンポーネントの削除
                Destroy(playingScenarioComponent);

                // 新しいシナリオコンポーネントの追加
                playingScenarioComponent = (Scenario)gameObject.AddComponent(scenarioClass);
                // ScenarioクラスからScenarioManager/ScenarioCommandsを参照するための参照付け
                playingScenarioComponent.SetScenarioManagerAndCommands(this, sc);

                // キャンセル用のトークンを発行し、コンポーネントに保存しておく
                CancellationTokenSource cts = new CancellationTokenSource();
                playingScenarioComponent.tokenSource = cts;
                playingScenarioComponent.token = cts.Token;
                // 次のシナリオの開始
                playingScenarioComponent.Content().Forget();
            }
            else
            {
                Debug.LogError($"Error: Scene File {scenarioName} is not found.");
            }

            // 全てのButtonPrefabの削除
            //foreach (Transform n in SelectButtonLayer.transform) { GameObject.Destroy(n.gameObject); }
        }

        // セーブロードできるように、進行ページの加算とスルー処理をする
        // セーブのために、進行ページを加算する
        // ロード時にはロード箇所までシナリオ関数を無視するためにfalseを返す
        public bool isDoTask()
        {
            // ラベル探索中は、実行せずカウンタを増加させる
            if (sb.isGettingLabel)
            {
                labelPageNum += 1;
                return false;
            }
            // ロード時はカウンタを減少させて実行しないようにfalseを返す
            if (remainSkipNum > 0)
            {
                remainSkipNum -= 1;
                return false;
            }
            // ロード処理中でない時は進行ページ数を加算し、trueを返す
            else
            {
                pageNum += 1;
                return true;
            }
        }
        // sys_label用に、ラベル探索中も実行する関数
        public bool isDoTaskOfSysLabel()
        {
            // ラベル探索中も実行する
            if (sb.isGettingLabel)
            {
                labelPageNum += 1;
                return true; // ここだけ変更
            }
            // ロード時はカウンタを減少させて実行しないようにfalseを返す
            if (remainSkipNum > 0)
            {
                remainSkipNum -= 1;
                return false;
            }
            // ロード処理中でない時は進行ページ数を加算し、trueを返す
            else
            {
                pageNum += 1;
                return true;
            }
        }

        // 実行を保存しないコマンド
        // 選択肢ボタン関連
        public bool isDoTaskOfSelectButton()
        {
            // ラベル探索中は実行しない
            if (sb.isGettingLabel)
            {
                return false;
            }
            // ロード時は実行しないようにする
            if (remainSkipNum > 0)
            {
                return false;
            }
            // それ以外の時は実行する
            else
            {
                return true;
            }
        }


        // クリックしたら終了するタスク
        public async UniTask WaitClick(UniTask task)
        {
            // テキストが最後まで表示されクリック待ちのときに、スキップやオートボタンを押すと次のページに進むフラグを無効にする
            if (sb.isNextPageForce_WhenPressSkipOrAuto_WhileWaitClick) sb.isNextPageForce_WhenPressSkipOrAuto_WhileWaitClick = false;

            // シナリオ関数の実行を待機する
            await task;

            // クリック待ち処理
            if (sb.isSkipping)
            {
                // スキップ中の時間間隔
                await UniTask.Delay(WAIT_TIME_SKIP);
            }
            else if (sb.isAutoPlaying)
            {
                // オート中の次ページまで待機する時間分待つ
                float SumTime = 0.0f;
                while (true)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update);
                    SumTime += Time.deltaTime;
                    if (SumTime > (WAIT_TIME_AUTO / 1000.0f))
                    {
                        // クリック待ち中で待機中にオートモードが解除された場合は通常のクリック入力を待つ
                        if(!sb.isAutoPlaying) await UniTask.WaitUntil(() => IsClickOnWaitClick());
                        break;
                    }
                    // クリックがある場合は進む
                    if (keyInput.IsClick()) break;
                }
            }
            else
            {
                // スキップ中でもオート中でもない時は、テキストのクリック待ちと実際のクリックを待つ
                await UniTask.WaitUntil(() => IsClickOnWaitClick());
            }
            // 同フレームで戻るとsb.isAddingTextがtrueになり、テキスト更新中にクリックして全文表示をするUpdate関数での入力をしてしまうので1フレーム遅延させる
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        // 強制ページ進行や、システム画面の表示中はクリックを検知しないようにする関数
        private bool IsClickOnWaitClick()
        {
            return keyInput.IsClick() && sb.isWaitClick && !IsDisplaySystemPanels() && sb.isDisplayTextWindow || sb.isNextPageForce_WhenPressSkipOrAuto_WhileWaitClick;
        }
        private bool IsDisplaySystemPanels()
        {
            //return isMouseOverOnSystemButtons || IsDisplaySystemPanels || isDisplayTextWindow;
            return sb.isDisplayBacklog || sb.isDisplaySettings || sb.isMouseOverOnSystemButtons || sb.isDisplaySaveLoadPanel;
        }

        // セーブロードできるように、指定した箇所までの処理をスキップするだけのタスク
        // 名前の変更など、セーブ復帰時やコールリターン時に実行したくない処理に挟む
        // 背景の変更や、キャラ表情の変更などにも挟む
        public async UniTask NoWaitClick(UniTask scenarioTask)
        {
            // シナリオ関数の実行を待機する
            await scenarioTask;
        }

        // Call処理
        public async UniTask Call(string nextScenarioName)
        {
            // 現在読み込んでいるシナリオ名を取得
            string playingScenarioName = playingScenarioComponent.GetType().Name;
            // セーブデータのコールスタックに追加する
            SaveDataHolder.I.AddScenarioCallStack(playingScenarioName, pageNum);
            // 次のシーンを読み込む
            StartScenario(nextScenarioName);
            return;
        }

        // CallReturn処理
        public async UniTask CallReturn()
        {
            // 1つ前のコールスタックにあるシナリオ名と進行ページ数を取得する
            ScenarioData scenarioData = SaveDataHolder.I.PopScenarioCallStack();
            // コールスタックが空の時は何もしない
            if (scenarioData == null) return;
            // 中断箇所からシナリオを再開する
            StartScenario(scenarioData.Name, scenarioData.Page);
            return;
        }

        // 指定したラベルを見つけて、labelPageNumを求める
        public async UniTask FindLabelAndJumpScenario(string scenarioName, string labelName)
        {
            // ページ進行度のリセット
            this.labelPageNum = 0;
            // 探索ラベルの設定
            this.findLabelName = labelName;
            
            // 子オブジェクトに指定したシナリオコンポーネントをAddする
            Type scenarioClass = Type.GetType(scenarioName);
            if (scenarioClass != null)
            {
                // ラベル探索フラグをONにする
                sb.isGettingLabel = true;

                // ラベル探索フラグをONにしたことを同期をとるために1フレーム待機する
                await UniTask.Yield();

                // 新しいシナリオコンポーネントの追加
                labelGetScenarioComponent = (Scenario)gameObject.AddComponent(scenarioClass);
                // ScenarioクラスからScenarioManager/ScenarioCommandsを参照するための参照付け
                labelGetScenarioComponent.SetScenarioManagerAndCommands(this, sc);
                // キャンセル用のトークンを発行し、コンポーネントに保存しておく
                CancellationTokenSource cts = new CancellationTokenSource();
                labelGetScenarioComponent.tokenSource = cts;
                labelGetScenarioComponent.token = cts.Token;

                // ラベル探索のため、シナリオを開始する
                labelGetScenarioComponent.Content().Forget();

#if UNITY_EDITOR
                // UnityEditorの場合
                // ラベルが見つからなかった場合のエラー出力処理を非同期で起動しておく
                ErrorHundle(labelName, scenarioName);
#endif

                // ラベル探索が終了するまで待機する
                // このタイミングでlabelNumが更新される
                await UniTask.WaitWhile(() => sb.isGettingLabel);              

                // 探索シナリオコンポーネントのCancelTokenをOFFにする
                labelGetScenarioComponent?.tokenSource.Cancel();

                // 探索シナリオコンポーネントの削除
                Destroy(labelGetScenarioComponent);
                
                // ジャンプ処理を実行する
                StartScenario(scenarioName, labelPageNum);

                Debug.Log($"labelPageNum:{labelPageNum}");
            }
            else
            {
                Debug.LogError($"Error: Scene File {scenarioName} is not found.");
            }
        }

        // 何秒か待機してLabelPageが更新されなかった際には、エラーを出して停止する
        async UniTask ErrorHundle(string labelName, string scenarioName)
        {
            // labelPageNumの更新を観測する
            // 1秒間経って変化しなくなった場合(探索の終了)に終了する
            int preLabelPage = 0;
            while (labelPageNum != preLabelPage)
            {
                await UniTask.Delay(1000);
                preLabelPage = labelPageNum;
            }
            // 継続してラベル探索中だった（ラベルが見つからなかった）場合にエラーを出力して停止する
            if (sb.isGettingLabel)
            {
                Debug.LogError($"Error: Label {labelName} in {scenarioName} Sccenario is not found.");
                // taskへ停止の通知
                playingScenarioComponent?.tokenSource.Cancel();
            }
        }

        // 探しているラベル名と一致した場合に終了するタスク
        public async UniTask FinishFindLabel(string name)
        {
            // ラベル探索時以外は何もしない
            if (!sb.isGettingLabel) { return; }
            // 探してるラベル名と一致した時、タスクチェーンを停止して行番号を確定する
            if (findLabelName == name)
            {
                // 探索処理の終了フラグ
                sb.isGettingLabel = false;
            }
        }

        // SaveCurrentToPlayingData.csからシナリオ名と進行ページを取得するための関数
        public string GetScenarioName()
        {
            return playingScenarioComponent.GetType().Name;
        }
        public int GetScenarioPage()
        {
            return pageNum;
        }

        // シナリオをストップする
        // 選択肢ボタンの表示後などに、進行をストップする際に用いる
        public void CancelScenarioProceed()
        {
            playingScenarioComponent.tokenSource.Cancel();
        }
    }
}