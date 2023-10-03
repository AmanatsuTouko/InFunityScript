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
        // キャラクター画像の表示や移動を行う
        public ScenarioCommandCharacter scChara { get; private set; }


        private void Awake()
        {
            sm = GetComponent<ScenarioManager>();
            sb = GetComponent<ScenarioBooleans>();
            scText = GetComponent<ScenarioCommandText>();
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

        // Character
        public async UniTask CharaShow(string charaName, string[] facetype, float time, float posx = 0, float posy = 0, bool reverse = false)
        {
            await scChara.CharaShow(charaName, facetype, time, posx, posy, reverse);
        }
    }
}