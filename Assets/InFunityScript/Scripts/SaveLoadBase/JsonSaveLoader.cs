using UnityEngine;
using System.IO;
// https://kiironomidori.hatenablog.com/entry/unity_save_json

namespace FThingSoftware.InFunityScript
{
    public static class JsonSaveLoader<T>
    {
        static string SavePath(string fileName) => $"{Application.persistentDataPath}/{fileName}.json";

        public static void Save(T data, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(SavePath(fileName), false))
            {
                string jsonstr = JsonUtility.ToJson(data, true);
                sw.Write(jsonstr);
                sw.Flush();
            }
        }

        public static T Load(string fileName)
        {
            if (File.Exists(SavePath(fileName)))
            {
                using (StreamReader sr = new StreamReader(SavePath(fileName)))
                {
                    string datastr = sr.ReadToEnd();
                    return JsonUtility.FromJson<T>(datastr);
                }
            }
            //存在しない場合はdefaultを返却
            return default;
        }
    }
}
