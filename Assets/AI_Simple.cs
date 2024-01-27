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


    //state
    CityHandler _cityOfInterest;
    [SerializeField] bool _isTryingToMove = false;
    float _timeToDropBreadcrump_0;
    float _timeToDropBreadcrump_1;
    TileHandler _destinationTile;
    [SerializeField] TileHandler _currentTile;
    [SerializeField] TileHandler _prevTile_0;
    TileHandler _prevTile_1;
    [SerializeField] bool _isBackingUp = false;
    bool _isBackingUpAgain = false;

    private void Start()
    {
        GameController.Instance.GameModeStarted += HandleGameStart;

    }
    internal void AttachAIToShip()
    {
        _ah = GetComponent<ActorHandler>();
        _ah.ActorCargoSold += HandleCargoSold;
        _ah.ActorCargoLoaded += HandleCargoLoaded;
        _dh = _ah.Destination;
        _ship = _ah.Ship;
        _ship.ShipCollided += HandleShipCollided;
        HandleGameStart();

        _timeToDropBreadcrump_0 = 0;
        _timeToDropBreadcrump_1 = _timeBetweenBreadcrumbs / 2f;
    }

    private void OnDestroy()
    {
        GameController.Instance.GameModeStarted -= HandleGameStart;
    }

    //private void Update()
    //{
    //    if (_isBackingUp)
    //    {
    //        if (FindCurrentTileOfShip() == _prevTile_0)
    //        {
    //            _isBackingUp = false;
    //            _dh.JumpToTile(_destinationTile);
    //        }
    //    }
    //    else
    //    {
    //        if (Time.time > _timeToDropBreadcrump_0)
    //        {
    //            DropBreadCrumb_0();
    //            _timeToDropBreadcrump_0 = Time.time + _timeBetweenBreadcrumbs;
    //        }

    //        //if (Time.time > _timeToDropBreadcrump_1)
    //        //{
    //        //    DropBreadCrumb_1();
    //        //    _timeToDropBreadcrump_0 = Time.time + _timeBetweenBreadcrumbs;
    //        //}
    //    }


    //}

    private void DropBreadCrumb_0()
    {
        _prevTile_0 = FindCurrentTileOfShip();
    }

    private void DropBreadCrumb_1()
    {
        _prevTile_1 = FindCurrentTileOfShip();
    }

    private void HandleGameStart()
    {
        FindSetBestLoadCity();
    }

    private TileHandler FindCurrentTileOfShip()
    {
        Vector3 testPos = _ship.transform.position;
        testPos.y -= 0;

        var hit_0 = Physics2D.OverlapCircle(testPos, 0.01f, Layers.LayerMask_AllTiles);
        if (hit_0)
        {
            _currentTile = hit_0.GetComponent<TileHandler>();
            return _currentTile;
        }
        else return null;

    }

    private void HandleCargoSold()
    {
        if (_cityOfInterest.CheckIfCityWantsAnyCargo(_ship.CargoInHold))
        {
            //do nothing while remaining cargo unloads
            _isTryingToMove = false;
        }
        else
        {

            FindSetBestLoadCity();
        }

    }

    private void HandleCargoLoaded()
    {
        if (_cityOfInterest.ProductionInStock > 0 && _ship.HasFreeCargoSpace)
        {
            //do nothing and wait for load to complete
            _isTryingToMove = false;
        }
        else
        {
            //find a place to sell
            FindSetBestSellCity();
        }
    }

    private void FindSetBestLoadCity()
    {
        _cityOfInterest = CityController.Instance.FindBestCityToLoadFrom(_ship.transform.position);
        _destinationTile = _cityOfInterest.Pier.Tile;
        _dh.JumpToTile(_destinationTile);
        _isTryingToMove = true;
    }

    private void FindSetBestSellCity()
    {
        _cityOfInterest = CityController.Instance.FindBestCityToSellAt(_ship.transform.position,
            _ship.CargoInHold);
        _destinationTile = _cityOfInterest.Pier.Tile;
        _dh.JumpToTile(_destinationTile);
        _isTryingToMove = true;
    }

    private void HandleShipCollided()
    {
        if (FindCurrentTileOfShip() == _destinationTile)
        {
            //do nothing, already at desired destination
        }
        else
        {
            _dh.JumpToTile(_prevTile_0);
            _isBackingUp = true;
        }
    }
}
