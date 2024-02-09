using System.Collections;
using System.Collections.Generic;
using FThingSoftware.InFunityScript;
using UnityEngine;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField] GameObject _loadLayer;

        [Header("BottomButtonsOnLoadLayer")]
        [SerializeField] GameObject _bottomButtonsParent;
        [SerializeField] GameObject _backButton;

        void Awake()
        {
            HideButtonsExceptBackButton();
            _backButton.GetComponent<Button>().onClick.AddListener( () => OnClickBackButton());
        }

        // BackButtonのみを表示する
        private void HideButtonsExceptBackButton()
        {
            foreach(Transform obj in _bottomButtonsParent.transform)
            {
                obj.gameObject.SetActive(false);
            }
            _backButton.SetActive(true);
        }

        // BackButtonをクリックしたときの挙動を設定する
        private void OnClickBackButton()
        {
            // レイヤーを非表示にする
            _loadLayer.SetActive(false);
        }
    }
}