using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class ScenarioCommandAudio : MonoBehaviour
    {
        public async UniTask AudioPlaySE(string seName, int volume, bool waitComplete, float startSec, float endSec, Easing.Ease fadeEasing, float fadeSec)
        {
            // SoundManagerから共通の関数を呼び出す
            await SoundManager.Instance.PlaySE(seName, volume, waitComplete, startSec, endSec, fadeEasing, fadeSec);
        }
    }
}