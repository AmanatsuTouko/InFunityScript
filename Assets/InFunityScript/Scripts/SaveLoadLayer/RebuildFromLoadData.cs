using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        // SelectButton復元用
        private ScenarioCommandSelectButton _scenarioCommandSelectButton;

        // GameObjectの取得
        public void Init()
        {
            GameObject scenarioManagerObject = GameObject.Find("ScenarioManager");
            if(scenarioManagerObject != null){
                _scenarioManager = scenarioManagerObject.GetComponent<ScenarioManager>();
                _scenarioCommandText = scenarioManagerObject.GetComponent<ScenarioCommandText>();
                _scenarioCommandCharacter = scenarioManagerObject.GetComponent<ScenarioCommandCharacter>();
                _scenarioCommandSelectButton = scenarioManagerObject.GetComponent<ScenarioCommandSelectButton>();
            }
        }

        // 直前にロードしたデータからキャラクターや背景などの画面情報を復元する
        public async void Rebuild()
        {
            Debug.Log("rebuild");
            await RebuildAsync();
        }

        public async UniTask RebuildAsync(){
            // 画面のリセット
            // キャラクターの全削除
            await _scenarioCommandCharacter.CharaHideAll(0);
            // 画像の全削除
            
            // 選択肢の全削除
            await _scenarioCommandSelectButton.DeleteSelectButtonAll();


            // 削除情報 同期の為、1フレーム待機する
            await UniTask.Yield();

            // 復元データの読み出し
            RebuildData rebuildData = SaveDataHolder.I.GetPlayingRebuildData();

            // テキストの復元
            await _scenarioCommandText.RebuildTextAndTalker(rebuildData.TextTalker, rebuildData.TextMain);

            // キャラクターの復元
            foreach(var chara in rebuildData.DisplayCharas)
            {
                string name = chara.Name;
                string[] face = chara.Images;
                Vector3 pos = chara.Pos;
                bool reverse = chara.Reverse;
                // シナリオ命令を実行して、復元する
                await _scenarioCommandCharacter.CharaShow(name, face, 0, pos.x, pos.y, reverse);
            }

            // 背景の復元


            // 画像の復元


            // BGMの復元


            // カメラの復元


            // 選択肢の復元


        }
    }

}
