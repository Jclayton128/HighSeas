using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    [SerializeField] AudioSource _soundAuso = null;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayClip(SoundLibrary.SoundID soundID)
    {
        _soundAuso.PlayOneShot(SoundLibrary.Instance.GetClip(soundID));
    }

    public void PlayClip(SoundLibrary.SoundID soundID, int forcedIndex)
    {
        _soundAuso.PlayOneShot(SoundLibrary.Instance.GetClip(soundID, forcedIndex));
    }

}
