using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SEPool : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

        // スロットを使用しているかどうか
        // AudioSource.IsPlayingの場合は、再生開始までtrueにならないため別途管理する
        public bool IsUsing = false;

        public AudioSource AudioSource {
            get
            {
                return _audioSource;
            } 
            private set
            {
                _audioSource = value;
            }
        }

    }
}