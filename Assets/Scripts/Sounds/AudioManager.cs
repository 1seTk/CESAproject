//＋－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－
//｜使用例
//｜ファイルのセット
//｜AudioManager.Instance.AudioSet(“Key”,”path”);
//｜
//｜再生(無指定)
//｜AudioManager.Instance.Play(“Key”);
//｜
//｜再生(ループ、生成時再生 指定あり)
//｜AudioManager.Instance.Play(“Key”,isLoop:true,isAwake:true);
//＋－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－

using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Audio再生クラス
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// AudioFile保持用変数
    /// </summary>
    private Dictionary<string, AudioSource> m_audioPool = new Dictionary<string, AudioSource>();

    /// <summary>
    /// シングルトン用変数
    /// </summary>
    private static AudioManager m_instance = null;

    /// <summary>
    /// コルーチン中断に使用
    /// </summary>
    private IEnumerator fadeOutCoroutine;
    /// <summary>
    /// コルーチン中断に使用
    /// </summary>
    private IEnumerator fadeInCoroutine;

    /// <summary>
    /// シングルトン
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                m_instance = obj.AddComponent<AudioManager>();
            }
            return m_instance;
        }
    }

    /// <summary>
    /// 音楽ファイルのセット処理
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="path"></param>
    public void AudioSet(string keyName, string path)
    {
        // 登録されていないkeyの場合登録
        if (!m_audioPool.ContainsKey(keyName))
        {
            string pathBase = path;
            AudioClip clip = Resources.Load(pathBase) as AudioClip;
            AudioSource source = gameObject.AddComponent<AudioSource>();


            if (Resources.Load(pathBase) == null)
            {
                print("clipはNULLです");
                return;
            }

            source.clip = clip;
            m_audioPool.Add(keyName, source);
        }
        else
        {// されている場合
            //source.clip = clip;
            //m_audioPool[keyName] = source;
        }
    }

    /// <summary>
    /// 再生処理
    /// </summary>
    /// <param name="audioKey">再生したい音楽ファイルのKey</param>
    /// <param name="isLoop">ループの可否：true:ループ/false:ループしない</param>
    /// <param name="isAwake">生成時の再生の可否：true:再生/false:再生しない</param>
    public void Play(string audioKey)
    {
        m_audioPool[audioKey].Play();
    }

    /// <summary>
    /// 再生処理
    /// </summary>
    /// <param name="audioKey">再生したい音楽ファイルのKey</param>
    /// <param name="isLoop">ループの可否：true:ループ/false:ループしない</param>
    /// <param name="isAwake">生成時の再生の可否：true:再生/false:再生しない</param>
    public void SetOption(string audioKey, bool isLoop = false, bool isAwake = false)
    {
        AudioSource source = m_audioPool[audioKey];
        source.loop = isLoop;
        source.playOnAwake = isAwake;
    }

    /// <summary>
    /// ボリュームの設定
    /// </summary>
    /// <param name="audioKey">変更する音のキー</param>
    /// <param name="volume">音量(0.0f~1.0f)</param>
    public void SetVolume(string audioKey,float volume)
    {
        m_audioPool[audioKey].volume = volume;
    }


    /// <summary>
    /// 停止処理
    /// </summary>
    public void Stop(string key)
    {
        m_audioPool[key].Stop();
    }

    /// <summary>
    /// BGMをフェードインさせながら再生を開始します。
    /// </summary>
    /// <param name="bgm">AudioSource</param>
    /// <param name="timeToFade">フェードインにかかる時間</param>
    /// <param name="delay">フェードイン開始までの待ち時間</param>
    public IEnumerator fadeIn(string key, float timeToFade,float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }


        float startTime = Time.time;
        m_audioPool[key].Play();
        while (true)
        {
            float spentTime = Time.time - startTime;
            if (spentTime > timeToFade)
            {
                m_audioPool[key].volume = 1.0f;
                this.fadeInCoroutine = null;
                break;
            }

            float rate = spentTime / timeToFade;
            float vol = Mathf.Lerp(0.0f, 1.0f, rate);
            m_audioPool[key].volume = vol;
            yield return null;
        }
    }

    /// <summary>
    /// BGMをフェードアウトし、その後停止します。
    /// </summary>
    /// <param name="bgm">フェードアウトさせるAudioSource</param>
    /// <param name="timeToFade">フェードアウトにかかる時間</param>
    public IEnumerator fadeOut(string key, float timeToFade)
    {
        float startTime = Time.time;
        while (true)
        {
            float spentTime = Time.time - startTime;
            if (spentTime > timeToFade)
            {
                m_audioPool[key].volume = 0.0f;
                m_audioPool[key].Stop();
                this.fadeOutCoroutine = null;
                break;
            }

            float rate = spentTime / timeToFade;
            float vol = Mathf.Lerp(1.0f, 0.0f, rate);
            m_audioPool[key].volume = vol;
            yield return null;
        }
    }
    /// <summary>
    /// フェードイン処理を中断。
    /// </summary>
    private void stopFadeIn()
    {
        if (this.fadeInCoroutine != null)
            StopCoroutine(this.fadeInCoroutine);
        this.fadeInCoroutine = null;

    }

    /// <summary>
    /// フェードアウト処理を中断。
    /// </summary>
    private void stopFadeOut()
    {
        if (this.fadeOutCoroutine != null)
            StopCoroutine(this.fadeOutCoroutine);
        this.fadeOutCoroutine = null;
    }

    public bool IsPlaying(string key)
    {
        return m_audioPool[key].isPlaying;
    }
}