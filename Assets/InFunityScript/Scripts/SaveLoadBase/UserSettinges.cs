using System;
using System.Collections.Generic;

// テキストスピードなどゲーム全体で保存する必要のあるユーザデータ
namespace FThingSoftware.InFunityScript
{
    [Serializable]
    public class UserSettings
    {
        public int TextSpeed = 17;     //1～ 20の値を取る
        public int SkipWaitTime = 50;  //1～100の値を取る
        public int AutoWaitTime = 50;  //1～100の値を取る
        public int VolumeMaster = 100; //0～100の値を取る(0:消音)
        public int VolumeBGM = 100;    //0～100の値を取る(0:消音)
        public int VolumeSE = 100;     //0～100の値を取る(0:消音)
    }
}