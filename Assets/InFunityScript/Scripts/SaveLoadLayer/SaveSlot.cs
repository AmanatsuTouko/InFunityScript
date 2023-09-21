using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace FThingSoftware.InFunityScript
{
    public class SaveSlot : MonoBehaviour
    {
        [SerializeField] private Image thumnailImage;
        [SerializeField] private TextMeshProUGUI slotNumText;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI dateText;

        // SAVE LOAD の取得などに用いる
        [SerializeField] private SaveLoadLayer sll;
        // ホバー時に詳細を表示する
        [SerializeField] private PropertyImage propertyImage;

        // 何番目のスロットか、UnityEditor側で設定する
        public int slotNum = 0;

        // スロットの情報を更新する
        public void UpdateSlotFromSaveDataNum()
        {
            if (slotNum == 0) return;
            this.thumnailImage.sprite = SaveDataHolder.I.GetSpriteFromSlotNum(sll.getSlotNum(slotNum));
            this.slotNumText.text = SaveDataHolder.I.GetSlotNumTextFromSlotNum(sll.getSlotNum(slotNum));
            this.mainText.text = SaveDataHolder.I.GetMainTextForOneSlotFromSlotNum(sll.getSlotNum(slotNum));
            this.dateText.text = SaveDataHolder.I.GetDateFromSlotNum(sll.getSlotNum(slotNum));
        }

        // クリックされた時に呼び出す
        // セーブもしくはロード処理を行う
        public void OnClickSlotSaveOrLoad()
        {
            // Titleシーンの時
            if(SceneManager.GetActiveScene().name == "Title")
            {
                // ロードすべきslotnumを保存して、Mainシーンに遷移する。
                SaveDataHolder.I.SaveSlotNumOnLoadTitle(sll.getSlotNum(slotNum));
                return;
            }

            if (sll.mode == SaveLoadLayer.MODE.SAVE)
            {
                // セーブする
                SaveDataHolder.I.SaveCurrentToSlotNum(sll.getSlotNum(slotNum));
                // スロットの詳細を更新する
                UpdateSlotFromSaveDataNum();
                // 詳細画面を更新する
                OnMouseHover();
            }
            else if(sll.mode == SaveLoadLayer.MODE.LOAD)
            {
                // ロードする
                SaveDataHolder.I.LoadAndStartScenarioFromSlotNum(sll.getSlotNum(slotNum));
            }
        }

        // ホバーされた時に詳細画面を更新する
        public void OnMouseHover()
        {
            propertyImage.UpdateImageAndTextsFromSlotNum(sll.getSlotNum(slotNum));
        }
    }
}
