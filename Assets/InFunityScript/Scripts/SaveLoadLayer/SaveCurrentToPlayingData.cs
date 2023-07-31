using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FThingSoftware.InFunityScript
{
    public class SaveCurrentToPlayingData : MonoBehaviour
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

        // 現在のゲームの状態をrebuildDataなどの0番目に保存する
        public void SaveCurrent(int slotNum)
        {
            var rebuildData = SaveDataHolder.I.GetPlayingRebuildData();

            // シナリオ名の取得と進行ページ数の保存
            rebuildData.ScenarioName = scenarioManager.GetScenarioName();
            rebuildData.ScenarioPage = scenarioManager.GetScenarioPage();

            // スロット名の取得
            string slotName = SaveDataHolder.I.GetSlotNumTextFromSlotNum(slotNum);

            // 背景画像の保存と画像名の保存
            StartCoroutine(ScreenShot.Capture(slotName));
            rebuildData.ScreenShotFile = $"{slotName}.png";
            
            // MainTextの保存 改行と文頭の全角スペースを削除する
            string mainText = scenarioCommandText.TextAllOfAddingText;
            mainText = mainText.Replace("\n", "");
            mainText = mainText.Replace("　", "");
            rebuildData.TextMain = mainText;

            // Nameの保存
            rebuildData.TextTalker = scenarioCommandText.TalkerName;

            // 日付の保存 現在日時を取得し、yyyymmdd形式の文字列に変換する
            rebuildData.Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            // Chapter名の保存

            // 背景画像の名前の保存

            // BGMの保存

            // 表示しているキャラクターの保存

            // 表示している画像の保存

            // カメラの状態の保存
        }
    }
}
