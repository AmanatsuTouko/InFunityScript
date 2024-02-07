using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

            // Mainシーンにおいてのみ、ScenarioManagerの参照を取得する
            if(SceneManager.GetActiveScene().name == Settings.SCENE_MAIN)
            {
                sb = GameObject.Find("ScenarioManager").GetComponent<ScenarioBooleans>();
            }
        }

        private bool isMainScene()
        {
            return SceneManager.GetActiveScene().name == Settings.SCENE_MAIN;
        }

        // マウスホバー時を検出して、クリック時にテキストが進まないようにする
        // マウスホバー時と、ホバーがはずれた時に画像を入れ替える
        public void UserMouseOver()
        {
            if(isMainScene())
            {
                sb.isMouseOverOnSystemButtons = true;
            }
            image.sprite = spriteHover;
        }
        public void UserMouseExit()
        {
            if(isMainScene())
            {
                sb.isMouseOverOnSystemButtons = false;
            }
            image.sprite = spriteNormal;
        }

        // normal,hover時の画像を更新する(SaveLoadPanel表示時にSaveLoadを切り替える際に用いる)
        public void setNormalHoverSprite(Sprite normal, Sprite hover)
        {
            spriteNormal = normal;
            spriteHover = hover;
        }
    }
}