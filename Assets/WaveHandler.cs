using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaveHandler : MonoBehaviour
{
    TileHandler _th;

    //state
    [SerializeField] float _travel;
    Vector3 _startingLocalPos;
    float _delta;
    [SerializeField] bool _isDescending;
    Tween _moveTween;

    private void Awake()
    {
        _th = GetComponentInParent<TileHandler>();
        _startingLocalPos = transform.localPosition;
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0) _isDescending = false;
        else _isDescending = true;
    }

    private void Start()
    {
        _delta = UnityEngine.Random.Range(-.1f, .1f) *
            TileController.Instance.WaveAmplitude;

        if (!_th) return;
        if (_th.TileType == TileController.TileType.ShallowWater ||
            _th.TileType == TileController.TileType.DecoyWater)
        {
            InitiateWaveLap();
        }
    }

    //private void HandleSpriteChanged()
    //{
    //    _moveTween.Kill();
    //    _travel = 0;
    //    //transform.localPosition = _startingLocalPos;
    //    if (_th.TileSprite == TileController.TileSprite.Water)
    //    {
    //        _shouldWave = true;
    //        //InitiateWaveLap();
    //    }
    //    else
    //    {
    //        _shouldWave = false;
    //        _moveTween.Kill();
    //    }
    //}

    private void InitiateWaveLap()
    {
        if (_isDescending)
        {
            _moveTween = transform.DOLocalMoveY(_startingLocalPos.y + TileController.Instance.WaveAmplitude + _delta,
                (TileController.Instance.WaveFrequency + _delta)).SetEase(Ease.InOutSine).OnComplete(ToggleWaveLap);
        }
        else
        {
            _moveTween = transform.DOLocalMoveY(_startingLocalPos.y - TileController.Instance.WaveAmplitude + _delta,
                (TileController.Instance.WaveFrequency + _delta)).SetEase(Ease.InOutSine).OnComplete(ToggleWaveLap);
        }
    }

    private void ToggleWaveLap()
    {
        if (_isDescending)
        {
            _isDescending = false;
        }
        else
        {
            _isDescending = true;
        }
        InitiateWaveLap();
    }

}

