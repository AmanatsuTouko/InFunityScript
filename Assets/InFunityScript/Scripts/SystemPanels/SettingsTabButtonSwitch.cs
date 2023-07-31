using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class SettingsTabButtonSwitch : MonoBehaviour
    {
        // Imageコンポーネント
        private Image image;
        // 選択時の画像
        [SerializeField] private Sprite spriteSelected;
        // 非選択時の画像
        [SerializeField] private Sprite spriteNotSelected;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        // 選択と非選択を切り替える
        public void TabImageSetActive(bool active)
        {
            image.sprite = active ? spriteSelected : spriteNotSelected;
        }
    }
}