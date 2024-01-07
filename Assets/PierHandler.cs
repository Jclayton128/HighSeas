using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                //Debug.Log("Pier isn't big enough for the two of us.");
            }
            else
            {
                //Debug.Log("new ship in port!");
                _currentShip = ship;
            }
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
            }
        }
    }

    public void ProcessOffload()
    {

    }

    public void ProcessOnload()
    {

    }
}
