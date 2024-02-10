using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
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

        // オブジェクトプールから、使うことのできるインデックスを取得する
        private int GetUseableSEPoolIndex()
        {
            int usedCount = 0;
            while(true)
            {
                if(_sePools[_SEPoolIdx].IsUsing == false)
                {
                    _sePools[_SEPoolIdx].IsUsing = true;
                    return _SEPoolIdx;
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
                        return _SEPoolIdx;
                    }
                }
            }
        }

        // テスト
        private void Start()
        {
            PlayTest();
        }

        private async UniTask PlayTest()
        {
            await PlaySE(se.opendoor1, 100, false, 0, 0, Easing.Ease.None, 0);

            await PlaySE(se.opendoor1, 100, true, 0, 0, Easing.Ease.None, 0);

            // await PlaySE(se.opendoor1, 100, true, 0, 0, Easing.Ease.Linear, 0);

            // await PlaySE(se.opendoor1, 100, true, 0, 0, Easing.Ease.InQuint, 0);

            // await PlaySE(bgm.acoustic52, 50, true, 0, 20, Easing.Ease.OutQuint, 10);

            // await PlaySE(se.opendoor1, 100, true, 0, 0, Easing.Ease.InQuint, 0);
        }

        public async UniTask PlaySE(string seName, int volume, bool waitComplete, float startSec, float endSec, Easing.Ease fadeInEasing, float fadeSec)
        {
            // seのファイルを探して、なければエラー出力
            AudioClip audioClip = Resources.Load<AudioClip>(Settings.RESOURCES_PATH_SE + seName);
            if (audioClip == null)
            {
                Debug.LogError($"Play SE Error : Audio file {seName} is not exist in Resources/{Settings.RESOURCES_PATH_SE}.");
                return;
            }

            // オブジェクトプールから使えるインデックスを取得する
            var poolIdx = GetUseableSEPoolIndex();
            var audioSource = _sePools[poolIdx].AudioSource;
            
            // 音声ファイルの設定
            audioSource.clip = audioClip;

            // 音量パラメータ(0～100)の取得と設定(0～1)
            var savedata = SaveDataHolder.I;
            float playVolume = (float)(volume * savedata.VolumeMaster * savedata.VolumeSE) / 1000000.0f;
            audioSource.volume = playVolume;

            // 無効な開始位置、終了位置の場合に警告を出して値を調整する
            (startSec, endSec) = GetValidStartEndTime(startSec, endSec, audioClip, seName); 

            // 開始位置の調整
            audioSource.time = startSec;
            // 再生時間の設定
            float playSec = endSec == 0 ? (audioClip.length - startSec) : (endSec - startSec);

            // フェード時間の調整
            if(fadeInEasing != Easing.Ease.None)
            {
                if(fadeSec != 0)
                {
                    // フェード時間が有効かどうか調べる
                    if(playSec / 2.0f < fadeSec)
                    {
                        fadeSec = playSec / 4.0f;
                        Debug.LogError($"Play SE Error : fadeSec of {seName} is invalid.\nReconfigure fadeSec less than (endSec - startSec)/2 ({playSec/2}).");
                    }
                }
                // 指定がない場合
                else
                {
                    // フェードインとアウトの時間を再生時間の1/4にする
                    fadeSec = playSec / 4.0f;
                }
            }

            // SEを鳴らす
            audioSource.Play();

            // 再生開始まで待機する
            await UniTask.WaitUntil( () => audioSource.isPlaying );
            
            // 終了を待機する場合
            if(waitComplete)
            {
                // フェードがある場合
                if(fadeInEasing != Easing.Ease.None) { await Fade(fadeSec, playSec, playVolume, fadeInEasing, audioSource, poolIdx); }
                // フェードがない場合
                else { await StopAfterSec(playSec, audioSource, poolIdx); }
            }
            // 終了を待機しない場合は、別スレッドで実行する
            else
            {
                // フェードがある場合
                if(fadeInEasing != Easing.Ease.None) { Fade(fadeSec, playSec, playVolume, fadeInEasing, audioSource, poolIdx).Forget(); }
                // フェードがない場合
                else { StopAfterSec(playSec, audioSource, poolIdx).Forget(); }
            }
        }

        // 再生位置と終了位置を正常な値に収めて取得する
        private static (float, float) GetValidStartEndTime(float startSec, float endSec, AudioClip audioClip, string soundName)
        {
            // 無効な開始位置、終了位置の場合に警告を出して値を調整する
            if (startSec < 0 || audioClip.length < startSec)
            {
                startSec = 0;
                Debug.LogError($"Play SE Error : startSec of {soundName} ({startSec}) is invalid.\nLength of {soundName} is {audioClip.length}.");
            }
            if(endSec < 0 || audioClip.length < endSec)
            {
                endSec = audioClip.length;
                
            }
            // 開始位置 > 終了位置の場合にもエラー
            if(startSec > endSec)
            {
                startSec = 0;
                endSec = audioClip.length;
                Debug.LogError($"Play SE Error : startSec({startSec}) larger than endSec({endSec}) of {soundName}.");
            }
            return (startSec, endSec);
        }

        // 指定した時間でフェードインを行う
        // minVolume(0.0~1.0), maxVolume(0.0~1.0)
        private async UniTask FadeIn(float fadeSec, float minVolume, float maxVolume, AudioSource audioSource, Easing.Ease ease)
        {
            audioSource.volume = minVolume;

            // 目標の音量との差
            var difVolume = Mathf.Abs(maxVolume - minVolume);
            // イージング関数の取得
            var Ease = Easing.GetEasing(ease);

            float time = 0; // 0 ~ 1
            while(true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                time += Time.deltaTime / fadeSec;
                if(time >= 1.0f)
                {
                    audioSource.volume = maxVolume;
                    break;
                }
                audioSource.volume = minVolume + difVolume * Ease(time);
            }
        }

        // 指定した時間でフェードアウトを行う
        // minVolume(0.0~1.0), maxVolume(0.0~1.0)
        private async UniTask FadeOut(float fadeSec, float minVolume, float maxVolume, AudioSource audioSource, Easing.Ease ease)
        {
            audioSource.volume = maxVolume;

            // 目標の音量との差
            var difVolume = Mathf.Abs(maxVolume - minVolume);
            // イージング関数の取得
            var Ease = Easing.GetEasing(ease);

            float time = 1.0f; // 0 ~ 1
            while(true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                time -= Time.deltaTime / fadeSec;
                if(time <= 0.0f)
                {
                    audioSource.volume = minVolume;
                    break;
                }
                audioSource.volume = minVolume + difVolume * Ease(time);
            }
        }

        // フェードを行う関数
        private async UniTask Fade(float fadeSec, float playSec, float playVolume, Easing.Ease fadeInEasing, AudioSource audioSource, int poolIdx)
        {
            // フェードインしながら再生する
            await FadeIn(fadeSec, 0, playVolume, audioSource, fadeInEasing);

            // フェードインとフェードアウトの間の時間だけ待つ
            float waiiSecBetweenFadeInOut = playSec - 2 * fadeSec;
            await UniTask.Delay((int)(waiiSecBetweenFadeInOut * 1000.0f));

            // フェードアウトしながら再生する
            await FadeOut(fadeSec, 0, playVolume, audioSource, fadeInEasing);

            audioSource.Stop();

            // オブジェクトプールの使用フラグを解除する
            _sePools[poolIdx].IsUsing = false;
        }

        // N秒後に再生をやめる関数
        private async UniTask StopAfterSec(float playSec, AudioSource audioSource, int poolIdx)
        {
            await UniTask.Delay((int)(playSec * 1000.0f));
            audioSource.Stop();

            // オブジェクトプールの使用フラグを解除する
            _sePools[poolIdx].IsUsing = false;
        }
    }
}