using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Pirate : MonoBehaviour
{
    DestinationHandler _dh;
    ActorHandler _ah;
    ShipHandler _sh;

    //settings
    [SerializeField] float _timeBetweenUpdates = 3f;

    //state
    float _timeForNextUpdate = 0;

    internal void AttachAIToShip()
    {
        _ah = GetComponent<ActorHandler>();
        _dh = _ah.Destination;
        
        _sh = _ah.Ship;
    }

    private void Update()
    {
        if (Time.time >= _timeForNextUpdate)
        {
            UpdateDestination();
            _timeForNextUpdate = Time.time + _timeBetweenUpdates;
        }
    }

    private void UpdateDestination()
    {
        _dh.JumpToTile(ActorController.Instance.FindWealthiestActor());
    }

    
}
