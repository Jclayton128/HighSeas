using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballHandler : MonoBehaviour
{

    //references
    [SerializeField] SpriteRenderer _srCannonball;
    [SerializeField] SpriteRenderer _srShadow;
    Collider2D _coll;

    //settings
    [SerializeField] float _scaleAtApex = 1.3f;
    [SerializeField] float _yAdjustStart = 0.08f;
    [SerializeField] float _yAdjustApex = 3f;

    //instance state
    [SerializeField] float _lifetimeFactor;
    float _totalLifetime;
    float _remainingLifetime;
    float _deltaFactorFromMidpoint;
    Vector3 _velocity;
    bool _isActive = false;
    Vector3 _scale = Vector3.one;
    Vector3 _yAdjust = Vector3.zero;

    public void InitializeCannonball()
    {
        _coll = GetComponent<Collider2D>();
    }

    public void SetupCannonballInstance(Vector3 spawnPosition, Vector3 targetPosition)
    {
        _srCannonball.enabled = true;
        _srShadow.enabled = true;
        _coll.enabled = true;

        transform.position = spawnPosition;
        Vector3 spread = (targetPosition - spawnPosition);
        Vector3 dirToTarget = spread.normalized;
        float distToTarget = spread.magnitude;
        _velocity = dirToTarget * CannonController.Instance.CannonballSpeed;
        _remainingLifetime = distToTarget / CannonController.Instance.CannonballSpeed;
        _totalLifetime = _remainingLifetime;
        _lifetimeFactor = _remainingLifetime / _totalLifetime;
        _deltaFactorFromMidpoint = (1-_lifetimeFactor) * 2;
        _isActive = true;
        Update();
    }

    private void Update()
    {
        if (!_isActive) return;

        transform.position += _velocity * Time.deltaTime;
        _remainingLifetime -= Time.deltaTime;

        _lifetimeFactor = 1 - _remainingLifetime / _totalLifetime; //increases from 0 to 1;

        AdjustYAdjustWithLifetime();
        AdjustSizeWithLifetime();

        if (_remainingLifetime <= 0)
        {
            TerminateCannonball();
        }

    }

    private void AdjustYAdjustWithLifetime()
    {
        if (_lifetimeFactor < .5f)
        {
            _yAdjust.y = Mathf.Lerp(_yAdjustStart, _yAdjustApex, _lifetimeFactor);
            //JUICE TODO adjust the Lerp to not be linear. This currently looks like a sharp rise-snap-fall.
            
        }
        if (_lifetimeFactor >= .5f)
        {
            _yAdjust.y = Mathf.Lerp(_yAdjustApex, _yAdjustStart, _lifetimeFactor);
        }
        _srCannonball.transform.localPosition = _yAdjust;
    }

    private void AdjustSizeWithLifetime()
    {
        if (_lifetimeFactor < .5f)
        {
            _srCannonball.transform.localScale = Vector3.Lerp(_scale, _scale * _scaleAtApex, _lifetimeFactor);
        }
        if (_lifetimeFactor >= .5f)
        {
            _srCannonball.transform.localScale = Vector3.Lerp(_scale * _scaleAtApex, _scale, _lifetimeFactor);
        }
    }

    private void TerminateCannonball()
    {
        _isActive = false;
        _srCannonball.enabled = false;
        _srShadow.enabled = false;
        _coll.enabled = false;
        //JUICE TODO check for tile tile and emit particle splash;
        Invoke(nameof(Delay_TerminateCannonball), 2f);
    }

    private void Delay_TerminateCannonball()
    {
        CannonController.Instance.ReturnCannonball(this);
    }
}
