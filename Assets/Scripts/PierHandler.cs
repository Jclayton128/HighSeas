using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PierHandler : MonoBehaviour
{
    [SerializeField] CityHandler _attachedCity;
    [SerializeField] SmithHandler _attachedSmith;

    TileHandler _tileHandler;

    //state
    [SerializeField] ShipHandler _currentShip;
    public TileHandler Tile => _tileHandler;
    [SerializeField] List<ShipHandler> _shipWaitingList = new List<ShipHandler>();
    
    private void Start()
    {
        Invoke(nameof(Delay_InitializePier), 0.01f);
    }

    public void Delay_InitializePier()
    {
        _tileHandler = GetComponent<TileHandler>();
        FindAttachedEntity();
    }

    private void FindAttachedEntity()
    {        
        foreach (var tile in _tileHandler.Neighbors)
        {
            if (tile.City)
            {
                _attachedCity = tile.City;
                _attachedCity.CargoProduced += HandleCargoProduced;
                _attachedCity.SetPier(this);
                break;
            }
            if (tile.Smith)
            {
                _attachedSmith = tile.Smith;
                _attachedSmith.SetPier(this);
            }
        }
    }

    private void HandleCargoProduced()
    {
        if (_attachedCity && _currentShip)
        {
            CheckAndInitiateShipTransaction();
            //SoundController.Instance.PlayClip(SoundLibrary.SoundID.EnterPort2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipHandler ship;
        if (collision.TryGetComponent<ShipHandler>(out ship))
        {
            if (_attachedCity)
            {
                SoundController.Instance.PlayClip(SoundLibrary.SoundID.EnterPort2);
            }

            if (_currentShip != null)
            {
                _shipWaitingList.Add(ship);
            }
            else
            {
                _currentShip = ship;
                if (_attachedCity) 
                {
                    CheckAndInitiateShipTransaction();
                }
                if (_attachedSmith)
                {
                    CheckAndInitiateShipUpgrade(
                        _attachedSmith.SmithType, _attachedSmith.CurrentUpgradeCost);
                }
            }
        }
    }


    private void CheckAndInitiateShipTransaction()
    {
        //Debug.Log("attempting transacion");
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


    private void CheckAndInitiateShipUpgrade(SmithLibrary.SmithType proposedUpgrade, int proposedCost)
    {
        if (_currentShip.CheckIfCanInstallUpgrade(proposedUpgrade) &&
            _currentShip.CheckIfCanAffordUpgrade(proposedCost))
        {
            SoundController.Instance.PlayClip(SoundLibrary.SoundID.StartUpgrade5);
            _attachedSmith.StartUpgrade();
            _attachedSmith.UpgradeCompleted += HandleUpgradeCompleted;
        }
        else if (!_currentShip.CheckIfCanInstallUpgrade(proposedUpgrade))
        {
            Debug.Log("Can't install the upgrade");
        }
        else if (!_currentShip.CheckIfCanAffordUpgrade(proposedCost))
        {
            Debug.Log("Can't afford the upgrade");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShipHandler ship;
        if (collision.TryGetComponent<ShipHandler>(out ship))
        {
            if (ship == _currentShip)
            {
                if (_attachedCity)
                {
                    _attachedCity.CancelOnload();
                    _attachedCity.CancelOffLoad();
                    _attachedCity.CargoOffloadCompleted -= HandleCompletedOffload;
                }
                else if (_attachedSmith)
                {
                    _attachedSmith.CancelUpgrade();
                    _attachedSmith.UpgradeCompleted -= HandleUpgradeCompleted;
                }

                if (_shipWaitingList.Count > 0)
                {
                    _currentShip = _shipWaitingList[0];
                    _shipWaitingList.Remove(_currentShip);

                    if (_attachedCity)
                    {
                        CheckAndInitiateShipTransaction();
                    }
                    if (_attachedSmith)
                    {
                        CheckAndInitiateShipUpgrade(
                            _attachedSmith.SmithType, _attachedSmith.CurrentUpgradeCost);
                    }

                }
                else
                {
                    _currentShip = null;
                }

            }
            else
            {
                _shipWaitingList.Remove(ship);
            }
        }
    }


    private void HandleUpgradeCompleted(SmithLibrary.SmithType upgradeCompleted)
    {
        _currentShip.Actor.ModifyCoins(-_attachedSmith.CurrentUpgradeCost);
        _currentShip.InstallUpgrade(upgradeCompleted);
        _attachedSmith.UpgradeCompleted -= HandleUpgradeCompleted;
        SoundController.Instance.PlayClip(SoundLibrary.SoundID.FinishUpgrade6);
    }

    public void InitiateCargoOffload(CargoLibrary.CargoType cargoToSell)
    {
        _attachedCity.StartOffLoad(cargoToSell);
        _attachedCity.CargoOffloadCompleted += HandleCompletedOffload;
    }

    private void HandleCompletedOffload(CargoLibrary.CargoType cargoSold, int profit)
    {
        _currentShip.Actor.ModifyCoins(profit);
        _currentShip.EmitCoins(profit);
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
        SoundController.Instance.PlayClip(SoundLibrary.SoundID.LoadCargo3, (int)obj);
        _attachedCity.CargoOnloadCompleted -= HandleCompletedOnload;        
        CheckAndInitiateShipTransaction();
    }
}
