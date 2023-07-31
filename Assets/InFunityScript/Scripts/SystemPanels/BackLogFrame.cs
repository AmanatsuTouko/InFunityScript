using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class BackLogFrame : MonoBehaviour
    {
        [SerializeField] RectTransform ContentRectTransform;
        [SerializeField] RectTransform LogTextRectTransform;
        [SerializeField] Scrollbar Scrollbar;

        void Start()
        {
            // デバッグ用
            // StartCoroutine(enumerator());
        }

        IEnumerator enumerator()
        {
            yield return new WaitForSeconds(0.5f);
            UpdateHeight();
        }

        // Textの行数に応じて高さを更新する
        public void UpdateHeight()
        {
            // ContentのHeightをMainTextと等しくする
            Vector2 size = this.gameObject.GetComponent<RectTransform>().sizeDelta;
            ContentRectTransform.sizeDelta = new Vector2(size.x, LogTextRectTransform.sizeDelta.y);

            // 最下にスクロール済みにする
            float height = ContentRectTransform.sizeDelta.y;
            ContentRectTransform.anchoredPosition = new Vector2(0, height);
            Scrollbar.value = 0.01f;
        }

        // バックログを表示する
        public void Display()
        {
            // Textの行数に応じて高さを更新する
            UpdateHeight();
            // BackLogLayerの位置を画面外から正面に移動させる
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        // バックログを非表示にする
        public void Hide()
        {
            // BackLogLayerの位置を正面から画面外に移動させる
            this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1080);
        }
    }
}