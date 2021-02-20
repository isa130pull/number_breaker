using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[System.Serializable]
public class SoundVolume
{
    public float bgm = 1.0f;
    public float se = 1.0f;

    public bool bgmMute = false;
    public bool seMute = false;

    public void Reset()
    {
        this.bgm = 1.0f;
        this.se = 1.0f;
        this.bgmMute = false;
        this.seMute = false;
    }
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public SoundVolume volume = new SoundVolume();

    private AudioClip[] seClips;
    private AudioClip[] bgmClips;

    private readonly Dictionary<string, int> seIndexes = new Dictionary<string, int>();
    private readonly Dictionary<string, int> bgmIndexes = new Dictionary<string, int>();

    private const int cNumChannel = 16;
    private AudioSource bgmSource;
    private readonly AudioSource[] seSources = new AudioSource[cNumChannel];

    readonly Queue<int> seRequestQueue = new Queue<int>();

    //------------------------------------------------------------------------------
    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        this.bgmSource = this.gameObject.AddComponent<AudioSource>();
        this.bgmSource.loop = true;

        for (var i = 0; i < this.seSources.Length; i++)
        {
            this.seSources[i] = this.gameObject.AddComponent<AudioSource>();
        }

        this.seClips = Resources.LoadAll<AudioClip>("Audio/SE");
        this.bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");

        for (var i = 0; i < this.seClips.Length; ++i)
        {
            this.seIndexes[this.seClips[i].name] = i;
        }

        for (var i = 0; i < this.bgmClips.Length; ++i)
        {
            this.bgmIndexes[this.bgmClips[i].name] = i;
        }
    }

    //------------------------------------------------------------------------------
    private void Update()
    {
        this.bgmSource.mute = this.volume.bgmMute;
        foreach (var source in this.seSources)
        {
            source.mute = this.volume.seMute;
        }

        this.bgmSource.volume = this.volume.bgm;
        foreach (var source in this.seSources)
        {
            source.volume = this.volume.se;
        }

        var count = this.seRequestQueue.Count;
        if (count != 0)
        {
            var sound_index = this.seRequestQueue.Dequeue();
            playSeImpl(sound_index);
        }
    }

    //------------------------------------------------------------------------------
    private void playSeImpl(int index)
    {
        if (0 > index || this.seClips.Length <= index)
        {
            return;
        }

        foreach (AudioSource source in this.seSources)
        {
            if (false == source.isPlaying)
            {
                source.clip = this.seClips[index];
                source.Play();
                return;
            }
        }
    }

    //------------------------------------------------------------------------------
    private int GetSeIndex(string name)
    {
        return this.seIndexes[name];
    }

    //------------------------------------------------------------------------------
    public int GetBgmIndex(string name)
    {
        return this.bgmIndexes[name];
    }

    //------------------------------------------------------------------------------
    public void PlayBgm(string name, float fadeTime = 0f)
    {
        var index = this.bgmIndexes[name];
        PlayBgm(index, fadeTime);
    }

    //------------------------------------------------------------------------------
    public void PlayBgm(int index, float fadeTime)
    {
        if (0 > index || this.bgmClips.Length <= index)
        {
            return;
        }

        if (this.bgmSource.clip == this.bgmClips[index])
        {
            return;
        }
        
        if (fadeTime > 0)
        {
            this.volume.bgm = 0f;
            DOTween.To(() => this.volume.bgm,
                v => this.volume.bgm = v,
                1.0f, fadeTime);
        }
        else
        {
            this.volume.bgm = 1f;
        }
        
        this.bgmSource.Stop();
        this.bgmSource.clip = this.bgmClips[index];
        this.bgmSource.Play();
    }

    //------------------------------------------------------------------------------
    public void StopBgm(float fadeTime = 0.0f)
    {
        if (fadeTime > 0)
        {
            DOTween.To(() => this.volume.bgm,
                    v => this.volume.bgm = v,
                    0.0f, fadeTime)
                .OnComplete(() =>
                {
                    this.bgmSource.Stop();
                    this.bgmSource.clip = null;
                });
        }
        else
        {
            this.bgmSource.Stop();
            this.bgmSource.clip = null;
        }
    }

    public void ChangeBgmSpeed(float time)
    {
        this.bgmSource.pitch = time;
    }

    //------------------------------------------------------------------------------
    public void PlaySe(string name)
    {
        PlaySe(GetSeIndex(name));
    }

    //一旦queueに溜め込んで重複を回避しているので
    //再生が1frame遅れる時がある
    //------------------------------------------------------------------------------
    public void PlaySe(int index)
    {
        if (!this.seRequestQueue.Contains(index))
        {
            this.seRequestQueue.Enqueue(index);
        }
    }

    //------------------------------------------------------------------------------
    public void StopSe()
    {
        ClearAllSeRequest();
        foreach (var source in this.seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    //------------------------------------------------------------------------------
    public void ClearAllSeRequest()
    {
        this.seRequestQueue.Clear();
    }
}