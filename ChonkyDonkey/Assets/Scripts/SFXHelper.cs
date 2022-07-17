using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple script handler for sound effects. There will need to be an AudioSource added to the scene
 * that is referenced by this script. The only other thing is to place sound files into the Resources 
 * folder of the project. Create a variable to store x sound file. Then add a switch case for it, so 
 * it can be found when the sound effect needs to be played. 
 */

public class SFXHelper : Singleton<SFXHelper>
{
    // Examples
    private static AudioClip audioClip1, audioClip2;
    public const string audioClip1Name = "DiceRollSound"; // FileName
    public const string audioClip2Name = ""; // FileName
    private static AudioClip mainMenuSqueaky;
    public const string mainMenuSqueakyName = "Sound/MainMenuSqueakyToy";

    private static AudioSource _audioSource;

    public float Volume 
    {
        get => _audioSource.volume;
        set => _audioSource.volume = value;
    }

    public bool SoundEnabled
    {
        get => !_audioSource.mute;
        set => _audioSource.mute = !value;
    }

    private void Awake() {
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();

        //audioClipName = Resources.Load<AudioClip>(audioClip1Name);

        mainMenuSqueaky = Resources.Load<AudioClip>(mainMenuSqueakyName);
    }

    public static void PlaySound(string clip) {
        switch (clip) {
            case audioClip1Name: // FileName used in Awake
                //audioSource.PlayOneShot(audioClip1Name);
                break;
            case mainMenuSqueakyName:
                _audioSource.PlayOneShot(mainMenuSqueaky);
                break;
            default:
                break;
        }
    }
}
