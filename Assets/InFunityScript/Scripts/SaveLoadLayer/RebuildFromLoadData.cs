using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class RebuildFromLoadData : MonoBehaviour
    {
        // ScenarioNameとScenarioPageの取得用
        private ScenarioManager scenarioManager;
        // mainTextとtalkerTextの取得用
        private ScenarioCommandText scenarioCommandText;

        // GameObjectの取得
        private void Awake()
        {
            GameObject scenarioManagerObject = GameObject.Find("ScenarioManager");
            scenarioManager = scenarioManagerObject.GetComponent<ScenarioManager>();
            scenarioCommandText = scenarioManagerObject.GetComponent<ScenarioCommandText>();
        }

        // 直前にロードしたデータからキャラクターや背景などの画面情報を復元する
        public void Rebuild()
        {
            Debug.Log("rebuild");
        }
    }

}
