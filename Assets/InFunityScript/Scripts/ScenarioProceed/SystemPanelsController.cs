using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SystemPanelsController : MonoBehaviour
    {
        // テキストウィンドウレイヤー
        [SerializeField] private GameObject textWindowLayer;
        // バックログ
        [SerializeField] private BackLogFrame BacklogFrame;
        // 設定レイヤー
        [SerializeField] private GameObject settingsLayer;
        // SaveLoadレイヤー
        [SerializeField] public GameObject saveloadLayer;

        // テキストウィンドウの表示と非表示の切り替え
        public void TextWindowLayerSetActive(bool active)
        {
            textWindowLayer.SetActive(active);
        }
        // バックログの表示と非表示の切り替え
        // バックログに関しては常に更新するためSetActiveは用いずGameObjectは常に有効にしておいて、位置のみ変更する
        public void BacklogLayerSetActive(bool active)
        {
            if (active)
            {
                BacklogFrame.Display();
            }
            else
            {
                BacklogFrame.Hide();
            }
        }
        // 設定レイヤーの表示と非表示の切り替え
        public void SettingsLayerSetActive(bool active)
        {
            settingsLayer.SetActive(active);
            if (active)
            {
                // セーブデータから値の表示を行う
                settingsLayer.GetComponent<SettingsLayerVolumeAdjust>()?.SetSliderValueFromSaveDataHolder();
            }
            else
            {
                // ローカルストレージにデータをjsonファイルで保存し、ScenarioManagerクラスに設定したパラメータを反映させる
                settingsLayer.GetComponent<SettingsLayerVolumeAdjust>()?.SaveToLocalStorageAndUpdateScenarioManagerParamOnExit();
            }
        }
        // Saveレイヤーの表示と非表示の切り替え
        public void SaveLayerSetActive(bool active)
        {
            saveloadLayer.SetActive(active);
            if (active)
            {
                // セーブモードにして各スロットの詳細を表示する
                saveloadLayer.GetComponent<SaveLoadLayer>().mode = SaveLoadLayer.MODE.SAVE;
                saveloadLayer.GetComponent<SaveLoadLayer>().UpdateSaveSlotsDetailAll();
                saveloadLayer.GetComponent<SaveLoadLayer>().UpdatePageText();
                saveloadLayer.GetComponent<SaveLoadLayer>().UpdateMaterialRefSaveLoadMode();
            }
        }
        // Loadレイヤーの表示と非表示の切り替え
        public void LoadLayerSetActive(bool active)
        {
            saveloadLayer.SetActive(active);
            if (active)
            {
                // ロードモードにして各スロットの詳細を表示する
                saveloadLayer.GetComponent<SaveLoadLayer>().mode = SaveLoadLayer.MODE.LOAD;
                saveloadLayer.GetComponent<SaveLoadLayer>().UpdateSaveSlotsDetailAll();
                saveloadLayer.GetComponent<SaveLoadLayer>().UpdatePageText();
                saveloadLayer.GetComponent<SaveLoadLayer>().UpdateMaterialRefSaveLoadMode();
            }
        }
    }
}

