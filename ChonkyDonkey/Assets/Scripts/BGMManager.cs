using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioSource musicPlayer;

        public float Volume 
    {
        get => musicPlayer.volume;
        set => musicPlayer.volume = value;
    }

    public bool SoundEnabled
    {
        get => !musicPlayer.mute;
        set => musicPlayer.mute = !value;
    }
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        musicPlayer.clip = music;
        musicPlayer.loop = true;
        musicPlayer.Play();
    }
}
