using System;
using System.Collections.Generic;

namespace FThingSoftware.InFunityScript
{
    [Serializable]
    public class ScenarioCallStacks
    {
        public List<ScenarioCallStack> Each = new List<ScenarioCallStack>() { };
    }

    [Serializable]
    public class ScenarioCallStack
    {
        public int Num = 0;
        public List<ScenarioData> scenarioDatas = new List<ScenarioData>() { };

        public ScenarioCallStack DeepCopy()
        {
            ScenarioCallStack sc = new ScenarioCallStack();
            foreach (ScenarioData data in scenarioDatas) 
            {
                sc.scenarioDatas.Add(data.DeepCopy());
            }
            return sc;
        }
    }

    [Serializable]
    public class ScenarioData
    {
        public string Name = "";
        public int Page = 0;

        public ScenarioData(string name, int page) 
        {
            Name = name;
            Page = page;
        }

        public ScenarioData DeepCopy()
        {
            ScenarioData sd = new ScenarioData("", 0);
            sd.Name = this.Name;
            sd.Page = this.Page;
            return sd;
        }
    }
}

