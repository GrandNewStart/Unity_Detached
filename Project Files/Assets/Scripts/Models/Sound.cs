using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public static float maxVolume   = 3f;
    public static float maxPitch    = 1f;

    public string       name;
    public AudioClip    clip;
    public AudioSource  source;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;
}
