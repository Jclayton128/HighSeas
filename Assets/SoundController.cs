using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }
    [SerializeField] AudioSource _soundAuso = null;
    [SerializeField] AudioSource _musicAuso_intro = null;
    [SerializeField] AudioSource _musicAuso_main = null;
    [SerializeField] AudioClip _windIntro = null;

    //state
    float _soundVolume = 1;

    float _musicVolume_Master = 1;
    float _musicVolume_intro = 1;
    float _musicVolume_main = 1;

    private void Awake()
    {
        Instance = this;
        _soundAuso.volume = _soundVolume;
        _musicAuso_main.volume = 0f;
        _musicAuso_intro.volume = 0f;       
        
    }

    private void Start()
    {
        _soundAuso.PlayOneShot(_windIntro);
        GameController.Instance.GameModeStarted += HandleGameModeStarted;
        Invoke(nameof(Delay_Start), 3f);
    }

    private void Delay_Start()
    {
        _musicAuso_intro.volume = 1 * _musicVolume_Master;
        _musicAuso_intro.Play();
    }

    private void HandleGameModeStarted()
    {
        _musicAuso_intro.DOFade(0, 4f);
        _musicAuso_main.Play();
        _musicAuso_main.DOFade(1 * _musicVolume_Master, 4f);
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
