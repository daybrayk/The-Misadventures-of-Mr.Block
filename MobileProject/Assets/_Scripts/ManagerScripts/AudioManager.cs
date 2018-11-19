using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    private static AudioManager _instance = null;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    private void Awake()
    {
        if (instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            DestroyImmediate(this.gameObject);
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void PlaySFX(AudioClip clip, bool loop = false)
    {
        sfxSource.clip = clip;
        sfxSource.loop = loop;
    }

    public void SFXVolume(float volume)
    {
        Debug.Log(volume);
        sfxSource.volume = volume;
    }

    public void MuteSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    /*************** Getters and Setters ***************/
    public static AudioManager instance
    {
        get { return _instance; }
    }
}
