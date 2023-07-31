using System;
using System.Collections.Generic;

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

        public DeveloperDefineVariable DeepCopy()
        {
            DeveloperDefineVariable ddv = new DeveloperDefineVariable();

            ddv.love_girl = this.love_girl;

            return ddv;
        }
    }
}