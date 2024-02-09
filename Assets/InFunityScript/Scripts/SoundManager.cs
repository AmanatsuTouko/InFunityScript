using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        private void Awake()
        {
            // シングルトンかつ、シーン遷移しても破棄されないようにする
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);         
            } else  { 
                Destroy(gameObject);
            }
        }

        // テスト
        private void Start()
        {
            // PlaySE(se.opendoor1, 100, false, 0, 0, Easing.Ease.None, 0);
        }

        // メモ
        // オブジェクトプールにして、最適化を行いたい

        public async UniTask PlaySE(string seName, int volume, bool waitComplete, float startSec, float endSec, Easing.Ease fadeEasing, float fadeSec)
        {
            // seのファイルを探して、なければエラー出力
            AudioClip audioClip = Resources.Load<AudioClip>(Settings.RESOURCES_PATH_SE + seName);
            if (audioClip == null)
            {
                Debug.LogError($"Play SE Error : Audio file {seName} is not exist in Resources/{Settings.RESOURCES_PATH_SE}.");
                return;
            }

            // Prefabを用いてインスタンスの作成
            // GameObject audioObject = Instantiate(_characterPrefab);

            // オブジェクトの作成
            GameObject audioObject = new GameObject();
            audioObject.transform.SetParent(this.transform, false);
            audioObject.name = seName;

            // AudioSourceを付ける
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;

            // 音を鳴らす
            audioSource.Play();

            // 鳴り終わったタイミングでオブジェクトを削除する

        }
    }
}