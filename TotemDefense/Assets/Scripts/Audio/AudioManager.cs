using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

//Some code taken from Brackeys audio tutorial.
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    
    private void Awake()
    {
        //Turn list of sounds in inspector to components
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //Search for sound and play it
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null) s.source.Play();
    }


    //Fade out volume of sound
    public void FadeOut(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); 
        if (s != null) StartCoroutine(FadeOut(s, duration));
    }

    IEnumerator FadeOut(Sound s, float duration)
    {
        float startVolume = s.volume;
        while(s.volume > 0)
        {
            //Gradually decrease volume
            s.source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        //Stop sound and reset volume
        s.source.Stop();
        s.source.volume = startVolume;

    }

}
