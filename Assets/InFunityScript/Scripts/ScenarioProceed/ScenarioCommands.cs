using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

namespace FThingSoftware.InFunityScript
{
    public class ScenarioCommands : MonoBehaviour
    {
        private ScenarioManager sm;
        private ScenarioBooleans sb;

        // テキストウィンドウへの表示とバックログへの追加
        public ScenarioCommandText scText { get; private set; }
        // 選択肢ボタンの表示を行う
        public ScenarioCommandSelectButton scSelectButton {get; private set;}
        // キャラクター画像の表示や移動を行う
        public ScenarioCommandCharacter scChara { get; private set; }
        
        private void Awake()
        {
            sm = GetComponent<ScenarioManager>();
            sb = GetComponent<ScenarioBooleans>();
            scText = GetComponent<ScenarioCommandText>();
            scSelectButton = GetComponent<ScenarioCommandSelectButton>();
            scChara = GetComponent<ScenarioCommandCharacter>();
        }

        // Text
        public async UniTask UpdateText(string text)
        {
            await scText.UpdateText(text);
        }
        public async UniTask UpdateTalker(string name)
        {
            await scText.UpdateTalker(name);
        }

        // SelectButton
        public async UniTask SelectButtonShow(string text, string scenarioName, string labelName)
        {
            await scSelectButton.SelectButtonShow(text, scenarioName, labelName);
        }

        public async UniTask SelectButtonWaitClick()
        {
            await scSelectButton.SelectButtonWaitClick();
        }
        
        // Character
        public async UniTask CharaShow(string charaName, string[] facetype, float time, float posx = 0, float posy = 0, bool reverse = false)
        {
            await scChara.CharaShow(charaName, facetype, time, posx, posy, reverse);
        }

        public async UniTask CharaFace(string charaName, string[] facetype, float time)
        {
            await scChara.CharaFace(charaName, facetype, time);
        }

        public async UniTask CharaHide(string charaName, float time)
        {
            await scChara.CharaHide(charaName, time);
        }

        public async UniTask CharaHideAll(float time)
        {
            await scChara.CharaHideAll(time);
        }

        public async UniTask CharaReverse(string charaName, bool reverse, float time, bool clockwiseRotation)
        {
            await scChara.CharaReverse(charaName, reverse, time, clockwiseRotation);
        }

        public async UniTask CharaMove(string charaName, float time, float posx, float posy, Easing.Ease easing, bool absolute)
        {
            await scChara.CharaMove(charaName, time, posx, posy, easing, absolute);
        }
    }
}