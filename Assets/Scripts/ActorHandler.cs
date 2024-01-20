using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActorHandler : MonoBehaviour
{
    public Action<int> ActorIndexUpdated;
    public Action<string> ActorNameUpdated;
    public Action<int> ActorCoinCountUpdated;
    public Action<int> ActorRankUpdated;
    public Action<List<CargoLibrary.CargoType>, int> ActorCargoSlotsUpdated;
    public Action<int> ActorCrewUpdated;

    //settings
    [SerializeField] DestinationHandler _destinationPrefab = null;
    [SerializeField] ShipHandler _shipPrefab = null;


    //Refs
    [SerializeField] DestinationHandler _destination;
    [SerializeField] ShipHandler _ship;
    public ShipHandler Ship => _ship;
    [SerializeField] ActorUIHandler _ui;

    //state
    int _actorIndex;
    public int ActorIndex => _actorIndex;
    int _totalCoins = 0;
    int _currentCoins = 0;
    CrewHandler _crewHandler;

    public void SetupNewActor(int playerIndex, ActorUIHandler ui)
    {
        _actorIndex = playerIndex;        
        _ui = ui;
        _ui.AssignActor(this, playerIndex);
    }

    public void SetupShip(int playerIndex, TileHandler startingTile)
    {
        _destination = Instantiate(_destinationPrefab, startingTile.transform.position, Quaternion.identity).
            GetComponent<DestinationHandler>();
        _ship = Instantiate(_shipPrefab, startingTile.transform.position, Quaternion.identity).
            GetComponent<ShipHandler>();

        _destination.SetPlayerIndex(playerIndex);
        _ship.SetupShip(playerIndex, this);
        _ship.SetDestination(_destination);
        _ship.CargoChanged += HandleCargoChanged;

        _crewHandler = _ship.GetComponent<CrewHandler>();
        _crewHandler.CrewCountChanged += HandleCrewChanged;
        _crewHandler.CrewCountAtZero += HandleCrewAtZero;
    }

    public void NullifyActorAtGameEnd()
    {
        Ship.NullifyShip();
        _destination.gameObject.SetActive(false);
    }

    private void HandleCrewChanged(int obj)
    {
        ActorCrewUpdated?.Invoke(obj);
    }

    private void HandleCrewAtZero()
    {
        ActorController.Instance.DispatchDinghy(_actorIndex);
        Debug.Log("Crew at zero!");
    }

    private void OnDestroy()
    {
        _ship.CargoChanged -= HandleCargoChanged;
    }

    private void HandleCargoChanged(List<CargoLibrary.CargoType> arg1, int arg2)
    {
        ActorCargoSlotsUpdated?.Invoke(arg1, arg2);
    }

    private void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        int moveDir = ConvertMoveVec2IntoInt(move);
        if (GameController.Instance.Context == UIController.Context.Gameplay)
        {
            _destination.CommandMove(moveDir);
        }
        else if (GameController.Instance.Context == UIController.Context.Title)
        {
            if (moveDir == 0)
            {
                GameController.Instance.RequestNewGameContext(UIController.Context.Pretitle);
            }
            else if (moveDir == 2)
            {
                GameController.Instance.RequestNewGameContext(UIController.Context.Wheel);
            }

        }
        else if (GameController.Instance.Context == UIController.Context.Wheel)
        {
            if (moveDir == 0)
            {
                GameController.Instance.RequestNewGameContext(UIController.Context.Title);
            }
            else if (moveDir == 2)
            {
                GameController.Instance.EnterCurrentWheelOption();
            }
            else if (moveDir == 1)
            {
                GameController.Instance.RequestWheelRotation(1);
            }
            else if (moveDir == 3)
            {
                GameController.Instance.RequestWheelRotation(-1);
            }


        }
        else if (GameController.Instance.Context == UIController.Context.GameOver)
        {

        }


    }

    /// <summary>
    /// 0: North, 1: East, 2: South, 3: West
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
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

    public void ModifyCoins(int coinsToAdd)
    {
        _currentCoins += coinsToAdd;
        if (coinsToAdd > 0)
        {
            //JUICE TODO play a chaching sound
            _totalCoins += coinsToAdd;
            GameController.Instance.HandleCoinsGained(_actorIndex, _currentCoins);
        }
        Mathf.Clamp(_currentCoins, 0, 99);

        ActorCoinCountUpdated?.Invoke(_currentCoins);

    }

    public bool CheckIfCanAfford(int coinBill)
    {
        if (coinBill <= _currentCoins) return true;
        else return false;
    }

    public void HandleUpgrade(int upgradesMade)
    {
        ActorRankUpdated?.Invoke(upgradesMade);
    }
}
