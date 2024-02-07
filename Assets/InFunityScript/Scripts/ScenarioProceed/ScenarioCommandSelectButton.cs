using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

namespace FThingSoftware.InFunityScript
{
    public class ScenarioCommandSelectButton : MonoBehaviour
    {
        [SerializeField] GameObject _selectButtonPrefab;
        [SerializeField] GameObject _selectButtonLayer;

        // 選択肢ボタンのクリック後にジャンプする必要があるため
        private ScenarioManager _scenarioManager;

        private void Awake()
        {
            _scenarioManager = GetComponent<ScenarioManager>();
        }

        public async UniTask SelectButtonShow(string text, string scenarioName, string labelName)
        {
            // instanceの生成
            GameObject selectButton = Instantiate(_selectButtonPrefab);
            // 親オブジェクトの設定
            selectButton.transform.SetParent(_selectButtonLayer.transform, false);
            // テキストの代入
            selectButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

            // クリック時に実行する関数の設定
            selectButton.GetComponent<Button>().onClick.AddListener( async () => 
            {
                // 選択肢ボタンを消去する
                await DeleteSelectButtonAll();
                // 指定したシナリオ名のラベル先にジャンプする
                // ラベルがない場合は、そのままジャンプ
                if(labelName == "")
                {
                    _scenarioManager.StartScenario(scenarioName, 0);
                }
                // ラベルの指定がある場合は、検索してジャンプ
                else
                {
                    await _scenarioManager.FindLabelAndJumpScenario(scenarioName, labelName);
                }
            });
        }

        // 選択肢ボタンの位置の設定値
        List<List<int>> _selectButtonPosY = new List<List<int>>()
        {
            new List<int>(){0},
            new List<int>(){75, -75},
            new List<int>(){150, 0, -150},
            new List<int>(){225, 75, -75, -225},
        };

        public async UniTask SelectButtonWaitClick()
        {
            // 位置の調整
            // 選択肢のオブジェクトを取得する
            List<Transform> buttonTransforms = new List<Transform>();
            // 選択肢の数を取得する
            int selectButtonCount = _selectButtonLayer.transform.childCount;
            for(int i=0; i<selectButtonCount; i++)
            {
                buttonTransforms.Add(_selectButtonLayer.transform.GetChild(i));
            }
            // 位置の調整
            for(int i=0; i<selectButtonCount; i++)
            {
                int yPos = _selectButtonPosY[selectButtonCount - 1][i];
                buttonTransforms[i].localPosition = new Vector3(0, yPos, 0);
            }

            // UniTaskをキャンセルする
            _scenarioManager.CancelScenarioProceed();
        }

        private async UniTask DeleteSelectButtonAll()
        {
            // 子オブジェクトを取得して全削除
            foreach(Transform transform in _selectButtonLayer.transform)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}