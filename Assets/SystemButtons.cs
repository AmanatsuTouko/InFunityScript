﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SystemButtons : MonoBehaviour
    {
        private KeyInputManager _keyInputManager;
        private void Awake()
        {
            _keyInputManager = GameObject.Find("ScenarioManager").GetComponent<KeyInputManager>();
        }

        public void SwichAutoMode()
        {
            _keyInputManager.SwitchAutoMode();
        }
        public void SwichSkipMode()
        {
            _keyInputManager.SwitchSkipMode();
        }

        public void ShowSaveLayer()
        {
            _keyInputManager.SwitchSavePanel();
        }
        public void ShowLoadeLayer()
        {
            _keyInputManager.SwitchLoadPanel();
        }

        public void ShowBacklog()
        {
            _keyInputManager.SwitchBacklog();
        }
        public void ShowSettingsLayer()
        {
            _keyInputManager.SwitchSettings();
        }
        public void HideTextWindow()
        {
            _keyInputManager.SwichTextWindow();
        }
    }
}
