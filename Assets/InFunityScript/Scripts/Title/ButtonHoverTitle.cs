using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class ButtonHoverTitle : MonoBehaviour
    {
        // ImageComponent
        private Image image;
        // マウスホバー前のsprite
        [SerializeField] private Sprite spriteNormal;
        // マウスホバー時のsprite
        [SerializeField] private Sprite spriteHover;

        // Imageコンポーネントの取得
        private void Awake()
        {
            image = GetComponent<Image>();
        }

        // マウスホバー時を検出して、クリック時にテキストが進まないようにする
        // マウスホバー時と、ホバーがはずれた時に画像を入れ替える
        public void UserMouseOver()
        {
            image.sprite = spriteHover;
        }
        public void UserMouseExit()
        {
            image.sprite = spriteNormal;
        }
    }
}