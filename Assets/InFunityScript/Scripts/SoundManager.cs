using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FThingSoftware.InFunityScript
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField] GameObject _sePrefab;

        private void Awake()
        {
            // シングルトンかつ、シーン遷移しても破棄されないようにする
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                // オブジェクトプールとして再利用するための初期化
                InitObjectPool();
            } else  { 
                Destroy(gameObject);
            }
        }

        // 現在鳴らしているオブジェクトプールのインデックス
        public int _SEPoolIdx = 0;
        // AudioSourceの参照等を管理する
        private SEPool[] _sePools;

        private void InitObjectPool()
        {
            if(_sePrefab == null)
            {
                Debug.LogError("SoundManager : SE prefab is not found.");
                return;
            }

            _sePools = new SEPool[Settings.SE_MAX_NUM];
            for(int i=0; i<Settings.SE_MAX_NUM; i++)
            {
                var obj = Instantiate(_sePrefab);
                obj.transform.SetParent(this.transform);
                _sePools[i] = obj.GetComponent<SEPool>();
            }
        }

        // テスト
        private void Start()
        {
            PlaySE(se.opendoor1, 100, false, 0, 0, Easing.Ease.None, 0);
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

            // オブジェクトプールから使えるインデックスを取得する
            int usedCount = 0;
            while(true)
            {
                if(_sePools[_SEPoolIdx].AudioSource.isPlaying == false)
                {
                    break;
                }
                else
                {
                    // インデックスのループ
                    _SEPoolIdx += 1;
                    if(_SEPoolIdx >= Settings.SE_MAX_NUM)
                    {
                        _SEPoolIdx = 0;
                    }
                    // 全て使い切った場合にはエラー出力して終了
                    usedCount += 1;
                    if(usedCount >= 10) 
                    {
                        Debug.LogError("SoundManager : SE object pool is limit se count.\nPlease review Settings.SE_MAX_NUM.");
                        return;
                    }
                }
            }

            var audioSource = _sePools[_SEPoolIdx].AudioSource;
            
            // 音声ファイルの設定後、音を鳴らす
            audioSource.clip = audioClip;
            audioSource.Play();
            

            // // Prefabを用いてインスタンスの作成
            // // GameObject audioObject = Instantiate(_characterPrefab);

            // // オブジェクトの作成
            // GameObject audioObject = new GameObject();
            // audioObject.transform.SetParent(this.transform, false);
            // audioObject.name = seName;

            // // AudioSourceを付ける
            // AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            // audioSource.clip = audioClip;

            // // 音を鳴らす
            // audioSource.Play();s

            // 鳴り終わったタイミングでオブジェクトを削除する

        }
    }
}