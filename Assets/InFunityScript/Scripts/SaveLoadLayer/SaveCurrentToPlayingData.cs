using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FThingSoftware.InFunityScript
{
    public class SaveCurrentToPlayingData : MonoBehaviour
    {
        // ScenarioNameとScenarioPageの取得用
        private ScenarioManager _scenarioManager;
        // mainTextとtalkerTextの取得用
        private ScenarioCommandText _scenarioCommandText;
        // 表示しているキャラクターの保存
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

        // 現在のゲームの状態をrebuildDataなどの0番目に保存する
        public void SaveCurrent(int slotNum)
        {
            var rebuildData = SaveDataHolder.I.GetPlayingRebuildData();

            // シナリオ名の取得と進行ページ数の保存
            rebuildData.ScenarioName = _scenarioManager.GetScenarioName();
            rebuildData.ScenarioPage = _scenarioManager.GetScenarioPage();

            // スロット名の取得
            string slotName = SaveDataHolder.I.GetSlotNumTextFromSlotNum(slotNum);

            // スクリーンショットの撮影・保存、画像名の保存
            StartCoroutine(ScreenShot.Capture(slotName));
            rebuildData.ScreenShotFile = $"{slotName}.png";
            
            // MainTextの保存 改行と文頭の全角スペースを削除する
            string mainText = _scenarioCommandText.TextAllOfAddingText;
            mainText = mainText.Replace("\n", "");
            mainText = mainText.Replace("　", "");
            rebuildData.TextMain = mainText;

            // Nameの保存
            rebuildData.TextTalker = _scenarioCommandText.TalkerName;

            // 日付の保存 現在日時を取得し、yyyymmdd形式の文字列に変換する
            rebuildData.Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

            // Chapter名の保存

            // 背景画像の名前の保存
            // rebuildData.BackGroudImage = "";

            // BGMの保存

            // 表示しているキャラクターリストの初期化と保存
            rebuildData.DisplayCharas.Clear();
            rebuildData.DisplayCharas = GetDisplayCharas();

            // 表示している画像の保存

            // カメラの状態の保存
        }

        // キャラクターの検索と保存用のデータの作成
        private List<DisplayChara> GetDisplayCharas()
        {
            var displayCharas = new List<DisplayChara>();
            var charaLayerObj = _scenarioCommandCharacter.CharaLayer;

            // キャラクターが表示されていない場合は何もしない
            int childCount = charaLayerObj.transform.childCount;
            if(childCount == 0)
            {
                return displayCharas;
            }

            for(int i=0; i<childCount; i++)
            {
                GameObject charaObj = charaLayerObj.transform.GetChild(i).gameObject;

                // 検索して保存するためのデータに整形する
                var displayChara = new DisplayChara();
                displayChara.Name = charaObj.name;
                Character character = charaObj.GetComponent<Character>();
                displayChara.Images = character.Images;
                displayChara.Pos = new Vector3(character.Pos.x, character.Pos.y, 0);
                displayChara.Scale = character.Scale;
                displayChara.Reverse = Mathf.Abs(charaObj.transform.localRotation.y) == 180 ? true : false;

                displayCharas.Add(displayChara);
            }

            return displayCharas;
        }
    }
}
