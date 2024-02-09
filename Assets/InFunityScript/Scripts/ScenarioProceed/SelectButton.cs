using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SelectButton : MonoBehaviour
    {
        // 選択肢に表示する文面
        public string Text = "";
        // ジャンプ先のシナリオファイル名
        public string ScenarioName = "";
        // ジャンプ先のラベル名
        public string LabelName = "";

        [SerializeField] TextMeshProUGUI _textUI;

        public void Init(string text, string scenarioName, string labelName)
        {
            _textUI.text = text;
            this.Text = text;
            this.ScenarioName = scenarioName;
            this.LabelName = labelName;
        }
    }
}