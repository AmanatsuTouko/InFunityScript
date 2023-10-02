using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class SaveLoadLayer : MonoBehaviour
    {
        [SerializeField] public MODE mode { get; set; }
        public enum MODE
        {
            SAVE,
            LOAD
        }
        // SaveSlot
        [SerializeField] private List<SaveSlot> saveSlots;

        // ページ番号
        private int pageNum = 1;
        // ページ番号の最大値(1～N)
        private int pageMax = Settings.SAVE_DATA_NUM / Settings.SAVE_SLOT_NUM_OF_PAGE;
        // ページテキスト
        [SerializeField] private TextMeshProUGUI pageText;

        // タイトル画像
        [SerializeField] Image titleImage;
        [SerializeField] Sprite titleSaveImage;
        [SerializeField] Sprite titleLoadImage;
        // 背景画像
        [SerializeField] Image bgImage;
        [SerializeField] Sprite bgSaveImage;
        [SerializeField] Sprite bgLoadImage;

        // 下部のSaveLoad切り替えボタン
        [SerializeField] Image switchSaveLoadButtonImage;
        [SerializeField] ButtonHover switchSaveLoadButtonButtonHover;
        [SerializeField] Sprite saveImageNormal;
        [SerializeField] Sprite saveImageHover;
        [SerializeField] Sprite loadImageNormal;
        [SerializeField] Sprite loadImageHover;

        // ========================
        // ページ切り替えと更新処理
        // ========================
        // ページ切り替えボタンからクリックされた時に呼び出す
        // ページ番号の表示更新と、全てのスロットの詳細情報の更新を行う
        public void NextPage()
        {
            pageNum += 1;
            if (pageNum > pageMax) pageNum = 1;
            UpdatePageText();
            UpdateSaveSlotsDetailAll();
        }
        public void BackPage()
        {
            pageNum -= 1;
            if (pageNum < 1) pageNum = pageMax;
            UpdatePageText();
            UpdateSaveSlotsDetailAll();
        }
        // ページ番号の表示を更新する
        public void UpdatePageText()
        {
            pageText.text = $"{pageNum.ToString("D2")}/{pageMax.ToString("D2")}";
        }

        // SAVE,LOADのモードに応じて左上のSAVE/LOAD表記と背景画像を更新する
        // 下部のSAVE/LOAD切り替えボタンの画像を更新する
        public void UpdateMaterialRefSaveLoadMode()
        {
            if(mode == MODE.SAVE)
            {
                titleImage.sprite = titleSaveImage;
                bgImage.sprite = bgSaveImage;
                // 下部のSAVE/LOAD切り替えボタンの画像の変更
                switchSaveLoadButtonImage.sprite = loadImageNormal;
                switchSaveLoadButtonButtonHover.setNormalHoverSprite(loadImageNormal, loadImageHover);
            }
            else if(mode == MODE.LOAD)
            {
                titleImage.sprite = titleLoadImage;
                bgImage.sprite = bgLoadImage;
                // 下部のSAVE/LOAD切り替えボタンの画像の変更
                switchSaveLoadButtonImage.sprite = saveImageNormal;
                switchSaveLoadButtonButtonHover.setNormalHoverSprite(saveImageNormal, saveImageHover);
            }
        }

        // ページ番号と画面上のクリックされた位置からスロット番号を返す関数
        public int getSlotNum(int slotnum)
        {
            return (pageNum-1)*Settings.SAVE_SLOT_NUM_OF_PAGE + slotnum;
        }

        // 全てのセーブスロットの情報を表示する
        public void UpdateSaveSlotsDetailAll()
        {
            foreach(var saveslot in saveSlots)
            {
                saveslot.UpdateSlotFromSaveDataNum();
            }
        }

        // キー入力でページを切り替える
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) BackPage();
            else if (Input.GetKeyDown(KeyCode.RightArrow)) NextPage();
        }
    }
}
