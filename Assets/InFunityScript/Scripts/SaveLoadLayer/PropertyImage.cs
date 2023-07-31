using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FThingSoftware.InFunityScript
{
    // SaveLoadLayerにてホバーしたSaveSlotの詳細を表示する
    public class PropertyImage : MonoBehaviour
    {
        [SerializeField] private Image thumnailImage;
        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private TextMeshProUGUI chapterText;
        [SerializeField] private TextMeshProUGUI mainText;

        // スロット番号からデータを読みだして更新する
        // SaveSlot.csから呼び出す
        public void UpdateImageAndTextsFromSlotNum(int slotNum)
        {
            // 0-84の値以外の時は何もしない
            if (slotNum == 0 || slotNum >= Settings.SAVE_DATA_NUM) return;
            this.thumnailImage.sprite = SaveDataHolder.I.GetSpriteFromSlotNum(slotNum);
            this.mainText.text = SaveDataHolder.I.GetMainTextForDetailFromSlotNum(slotNum);
            this.dateText.text = SaveDataHolder.I.GetDateFromSlotNum(slotNum);
            this.chapterText.text = SaveDataHolder.I.GetChapterFromSlotNum(slotNum);
        }
    }

}
