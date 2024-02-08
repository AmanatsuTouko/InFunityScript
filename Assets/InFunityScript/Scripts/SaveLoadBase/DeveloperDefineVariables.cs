using System;
using System.Collections.Generic;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    [Serializable]
    public class DeveloperDefineVariables
    {
        public List<DeveloperDefineVariable> Each = new List<DeveloperDefineVariable>() { };
    }

    [Serializable]
    public class DeveloperDefineVariable
    {
        // フラグ変数などを定義する
        public int love_girl = 0;

        // ディープコピー用のメソッド
        // 変数を追加した場合には、以下にも追加していく
        public DeveloperDefineVariable DeepCopy()
        {
            DeveloperDefineVariable ddv = new DeveloperDefineVariable();

            // これ以降に ddv.変数名 = this.変数名 の形で記述していく
            ddv.love_girl = this.love_girl;

            return ddv;
        }
    }
}