using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class RebuildFromLoadData : MonoBehaviour
    {
        // ScenarioNameとScenarioPageの取得用
        private ScenarioManager _scenarioManager;
        // mainTextとtalkerTextの取得用
        private ScenarioCommandText _scenarioCommandText;
        // Character復元用
        private ScenarioCommandCharacter _scenarioCommandCharacter;

        // GameObjectの取得
        public void Init()
        {
            GameObject scenarioManagerObject = GameObject.Find("ScenarioManager");
            if(scenarioManagerObject != null){
                _scenarioManager = scenarioManagerObject.GetComponent<ScenarioManager>();
                _scenarioCommandText = scenarioManagerObject.GetComponent<ScenarioCommandText>();
                _scenarioCommandCharacter = scenarioManagerObject.GetComponent<ScenarioCommandCharacter>();
            }
        }

        // 直前にロードしたデータからキャラクターや背景などの画面情報を復元する
        public void Rebuild()
        {
            Debug.Log("rebuild");

            // シナリオ命令を実行して、復元する
            // _scenarioCommandCharacter.CharaShow();
        }
    }

}
