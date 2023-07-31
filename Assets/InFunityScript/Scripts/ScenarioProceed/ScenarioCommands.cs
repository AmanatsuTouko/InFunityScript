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

        private void Awake()
        {
            sm = GetComponent<ScenarioManager>();
            sb = GetComponent<ScenarioBooleans>();
            scText = GetComponent<ScenarioCommandText>();
        }

        public async UniTask UpdateText(string text)
        {
            await scText.UpdateText(text);
        }
        public async UniTask UpdateTalker(string name)
        {
            await scText.UpdateTalker(name);
        }

    }
}