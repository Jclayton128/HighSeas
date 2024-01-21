using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIController : MonoBehaviour
{

    public static UIController Instance { get; private set; }

    //scene ref
    [SerializeField] RectTransform _titleImage = null;
    [SerializeField] RectTransform _gameoverPanel = null;
    [SerializeField] RectTransform _wheel = null;
    [SerializeField] RectTransform _playerPanel_0 = null;
    [SerializeField] RectTransform _playerPanel_1 = null;
    [SerializeField] RectTransform _playerPanel_2 = null;
    [SerializeField] RectTransform _playerPanel_3 = null;
    [SerializeField] UI_WheelHandler _wheelHandler = null;
    [SerializeField] TextMeshProUGUI _helperTMP = null;

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

    [SerializeField] float _playerPanel_offLeft = -400f;
    [SerializeField] float _playerPanel_offRight = 400f;
    [SerializeField] float _playerPanel_inLeft = -100f;
    [SerializeField] float _playerPanel_inRight = 100f;
    Tween _playerPanelTween_0;
    Tween _playerPanelTween_1;
    Tween _playerPanelTween_2;
    Tween _playerPanelTween_3;

    [SerializeField] float _gameoverPanel_off = 400f;
    [SerializeField] float _gameoverPanel_down = 100f;
    Tween _gameoverTween;

    bool _isSwappingContexts = false;
    public bool IsSwappingContext => _isSwappingContexts;

    bool _isRotatingWheel = false;
    public bool IsRotatingWheel => _isRotatingWheel;



    public enum Context { Pretitle, Title, Wheel, Gameplay, GameOver}



    private void Awake()
    {
        Instance = this;
    }


    private void HandleContextSwapComplete()
    {
        _isSwappingContexts = false;
    }

    public void HandleWheelRotationComplete()
    {
        _isRotatingWheel = false;
    }
    
    public void HandleWheelRotationStarted()
    {
        _isRotatingWheel = true;
        //JUICE TODO play a wheel turning creak
    }

    public int IncreaseWheelAndGetCurrentStep()
    {
        _wheelHandler.IncrementTurn();
        return _wheelHandler.CurrentStep;
    }

    public int DecreaseWheelAndGetCurrentStep()
    {
        _wheelHandler.DecrementTurn();
        return _wheelHandler.CurrentStep;
    }

    public void SetContext(Context context)
    {
        SetContext(context, _tweenDuration);
    }

    public void SetContext(Context context, float duration)
    {
       switch (context)
        {
            case Context.Pretitle:
                _isSwappingContexts = true;
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_off, duration).OnComplete(HandleContextSwapComplete);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_off, duration);

                _playerPanelTween_0.Kill();
                _playerPanelTween_1.Kill();
                _playerPanelTween_2.Kill();
                _playerPanelTween_3.Kill();

                _playerPanelTween_0 = _playerPanel_0.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_3 = _playerPanel_3.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_1 = _playerPanel_1.DOAnchorPosX(_playerPanel_offRight, duration);
                _playerPanelTween_2 = _playerPanel_2.DOAnchorPosX(_playerPanel_offRight, duration);

                _helperTMP.CrossFadeAlpha(0, 0.001f, false);

                _gameoverTween.Kill();
                _gameoverTween = _gameoverPanel.DOAnchorPosY(_gameoverPanel_off, duration);
                break;

            case Context.Title:
                _isSwappingContexts = true;
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_down, duration).OnComplete(HandleContextSwapComplete);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_down, duration);
                _wheelHandler.DeactivateWheelCells();

                _playerPanelTween_0.Kill();
                _playerPanelTween_1.Kill();
                _playerPanelTween_2.Kill();
                _playerPanelTween_3.Kill();

                _playerPanelTween_0 = _playerPanel_0.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_3 = _playerPanel_3.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_1 = _playerPanel_1.DOAnchorPosX(_playerPanel_offRight, duration);
                _playerPanelTween_2 = _playerPanel_2.DOAnchorPosX(_playerPanel_offRight, duration);

                _gameoverTween.Kill();
                _gameoverTween = _gameoverPanel.DOAnchorPosY(_gameoverPanel_off, duration);
                break;

            case Context.Wheel:
                _isSwappingContexts = true;
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_up, duration).OnComplete(HandleContextSwapComplete);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_up, duration);
                _wheelHandler.ActivateWheelCells();
                _playerPanelTween_0.Kill();
                _playerPanelTween_1.Kill();
                _playerPanelTween_2.Kill();
                _playerPanelTween_3.Kill();

                _playerPanelTween_0 = _playerPanel_0.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_3 = _playerPanel_3.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_1 = _playerPanel_1.DOAnchorPosX(_playerPanel_offRight, duration);
                _playerPanelTween_2 = _playerPanel_2.DOAnchorPosX(_playerPanel_offRight, duration);

                _gameoverTween.Kill();
                _gameoverTween = _gameoverPanel.DOAnchorPosY(_gameoverPanel_off, duration);
                break;

            case Context.Gameplay:
                _isSwappingContexts = true;
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_off, duration).OnComplete(HandleContextSwapComplete);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_off, duration);
                _wheelHandler.DeactivateWheelCells();
                _playerPanelTween_0.Kill();
                _playerPanelTween_1.Kill();
                _playerPanelTween_2.Kill();
                _playerPanelTween_3.Kill();

                _playerPanelTween_0 = _playerPanel_0.DOAnchorPosX(_playerPanel_inLeft, duration);
                _playerPanelTween_3 = _playerPanel_3.DOAnchorPosX(_playerPanel_inLeft, duration);
                _playerPanelTween_1 = _playerPanel_1.DOAnchorPosX(_playerPanel_inRight, duration);
                _playerPanelTween_2 = _playerPanel_2.DOAnchorPosX(_playerPanel_inRight, duration);
                _helperTMP.CrossFadeAlpha(0, duration/2f, false);

                _gameoverTween.Kill();
                _gameoverTween = _gameoverPanel.DOAnchorPosY(_gameoverPanel_off, duration);
                break;

            case Context.GameOver:
                _isSwappingContexts = true;
                _titleTween.Kill();
                _titleTween = _titleImage.DOAnchorPosY(_titleImage_off, duration).OnComplete(HandleContextSwapComplete);
                _wheelTween.Kill();
                _wheelTween = _wheel.DOAnchorPosY(_wheelImage_off, duration);
                _wheelHandler.DeactivateWheelCells();
                _playerPanelTween_0.Kill();
                _playerPanelTween_1.Kill();
                _playerPanelTween_2.Kill();
                _playerPanelTween_3.Kill();

                _playerPanelTween_0 = _playerPanel_0.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_3 = _playerPanel_3.DOAnchorPosX(_playerPanel_offLeft, duration);
                _playerPanelTween_1 = _playerPanel_1.DOAnchorPosX(_playerPanel_offRight, duration);
                _playerPanelTween_2 = _playerPanel_2.DOAnchorPosX(_playerPanel_offRight, duration);
                _helperTMP.CrossFadeAlpha(0, 0.001f, false);

                _gameoverTween.Kill();
                _gameoverTween = _gameoverPanel.DOAnchorPosY(_gameoverPanel_down, duration);
                break;
        }
    }

    public void SetHelperText(string text)
    {
        _helperTMP.CrossFadeAlpha(1, _tweenDuration, false);
        _helperTMP.text = text;
    }


}
