using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSrc;
    public float musicVolume = 0.2f;
    public Slider slider;

    void Start()
    {
        audioSrc.volume = musicVolume;
    }
    
    public void SetVolume()
    {
        Debug.Log(slider.value);
        musicVolume = slider.value;
        audioSrc.volume = musicVolume;
    }
}
