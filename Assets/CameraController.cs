using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public enum ZoomLevel { Close, Normal, Wide};
    public static CameraController Instance { get; private set; }

    //settings
    [Tooltip("0: close, 1: normal, 2: wide")]
    [SerializeField] float[] _zoomLevels = new float[3] { 3f, 4f, 5f };
    [SerializeField] float _zoomDuration = 3f;
    float _zOffset = -10f;

    //state
    Tween _zoomTween;
    Tween _posTween;


    private void Awake()
    {
        Instance = this;
        
    }


    public void SetZoom(ZoomLevel zoomLevel, Transform targetTransform)
    {
        _zoomTween.Kill();
        _posTween.Kill();
        _zoomTween = Camera.main.DOOrthoSize(_zoomLevels[(int)zoomLevel], _zoomDuration)
            .SetEase(Ease.InCirc);

        if (targetTransform != null)
        {
            _posTween = Camera.main.transform.DOMove(
                new Vector3(targetTransform.position.x, targetTransform.position.y, _zOffset),
                _zoomDuration);
        }
        else
        {
            _posTween = Camera.main.transform.DOMove(
                new Vector3(transform.position.x, transform.position.y, _zOffset),
                _zoomDuration);
        }
    }

}
