using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private float volume = .5f;
    private AudioSource audioSource;
    private const string MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(MUSIC_VOLUME, .5f);
        audioSource.volume = volume;
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume >= 1.1f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
