using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RockingHandler : MonoBehaviour
{

    //state
    float _delta;
    bool _isRockingRight;
    Tween _moveTween;
    Vector3 _rotation;

    private void Awake()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0) _isRockingRight = false;
        else _isRockingRight = true;
    }

    private void Start()
    {
        _rotation = transform.localRotation.eulerAngles;
        //JUICE TODO make rocking based on current speed

        InitiateWaveLap();

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
        if (_isRockingRight)
        {
            _rotation.z = MovementController.Instance.RockingAmplitude;
            _moveTween = transform.DOLocalRotate(_rotation, 
                MovementController.Instance.RockingPeriod).SetEase(Ease.InOutSine).OnComplete(ToggleWaveLap);

        }
        else
        {
            _rotation.z = -MovementController.Instance.RockingAmplitude;
            _moveTween = transform.DOLocalRotate(_rotation,
                MovementController.Instance.RockingPeriod).SetEase(Ease.InOutSine).OnComplete(ToggleWaveLap);
        }
    }

    private void ToggleWaveLap()
    {
        if (_isRockingRight)
        {
            _isRockingRight = false;
        }
        else
        {
            _isRockingRight = true;
        }
        InitiateWaveLap();
    }
}
