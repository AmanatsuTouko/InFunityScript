using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SEPool : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

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