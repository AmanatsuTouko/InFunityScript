using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SettingsLayer : MonoBehaviour
    {
        // 基本設定とショートカットのパネル
        [SerializeField] GameObject BaseSettings;
        [SerializeField] GameObject ShortcutSettings;

        // 基本設定とショートカットボタン
        [SerializeField] SettingsTabButtonSwitch BaseTab;
        [SerializeField] SettingsTabButtonSwitch ShortcutTab;

        // 基本設定画面の表示と、他の非表示
        public void DisplayBaseSettings()
        {
            BaseSettings.SetActive(true);
            BaseTab.TabImageSetActive(true);
            ShortcutSettings.SetActive(false);
            ShortcutTab.TabImageSetActive(false);
        }

        // ショートカット画面の表示と、他の非表示
        public void DisplayShortcutSettings()
        {
            BaseSettings.SetActive(false);
            BaseTab.TabImageSetActive(false);
            ShortcutSettings.SetActive(true);
            ShortcutTab.TabImageSetActive(true);
        }
    }
}
