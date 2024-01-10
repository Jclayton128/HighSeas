using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PierHandler : MonoBehaviour
{
    [SerializeField] CityHandler _attachedCity;
    TileHandler _tileHandler;

    //state
    [SerializeField] ShipHandler _currentShip;

    private void Start()
    {
        Invoke(nameof(Delay_InitializePier), 0.01f);
    }

    public void Delay_InitializePier()
    {
        _tileHandler = GetComponent<TileHandler>();
        FindAttachedCity();
    }

    private void FindAttachedCity()
    {        
        foreach (var tile in _tileHandler.Neighbors)
        {
            if (tile.City)
            {
                _attachedCity = tile.City;
                break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipHandler ship;
        if (collision.TryGetComponent<ShipHandler>(out ship))
        {
            if (_currentShip != null)
            {
                
            }
            else
            {
                _currentShip = ship;
                CheckAndInitiateShipTransaction();
            }
        }
    }

    private void CheckAndInitiateShipTransaction()
    {
        //Initiate Cargo Sale of... most demanded cargo? cargo in first index?
        var options = _currentShip.CargoInHold;

        foreach (var option in options)
        {
            if (_attachedCity.CheckIfCityWantsCargo(option))
            {
                //initiate sell;
                InitiateCargoOffload(option);
                return;
            }
        }

        //okay, if the ship has nothing to sell, it must be ready to onload
        if (_currentShip.HasFreeCargoSpace && _attachedCity.ProductionInStock > 0)
        {
            //initiate onload
            InitiateCargoOnload();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShipHandler ship;
        if (collision.TryGetComponent<ShipHandler>(out ship))
        {
            if (ship == _currentShip)
            {

                _attachedCity.CancelOnload();
                _attachedCity.CancelOffLoad();
                _currentShip = null;
            }
        }
    }


    public void InitiateCargoOffload(CargoLibrary.CargoType cargoToSell)
    {
        _attachedCity.StartOffLoad(cargoToSell);
        _attachedCity.CargoOffloadCompleted += HandleCompletedOffload;
    }

    private void HandleCompletedOffload(CargoLibrary.CargoType cargoSold, int profit)
    {
        Debug.Log($"chaching! Gained {profit} coins!");
        _currentShip.RemoveOneCargo(cargoSold);
        //TODO hook into player corner UI;
        _attachedCity.CargoOffloadCompleted -= HandleCompletedOffload;
        CheckAndInitiateShipTransaction();
    }

    public void InitiateCargoOnload()
    {
        _attachedCity.StartOnload();
        _attachedCity.CargoOnloadCompleted += HandleCompletedOnload;
    }

    private void HandleCompletedOnload(CargoLibrary.CargoType obj)
    {
        _currentShip.AddOneCargo(obj);
        _attachedCity.CargoOnloadCompleted -= HandleCompletedOnload;
        Debug.Log($"loaded a " + obj);
        //TODO hook into player corner UI;

        CheckAndInitiateShipTransaction();
    }
}
