using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    //scene ref
    [SerializeField] RectTransform _titleImage = null;
    [SerializeField] RectTransform _wheel = null;
    [SerializeField] UI_WheelHandler _wheelHandler = null;

    //settings
    [SerializeField] float _tweenDuration = 2f;

    [SerializeField] float _titleImage_down = 0f;
    [SerializeField] float _titleImage_up = 300f;
    [SerializeField] float _titleImage_off = 600f;
    Tween _titleTween;
    
    [SerializeField] float _wheelImage_down = -489f;
    [SerializeField] float _wheelImage_up = -310f;
    [SerializeField] float _wheelImage_off = -817f;
    Tween _wheelTween;

    public enum Context { Pretitle, Title, Wheel, Gameplay, GameOver}
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetContext(Context.Pretitle);
    }

    private void SetContext(Context context)
    {
       switch (context)
        {
            case Context.Pretitle:
                _titleTween.Kill();
                _titleImage.anchoredPosition = new Vector2(0, _titleImage_off);
                _wheelTween.Kill();
                _wheel.anchoredPosition = new Vector2(0, _wheelImage_off);
                break;

            case Context.Title:
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_down, _tweenDuration);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_down, _tweenDuration);
                _wheelHandler.DeactivateWheelCells();
                break;

            case Context.Wheel:
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_up, _tweenDuration);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_up, _tweenDuration);
                _wheelHandler.ActivateWheelCells();
                break;
        }
    }

    [ContextMenu("set pretitle")]
    public void SetPretitle()
    {
        SetContext(Context.Pretitle);
    }

    [ContextMenu("set title")]
    public void SetTitle()
    {
        SetContext(Context.Title);
    }

    [ContextMenu("set wheel")]
    public void SetWheel()
    {
        SetContext(Context.Wheel);
    }
}
