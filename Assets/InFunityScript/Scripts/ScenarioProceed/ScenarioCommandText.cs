using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace FThingSoftware.InFunityScript
{
    public class ScenarioCommandText : MonoBehaviour
    {
        private ScenarioManager sm;
        private ScenarioBooleans sb;

        private void Awake()
        {
            sm = GetComponent<ScenarioManager>();
            sb = GetComponent<ScenarioBooleans>();
        }

        // テキストウィンドウの文字オブジェクト
        [SerializeField] private TextMeshProUGUI textWindowMain;
        [SerializeField] private TextMeshProUGUI textWindowTalker;
        public string TalkerName { get; private set; }
        public string TextAllOfAddingText { get; private set; }

        // バックログ
        [SerializeField] private TextMeshProUGUI backlogTextMain;
        [SerializeField] private TextMeshProUGUI backlogTextTalker;
        // 指定した保存数を超えると一部削除するために用いる
        private List<string> backlogTextMainList = new List<string>();
        private List<string> backlogTextTalkerList = new List<string>();

        // テキスト表示に何行用いたか（バックログに用いる）
        private int lineNum = 1;

        // テキスト追加を行う
        private bool _isAddText = false;
        int _textIdx = 0;
        string _addText = "";

        int _waitFrameCount = 1;

        private void FixedUpdate()
        {
            // テキスト追加フラグが立っている時のみ、テキストを追加していく
            if (!_isAddText) return;

            // テキストスピードの調整、指定フレーム待機
            if (_waitFrameCount < sm.WAIT_TIME_TEXT)
            {
                _waitFrameCount += 1;
                return;
            }
            else
            {
                _waitFrameCount = 1;
            }

            // テキスト更新中にクリックがあった場合は全文を表示する
            if (sb.isStopAddingTextAndDisplayAllText)
            {
                sb.isStopAddingTextAndDisplayAllText = false;
                textWindowMain.text = TextAllOfAddingText;

                // テキスト追加を終了する
                _isAddText = false;
                return;
            }

            // テキスト追加を終了する
            if (_textIdx >= _addText.Length)
            {
                _isAddText = false;
                return;
            }

            textWindowMain.text += _addText[_textIdx].ToString();
            _textIdx += 1;
        }

        public async UniTask UpdateText(string text)
        {
            sb.isAddingText = true;

            // 改行位置の調整
            text = GetAdjustedNewLine(text);

            // バックログへの追加
            AddTextBackLog(text, lineNum);

            // スキップ中はそのまま表示のみする
            if (sb.isSkipping)
            {
                textWindowMain.text = text;
                sb.isAddingText = false;
                return;
            }

            sb.isWaitClick = false;

            // クリックで全文表示のために保存しておく
            TextAllOfAddingText = text;
            // 既に入っているテキストのリセット
            textWindowMain.text = "";

            // FixedUpdate関数内で、テキスト追加を開始する
            _textIdx = 0;
            _addText = text;
            _isAddText = true;

            // FixedUpdate関数内で、テキスト追加が終了するのを待機する
            await UniTask.WaitWhile(() => _isAddText);

            sb.isWaitClick = true;
            sb.isAddingText = false;

            return;
        }

        // 改行位置を整える
        private string GetAdjustedNewLine(string text)
        {
            // セリフかどうか。
            bool isQuote = (text[0] == '「' || text[0] == '（');
            
            // 1行の文字数が指定数を上回るとき自動的に改行する
            int countOfNoneNewLine = 0; // 改行記号でない文字が連続した値
            int nowLine = 1; // 何行目か
            List<char> newText = new List<char>();

            for (int idx = 0; idx < text.Length; idx++)
            {
                newText.Add(text[idx]);
                countOfNoneNewLine++;
                // 先頭が「や（のとき、2行目以降では改行文字数を変え、先頭に空白を挿入する
                if (isQuote)
                {
                    if (text[idx] == '\n')
                    {
                        newText.Add('　');
                        countOfNoneNewLine = 0;
                        nowLine++;
                    }
                    if (nowLine == 1 && countOfNoneNewLine >= Settings.MAX_CHAR_NUM_PER_LINE
                        || nowLine > 1 && countOfNoneNewLine >= Settings.MAX_CHAR_NUM_PER_LINE - 1)
                    {
                        newText.Add('\n');
                        newText.Add('　');
                        countOfNoneNewLine = 0;
                        nowLine++;
                    }
                }
                // 地の文の時
                else
                {
                    if (text[idx] == '\n')
                    {
                        countOfNoneNewLine = 0;
                        nowLine++;
                    }
                    if (countOfNoneNewLine >= Settings.MAX_CHAR_NUM_PER_LINE)
                    {
                        newText.Add('\n');
                        countOfNoneNewLine = 0;
                        nowLine++;
                    }
                }
            }
            // 何行用いたかを更新する（ログの表示に用いる）
            lineNum = nowLine;
            // 連結した文字列を返す
            return string.Join("", newText);
        }

        public async UniTask UpdateTalker(string name)
        {
            textWindowTalker.text = name;
            TalkerName = name;
            return;
        }

        // ログへの追加
        private void AddTextBackLog(string text, int lineNum)
        {
            // 名前があるときは【】を付ける
            string talkerText = (TalkerName == "") ? ("\n") : ("【" + TalkerName + "】\n");
            // 名前欄は本文の改行数に合わせる
            for(int i=0; i<lineNum; i++) talkerText += '\n';
            backlogTextTalker.text += talkerText;

            string mainText = (text + "\n\n");
            backlogTextMain.text += mainText;

            // Listへの追加
            backlogTextTalkerList.Add(talkerText);
            backlogTextMainList.Add(mainText);
            
            // 指定した数を超えた場合にログを一部消去する
            if (backlogTextMainList.Count > Settings.BACKLOG_MAX_HOLD_NUM)
            {
                // 削除する数
                int deletedLines = 10;

                // Listの最初からN行削除する
                backlogTextTalkerList.RemoveRange(0, deletedLines);
                backlogTextMainList.RemoveRange(0, deletedLines);

                // 文字列を全て結合して再代入する
                string talkertext = "";
                string maintext = "";
                foreach (var line in backlogTextTalkerList) { talkertext += line; }
                foreach (var line in backlogTextMainList) { maintext += line; }
                backlogTextTalker.text = talkertext;
                backlogTextMain.text = maintext;
            }
        }
    }
}
