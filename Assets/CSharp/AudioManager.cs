using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [Header("BGM")]
    public AudioClip BGM;
    public AudioSource BGMSource;
    [Header("SoundFX")]
    public AudioClip[] SoundFX;
    public AudioSource[] EffectSource;
    [Header("Interface Sound")]
    public AudioClip[] InterfaceEffect;
    public AudioSource InterfaceSource;
    [Header("Pitch")]
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;
    [Header("Volume")]
    public float GlobalEffectVolume = 0.7f;
    public float GlobalBGMVoume = 0.45f;

    private Dictionary<string, AudioSource> OnPlayingEffectSource;

    public static AudioManager Instance = null;
	// Use this for initialization
	void Awake () {
		if(Instance == null)
        {
            OnPlayingEffectSource = new Dictionary<string, AudioSource>();
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

    /// <summary>
    /// Set random pitch to the audiosource
    /// </summary>
    private void SetRandomPitch(AudioSource audioSource)
    {
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        audioSource.pitch = randomPitch;
    }

    /// <summary>
    /// Play the background music
    /// </summary>
    public void PlayBGM()
    {
        BGMSource.clip = BGM;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    /// <summary>
    /// Play a soundfx from loaded clips
    /// </summary>
    /// <param name="loop"> Whether loop the soundfx </param>
    public void PlayEffect(string clipName, bool loop)
    {
        AudioSource tempSource = null;
        foreach(AudioSource s in EffectSource)
        {
            if(s.isPlaying == false)
            {
                tempSource = s;
            }
        }
        if (tempSource == null)
        {
            Debug.Log("[AudioManager]: SoundFX AudioSource Runout ");
        }
        else
        {
            SetRandomPitch(tempSource);
            foreach (AudioClip c in SoundFX)
            {
                if (c.name.Equals(clipName))
                {
                    tempSource.clip = c;
                    tempSource.Play();
                    OnPlayingEffectSource.Add(clipName, tempSource);
                    break;
                }
                else
                    Debug.Log("[AudioManager]: SoundFX Match Failure ");

            }
        }
    }

    /// <summary>
    /// Play Interface Sound
    /// </summary>
    public void PlayInterface(string clipName)
    {
        foreach (AudioClip c in InterfaceEffect)
        {
            if (c.name.Equals(clipName))
            {
                InterfaceSource.clip = c;
                InterfaceSource.Play();
                break;
            }
            else
                Debug.Log("[AudioManager]: InterfaceEffect Match Failure ");

        }
    }
    /// <summary>
    /// Stop a sound effect
    /// </summary>
    /// <param name="fadeOut"> Stop the soundFX by fadingout </param>
    /// <param name="fadeOutTime"> The fade out time, default 1f </param>
    public void StopEffect(string clipName, bool fadeOut, float fadeoutTime = 1f)
    {
        AudioSource tempSource;
        if(OnPlayingEffectSource.ContainsKey(clipName))
        {
            tempSource = OnPlayingEffectSource[clipName];
            if (fadeOut == true)            
                FadeOut(tempSource, fadeoutTime);
            else
                tempSource.Stop();
        }
        else
        {
            Debug.Log("[AudioManager]: The SoundFX is not playing");
        }
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void SetGlobalEffectVolume(float volume)
    {
        foreach(AudioSource s in EffectSource)
        {
            s.volume = volume;
        }
    }

    public void SetGobalBGMVolume(float volume)
    {
        BGMSource.volume = volume;
    }

    private void FadeOut(AudioSource audioSource, float fadeOutTime)
    {
        StartCoroutine(AudioFadeOut.FadeOut(audioSource, fadeOutTime));
    }
}
public static class AudioFadeOut
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}