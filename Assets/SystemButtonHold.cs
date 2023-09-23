using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FThingSoftware.InFunityScript
{
    public class SystemButtonHold : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _sprite_hold;
        [SerializeField] private Sprite _sprite_unhold;

        [SerializeField] private SystemButtonMoveOnHover _systemButtonMoveOnHover;

        private ScenarioBooleans sb;

        private void Awake()
        {
            sb = GameObject.Find("ScenarioManager").GetComponent<ScenarioBooleans>();
        }
        public void MouseOver()
        {
            sb.isMouseOverOnSystemButtons = true;
        }
        public void MouseExit()
        {
            sb.isMouseOverOnSystemButtons = false;
        }
        public void OnClickHold()
        {
            if (_systemButtonMoveOnHover.isHold)
            {
                _image.sprite = _sprite_unhold;
            }
            else
            {
                _image.sprite = _sprite_hold;
            }
            _systemButtonMoveOnHover.SwitchHold();
        }
    }
}