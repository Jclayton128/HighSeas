using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Simple : MonoBehaviour
{
    [SerializeField] ActorHandler _ah;
    [SerializeField] ShipHandler _ship;
    [SerializeField] DestinationHandler _dh;
    float _timeBetweenBreadcrumbs = 1f;
    [SerializeField] float _staticTimeThreshold = 6f;

    //state
    CityHandler _cityOfInterest;
    TileHandler _destinationTile;
    [SerializeField] TileHandler _currentTile;

    [SerializeField] bool _isBackingUp = false;
    bool _isBackingUpAgain = false;
    float _timeForStaticThresholdTest;

    private void Start()
    {
        GameController.Instance.GameModeStarted += HandleGameStart;
        _timeForStaticThresholdTest = float.MaxValue;
    }
    internal void AttachAIToShip()
    {
        _ah = GetComponent<ActorHandler>();
        _ah.ActorCargoSold += HandleCargoSold;
        _ah.ActorCargoLoaded += HandleCargoLoaded;
        _dh = _ah.Destination;
        _ship = _ah.Ship;
        HandleGameStart();

    }

    private void OnDestroy()
    {
        GameController.Instance.GameModeStarted -= HandleGameStart;
    }

  
    private void HandleGameStart()
    {
        FindSetBestLoadCity();
    }


    private void Update()
    {
        if (Time.time > _timeForStaticThresholdTest)
        {
            _timeForStaticThresholdTest = Time.time + _staticTimeThreshold;
            StaticRethink();
            
        }
    }

    private void HandleCargoSold()
    {
        if (_cityOfInterest.CheckIfCityWantsAnyCargo(_ship.CargoInHold))
        {
            //do nothing while remaining cargo unloads
            _timeForStaticThresholdTest = Time.time + _staticTimeThreshold;
        }
        else
        {
            _timeForStaticThresholdTest = float.MaxValue;
            FindSetBestLoadCity();
        }

    }

    private void HandleCargoLoaded()
    {
        if (_cityOfInterest.ProductionInStock > 0 && _ship.HasFreeCargoSpace)
        {
            //do nothing and wait for load to complete
            _timeForStaticThresholdTest = Time.time + _staticTimeThreshold;
        }
        else
        {
            //find a place to sell
            _timeForStaticThresholdTest = float.MaxValue;
            FindSetBestSellCity();
        }
    }

    private void FindSetBestLoadCity()
    {
        _cityOfInterest = CityController.Instance.FindBestCityToLoadFrom(_ship.transform.position);
        _destinationTile = _cityOfInterest.Pier.Tile;
        _dh.JumpToTile(_destinationTile);
    }

    private void FindSetBestSellCity()
    {
        _cityOfInterest = CityController.Instance.FindBestCityToSellAt(_ship.transform.position,
            _ship.CargoInHold);
        _destinationTile = _cityOfInterest.Pier.Tile;
        _dh.JumpToTile(_destinationTile);
    }

    private void StaticRethink()
    {
        Debug.Log("Static Rethink!");
        if (_ship.CargoInHold.Count > 0)
        {
            FindSetBestSellCity();
        }
        else
        {
            FindSetBestLoadCity();
        }
    }
}
