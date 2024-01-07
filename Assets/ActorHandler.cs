using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActorHandler : MonoBehaviour
{
    //settings
    [SerializeField] DestinationHandler _destinationPrefab = null;
    [SerializeField] ShipHandler _shipPrefab = null;


    //state
    [SerializeField] DestinationHandler _destination;
    [SerializeField] ShipHandler _ship;
    [SerializeField] ActorUIHandler _ui;

    public void SetupNewActor(int playerIndex, TileHandler startingTile, ActorUIHandler ui)
    {
        _destination = Instantiate(_destinationPrefab, startingTile.transform.position, Quaternion.identity).
            GetComponent<DestinationHandler>();
        _ship = Instantiate(_shipPrefab, startingTile.transform.position, Quaternion.identity).
            GetComponent<ShipHandler>();
        _ui = ui;

        _destination.SetPlayerIndex(playerIndex);
        _ship.SetPlayerIndex(playerIndex);
        _ship.SetDestination(_destination);
    }

    private void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        
        int moveDir = ConvertMoveVec2IntoInt(move);
        _destination.CommandMove(moveDir);

    }

    private int ConvertMoveVec2IntoInt(Vector2 move)
    {
        if (move.y > 0)
        {
            return 0;
        }
        else if (move.x > 0)
        {
            return 1;
        }
        else if (move.y < 0)
        {
            return 2;
        }
        else if (move.x < 0)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }
}
