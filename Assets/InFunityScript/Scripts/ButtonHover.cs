using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class ButtonHover : MonoBehaviour
    {
        // ImageComponent
        private Image image;
        // マウスホバー前のsprite
        [SerializeField] private Sprite spriteNormal;
        // マウスホバー時のsprite
        [SerializeField] private Sprite spriteHover;
        // システムボタンにホバー時、フラグ変数を変更するために用いる
        private ScenarioBooleans sb;

        private void Awake()
        {
            image = GetComponent<Image>();
            sb = GameObject.Find("ScenarioManager").GetComponent<ScenarioBooleans>();
        }

        // マウスホバー時を検出して、クリック時にテキストが進まないようにする
        // マウスホバー時と、ホバーがはずれた時に画像を入れ替える
        public void UserMouseOver()
        {
            sb.isMouseOverOnSystemButtons = true;
            image.sprite = spriteHover;
        }
        public void UserMouseExit()
        {
            sb.isMouseOverOnSystemButtons = false;
            image.sprite = spriteNormal;
        }
    }
}