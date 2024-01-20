using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_WheelHandler : MonoBehaviour
{
    [SerializeField] Image _wheelImage_cells = null;
    [SerializeField] Image _wheelImage_nocells = null;
    [SerializeField] Image[] _slotIcons = null;

    //settings
    [SerializeField] float _timePerStep = 2f;
    [SerializeField] float _rotationPerStep = 22.5f;
    [SerializeField] int _startingStep = 1;
    [SerializeField] Sprite _wheel_NoCells = null;
    [SerializeField] Sprite _wheel_Cells = null;

    //state
    [SerializeField] int _currentStep = 0;
    public int CurrentStep => _currentStep;
    Tween _turnTween;

    [SerializeField] Vector3 _angleWheel = Vector3.zero;
    //[SerializeField] Vector3 _angleIcon = Vector3.zero;

    private void Start()
    {
        _wheelImage_cells.CrossFadeAlpha(0, 0.001f, false);
        foreach (Image icon in _slotIcons)
        {
            icon.CrossFadeAlpha(0, 0.001f, false);
        }

        _currentStep = _startingStep;
        _angleWheel.z = _startingStep * _rotationPerStep/2f;
        //_angleIcon.z = _slotIcons[0].transform.localEulerAngles.z;
    }

    //private void Update()
    //{
    //    if (!gameObject.activeSelf) return;
    //    foreach (var icon in _slotIcons)
    //    {
    //        _angleIcon.z = -transform.localRotation.eulerAngles.z;
    //        icon.transform.localRotation = Quaternion.Euler(_angleIcon);
    //    }
    //}

    [ContextMenu("Activate")]
    public void ActivateWheelCells()
    {
        _wheelImage_cells.CrossFadeAlpha(1, 1f, false);
        _wheelImage_nocells.CrossFadeAlpha(0, 2f, false);
        foreach (Image icon in _slotIcons)
        {
            icon.CrossFadeAlpha(1, 2f, false);
        }
    }

    [ContextMenu("Deact")]
    public void DeactivateWheelCells()
    {
        _wheelImage_cells.CrossFadeAlpha(0, 2f, false);
        _wheelImage_nocells.CrossFadeAlpha(1, 1f, false);
        foreach (Image icon in _slotIcons)
        {
            icon.CrossFadeAlpha(0, 1f, false);
        }
    }


    [ContextMenu("Increment")]
    public void IncrementTurn()
    {
        _currentStep++;
        if (_currentStep > 7)
        {
            _currentStep = 0;
        }
        _angleWheel.z += _rotationPerStep;
        _turnTween.Kill();
        _turnTween = _wheelImage_cells.transform.DOLocalRotate(_angleWheel, _timePerStep, RotateMode.Fast).SetEase(Ease.OutElastic);
    }

    [ContextMenu("Decrement")]
    public void DecrementTurn()
    {
        _currentStep--;
        if (_currentStep < 0)
        {
            _currentStep = 7;
        }
        _angleWheel.z -= _rotationPerStep;
        _turnTween.Kill();
        _turnTween = _wheelImage_cells.transform.DOLocalRotate(_angleWheel, _timePerStep, RotateMode.Fast).SetEase(Ease.OutElastic);
    }
}
