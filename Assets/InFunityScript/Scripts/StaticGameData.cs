using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class StaticGameData : MonoBehaviour
    {
        // 新しくゲームを始めた時と、ロード時で読み込むシナリオを変えるようにする
        public static bool NewGameFromTitle = false;
    }
}