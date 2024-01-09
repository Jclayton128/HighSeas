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
                InitiateCargoSale(option);
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
                _currentShip = null;
                _attachedCity.CancelOnload();
            }
        }
    }


    public void InitiateCargoSale(CargoLibrary.CargoType cargoToSell)
    {
       int profit = _attachedCity.SatisfyDemandByOneCargo(cargoToSell);
        Debug.Log($"chaching! Gained {profit} coins!");
        _currentShip.RemoveOneCargo(cargoToSell);
        //TODO hook into player corner UI;
        CheckAndInitiateShipTransaction();
    }

    public void InitiateCargoOnload()
    {
        _attachedCity.StartOnload();
        _attachedCity.CargoLoadCompleted += HandleCompletedOnload;
    }

    private void HandleCompletedOnload(CargoLibrary.CargoType obj)
    {
        _currentShip.AddOneCargo(obj);
        _attachedCity.CargoLoadCompleted -= HandleCompletedOnload;
        Debug.Log($"loaded a " + obj);
        //TODO hook into player corner UI;

        CheckAndInitiateShipTransaction();
    }
}
