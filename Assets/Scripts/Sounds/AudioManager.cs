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
            AudioClip clip = Resources.Load(path) as AudioClip;
            AudioSource source = gameObject.AddComponent<AudioSource>();

            if (Resources.Load(path) == null)
            {
                print("clipはNULLです");
            }
            source.clip = clip;
            m_audioPool.Add(keyName, source);
        }
        else
        {
            // TODO: 開発完了後削除
            print(keyName + "は既に登録されています。");
        }
    }

    /// <summary>
    /// 再生処理
    /// </summary>
    /// <param name="audioKey">再生したい音楽ファイルのKey</param>
    /// <param name="isLoop">ループの可否：true:ループ/false:ループしない</param>
    /// <param name="isAwake">生成時の再生の可否：true:再生/false:再生しない</param>
    public void Play(string audioKey, bool isLoop = false, bool isAwake = false)
    {
        AudioSource source = m_audioPool[audioKey];
        print(audioKey+"を再生します。");

        source.loop = isLoop;
        source.playOnAwake = isAwake;

        source.Play();
    }

    /// <summary>
    /// 停止処理
    /// </summary>
    public void Stop(string key)
    {
        m_audioPool[key].Stop();
    }
}