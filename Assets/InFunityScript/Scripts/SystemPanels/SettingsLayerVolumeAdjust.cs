using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FThingSoftware.InFunityScript
{
    public class SettingsLayerVolumeAdjust : MonoBehaviour
    {
        // テキスト進行スライダー
        [SerializeField] private Slider textSpeedSlider;
        [SerializeField] private Slider waitTimeAutoSlider;
        [SerializeField] private Slider waitTimeSkipSlider;
        // 音量スライダー
        [SerializeField] private Slider volumeMasterSlider;
        [SerializeField] private Slider volumeBGMSlider;
        [SerializeField] private Slider volumeSESlider;
        // 設定値を表示するテキスト
        [SerializeField] private TextMeshProUGUI textSpeedText;
        [SerializeField] private TextMeshProUGUI waitTimeAutoText;
        [SerializeField] private TextMeshProUGUI waitTimeSkipText;
        [SerializeField] private TextMeshProUGUI volumeMasterText;
        [SerializeField] private TextMeshProUGUI volumeBGMText;
        [SerializeField] private TextMeshProUGUI volumeSEText;

        // ScenarioManager
        private ScenarioManager scenarioManager;

        // パラメータを更新したかどうか
        // 設定画面を閉じたり開いたりした際に、無駄な書き込み処理を行わないようにする
        private bool isUpdateParam = false;

        private void Awake()
        {
            scenarioManager = GameObject.Find("ScenarioManager").GetComponent<ScenarioManager>();
        }

        // 設定値を表示する。設定画面起動時に呼び出す
        public void SetSliderValueFromSaveDataHolder()
        {
            // テキスト進行に用いるパラメータの代入
            textSpeedSlider.value    = SaveDataHolder.I.GetTextSpeedToSettingLayerVolumeAdjust();
            waitTimeAutoSlider.value = SaveDataHolder.I.GetWaitTimeAutoToSettingLayerVolumeAdjust();
            waitTimeSkipSlider.value = SaveDataHolder.I.GetWaitTimeSkipToSettingLayerVolumeAdjust();
            // 音量パラメータの代入
            volumeMasterSlider.value = SaveDataHolder.I.GetVolumeMasterToSettingLayerVolumeAdjust();
            volumeBGMSlider.value    = SaveDataHolder.I.GetVolumeBGMToSettingLayerVolumeAdjust();
            volumeSESlider.value     = SaveDataHolder.I.GetVolumeSEToSettingLayerVolumeAdjust();
            // パラメータ更新のフラグをオフにする
            isUpdateParam = false;
        }

        // 設定画面の終了前に呼び出して設定を反映させる
        public void SaveToLocalStorageAndUpdateScenarioManagerParamOnExit()
        {
            // 一箇所も更新していない場合は何もしない
            if (!isUpdateParam) return;
            // 設定値をセーブデータに保存する
            SaveDataHolder.I.SaveUserSettingsToLocalStorage();
            // ScenarioManagerクラスの設定値をセーブデータから読み込んで更新する
            scenarioManager.UpdateWaitTimeFromSaveData();
        }

        // 各スライダーが更新された時に呼び出す
        // スライダーの値からセーブデータクラスを更新し、表示している数値を更新する
        public void UpdateTextSpeedFromSlider()
        {
            SaveDataHolder.I.SetTextSpeedFromSettingLayerVolumeAdjust((int)textSpeedSlider.value);
            // 実際は1~20の値だが、他パラメータとの兼ね合いで1~100の値として表示する
            textSpeedText.text = (textSpeedSlider.value * 5).ToString();
            isUpdateParam = true;
        }
        public void UpdateWaitTimeAutoFromSlider()
        {
            SaveDataHolder.I.SetWaitTimeAutoFromSettingLayerVolumeAdjust((int)waitTimeAutoSlider.value);
            waitTimeAutoText.text = waitTimeAutoSlider.value.ToString();
            isUpdateParam = true;
        }
        public void UpdateWaitTimeSkipFromSlider()
        {
            SaveDataHolder.I.SetWaitTimeSkipFromSettingLayerVolumeAdjust((int)waitTimeSkipSlider.value);
            waitTimeSkipText.text = waitTimeSkipSlider.value.ToString();
            isUpdateParam = true;
        }
        public void UpdateVolumeMasterFromSlider()
        {
            SaveDataHolder.I.SetVolumeMasterFromSettingLayerVolumeAdjust((int)volumeMasterSlider.value);
            volumeMasterText.text = volumeMasterSlider.value.ToString();
            isUpdateParam = true;
        }
        public void UpdateVolumeBGMFromSlider()
        {
            SaveDataHolder.I.SetVolumeBGMFromSettingLayerVolumeAdjust((int)volumeBGMSlider.value);
            volumeBGMText.text = volumeBGMSlider.value.ToString();
            isUpdateParam = true;
        }
        public void UpdateVolumeSEFromSlider()
        {
            SaveDataHolder.I.SetVolumeSEFromSettingLayerVolumeAdjust((int)volumeSESlider.value);
            volumeSEText.text = volumeSESlider.value.ToString();
            isUpdateParam = true;
        }
    }
}

