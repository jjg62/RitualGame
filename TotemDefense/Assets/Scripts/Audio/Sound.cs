using UnityEngine.Audio;
using UnityEngine;

//Some code taken from Brackeys tutorial.
//Encapsulates information about a sound, which can be edited in inspector
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip; //The audio file
    [Range(0,1)]
    public float volume = 1;
    [Range(0.1f, 3)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
