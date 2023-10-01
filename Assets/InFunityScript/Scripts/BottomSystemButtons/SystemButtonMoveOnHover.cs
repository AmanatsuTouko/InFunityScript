using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SystemButtonMoveOnHover : MonoBehaviour
    {
        public RectTransform SystemButtonsTransform;

        private ScenarioBooleans sb;

        // デフォルトはHOLD
        public bool isHold = true;

        // 上昇後のposY
        public int maxPosY = -490;
        // 下降後のposY
        public int minPosY = -565;

        public bool isHoverBotton = false;

        public bool isUp = false;
        public bool isUpStop = false;
        public bool isDown = false;
        public bool isDownStop = false;

        // 1秒あたりの移動量 * Time.deltaTime
        private int deltaMove = 400;

        private void Awake()
        {
            sb = GameObject.Find("ScenarioManager").GetComponent<ScenarioBooleans>();
        }

        public void OnHoverEnter()
        {
            isHoverBotton = true;
        }
        public void OnHoverExit()
        {
            isHoverBotton = false;
        }

        // SystemButtonsを上昇させる
        IEnumerator UpSystemButtons()
        {
            while (true)
            {
                yield return null;

                if (isUpStop) break;
                if (SystemButtonsTransform.anchoredPosition.y == maxPosY) break;
                else
                {
                    SystemButtonsTransform.anchoredPosition = new Vector3(0, SystemButtonsTransform.anchoredPosition.y + deltaMove * Time.deltaTime, 0);
                    // 目標位置を超えた場合には、移動を終了する
                    if (maxPosY <= SystemButtonsTransform.anchoredPosition.y)
                    {
                        SystemButtonsTransform.anchoredPosition = new Vector3(0, maxPosY, 0);
                        isUpStop = true;
                        break;
                    }
                }
            }
        }
        // SystemButtonsを下降させる
        IEnumerator DownSystemButtons()
        {
            while (true)
            {
                yield return null;

                if (isDownStop) break;
                if (SystemButtonsTransform.anchoredPosition.y == minPosY) break;
                else
                {
                    SystemButtonsTransform.anchoredPosition = new Vector3(0, SystemButtonsTransform.anchoredPosition.y - deltaMove * Time.deltaTime, 0);
                    // 目標位置を超えた場合には、移動を終了する
                    if (minPosY >= SystemButtonsTransform.anchoredPosition.y)
                    {
                        SystemButtonsTransform.anchoredPosition = new Vector3(0, minPosY, 0);
                        isDownStop = true;
                        break;
                    }
                }
            }
        }

        // ホールドの切り替え
        public void SwitchHold()
        {
            if (isHold)
            {
                isHold = false;
            }
            else
            {
                SystemButtonsTransform.anchoredPosition = new Vector3(0, maxPosY, 0);
                isHold = true;
            }
        }

        private void Update()
        {
            // ホールド時は何もしない
            if (isHold)
            {
                return;
            }

            // マウスホバー時 and システムボタンの上にマウスがある時
            // SystemButtonsを上昇させる
            if (isHoverBotton || sb.isMouseOverOnSystemButtons)
            {
                isDownStop = true;
                isDown = false;

                isUpStop = false;
                if (!isUp) StartCoroutine(UpSystemButtons());
                isUp = true;
            }
            // SystemButtonsを下降させる
            else
            {
                isUpStop = true;
                isUp = false;

                isDownStop = false;
                if (!isDown) StartCoroutine(DownSystemButtons());
                isDown = true;
            }
        }
    }

}