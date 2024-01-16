using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonHandler : MonoBehaviour
{
    [SerializeField] TargetHandler _targetMarkerPrefab = null;

    //Settings
    [SerializeField] float _scanRange = 4f;
    [SerializeField] float _timeBetweenScans = 0.5f;
    Vector3 _targetMarkerHoldingPosition = Vector3.one * 1000;
    [SerializeField] float _timeBetweenShots = 3f;
    [SerializeField] float _shotSpeed = 2f;
    [SerializeField] float _maxTargetError = 0.5f;
    [SerializeField] float _errorReductionPerShot = 0.1f;


    //state
    [SerializeField] int _cannonLevel = 0;
    ShipHandler _target;
    TargetHandler _targetMarker;
    float _timeForNextScan = 0;
    ShipHandler _ship;
    float _timeForNextShot = 0;
    float _currentTargetError;
    int _shotsInStreak = 0;

    

    private void Start()
    {
        _targetMarker = Instantiate(
            _targetMarkerPrefab, _targetMarkerHoldingPosition, Quaternion.identity);
        _ship = GetComponentInParent<ShipHandler>();
        if (_ship)
        {
            _targetMarker.SetPlayerIndex(_ship.Actor.ActorIndex);
        }       

    }

    private void Update()
    {
        if (_cannonLevel == 0) return;
        else if (Time.time  >= _timeForNextScan)
        {

            Scan();
            _timeForNextScan = Time.time + _timeBetweenScans;
        }      


        if (_target)
        {
            _targetMarker.transform.position = _target.transform.position;
            if (Time.time >= _timeForNextShot)
            {
                Shoot();
                _timeForNextShot = Time.time + _timeBetweenShots;
            }
        }
        else
        {
            _shotsInStreak = 0;
            _targetMarker.transform.position = _targetMarkerHoldingPosition;
        }  
    }

    private void Shoot()
    {
        Debug.Log("kaboom!");
        var shot = CannonController.Instance.RequisitionCannonball();

        Vector3 error = UnityEngine.Random.insideUnitCircle.normalized * _currentTargetError;
        float dist = (transform.position - _target.transform.position).magnitude;
        Vector3 lead = _target.Velocity * (dist / CannonController.Instance.CannonballSpeed);


        shot.SetupCannonballInstance(transform.position,
            _target.transform.position + error + lead);

        Debug.DrawLine(
            transform.position,
            _target.transform.position + error + lead,
            Color.red, 1f);

        _shotsInStreak++;
        _currentTargetError = _maxTargetError - (_errorReductionPerShot * _shotsInStreak);
        _currentTargetError = Mathf.Clamp(_currentTargetError, 0, 99);
    }

    private void Scan()
    {
        var go = CUR.FindNearestGameObjectWithTag(
            transform, "Targetable", _scanRange, transform.root.gameObject);

        if (go)
        {
            _target = go.GetComponent<ShipHandler>();
            _targetMarker.transform.position = _target.transform.position;
        }
        else
        {
            _target = null;
        }

    }
}
