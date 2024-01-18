using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class ShipHandler : MonoBehaviour
{
    public Action<List<CargoLibrary.CargoType>, int> CargoChanged;

    [SerializeField] SpriteRenderer _baseShipSR = null;
    AIPath _ai;

    //settings
    int _maxSailingLevels = 3;
    int _startingSailingLevels = 1;

    int _maxCannonLevels = 2;
    int _startingCannonLevels = 0;

    int _maxCargoSlots = 3;
    int _startingCargoSlots = 1;
    

    //state
    float _timeBetweenMoves = 1f;
    float _timeForNextMove = 0;
    ActorHandler _actor;
    public ActorHandler Actor => _actor;
    [SerializeField] TileHandler _currentTile;
    TileHandler _prevTile;
    Seeker _seeker;
    Path _currentPath;
    DestinationHandler _destinationHandler;
    [SerializeField] List<CargoLibrary.CargoType> _cargoInHold;
    public List<CargoLibrary.CargoType> CargoInHold => _cargoInHold;
    bool _hasFreeCargoSpace = true;
    public bool HasFreeCargoSpace => _hasFreeCargoSpace;

    int _currentSailingLevel;
    CannonHandler _cannonHandler;
    int _currentCargoSlots;
    int _upgradeCount = 0;

    public Vector3 Velocity { get; private set; }

    private void Awake()
    {
        _currentCargoSlots = _startingCargoSlots;
        _currentSailingLevel = _startingSailingLevels;
        _cannonHandler = GetComponentInChildren<CannonHandler>();
        _cannonHandler.SetCannonLevel(_startingCannonLevels);
        _seeker = GetComponent<Seeker>();
        _ai = GetComponent<AIPath>();
        _cargoInHold = new List<CargoLibrary.CargoType>();

    }

    private void Start()
    {
        _timeForNextMove = Time.time + _timeBetweenMoves;
        //FindCurrentTile();w
        SetCargoUI();
    }

    public void SetupShip(int index, ActorHandler handler)
    {
        _baseShipSR.color = PlayerLibrary.Instance.GetPlayerColor(index);
        _actor = handler;
    }

    #region Movement

    public void SetDestination(DestinationHandler dh)
    {
        _destinationHandler = dh;
        _destinationHandler.DestinationChanged += HandleUpdatedDestination;
    }

    private void HandleUpdatedDestination(TileHandler newDest)
    {
        _ai.destination = newDest.transform.position;

    }

    private void Update()
    {
        Velocity = _ai.velocity;
    }

    #endregion


    #region Cargo
    public void RemoveOneCargo(CargoLibrary.CargoType cargoRemoved)
    {
        if (_cargoInHold.Contains(cargoRemoved))
        {
            _cargoInHold.Remove(cargoRemoved);
        }

        CheckForFreeCargoSpace();
        SetCargoUI();
    }

    public void AddOneCargo(CargoLibrary.CargoType cargoAdded)
    {
        if (!HasFreeCargoSpace) return;

        _cargoInHold.Add(cargoAdded);
        CheckForFreeCargoSpace();
        SetCargoUI();
    }

    public bool CheckCanModifyCargoCapacity()
    {
        if (_currentCargoSlots == _maxCargoSlots) return false;
        else return true;
    }

    private void ModifyCargoCapacity(int amountToAdd)
    {
        _currentCargoSlots += amountToAdd;
        _currentCargoSlots = Mathf.Clamp(_currentCargoSlots, 0, _maxCargoSlots);

        CheckForFreeCargoSpace();
        SetCargoUI();
    }

    private void SetCargoUI()
    {
        CargoChanged?.Invoke(_cargoInHold, _currentCargoSlots);
    }

    private void CheckForFreeCargoSpace()
    {
        if (_cargoInHold.Count < _currentCargoSlots)
        {
            _hasFreeCargoSpace = true;
        }
        else _hasFreeCargoSpace = false;
    }

    #endregion

    #region Upgrades

    public bool CheckIfCanInstallUpgrade(SmithLibrary.SmithType upgradeConsidered)
    {
        switch (upgradeConsidered)
        {
            case SmithLibrary.SmithType.Sails:
                if (_currentSailingLevel >= _maxSailingLevels)
                    return false;
                else return true;

            case SmithLibrary.SmithType.Cargo:
                if (_currentCargoSlots  >= _maxCargoSlots)
                    return false;
                else return true;

            case SmithLibrary.SmithType.Cannon:
                if (_cannonHandler.CannonLevel  >= _maxCannonLevels)
                    return false;
                else return true;

            default:
                return false;
        }
    }

    public bool CheckIfCanAffordUpgrade(int proposedCost)
    {
        return _actor.CheckIfCanAfford(proposedCost);
    }

    public void InstallUpgrade(SmithLibrary.SmithType upgradeInstalled)
    {
        switch (upgradeInstalled)
        {
            case SmithLibrary.SmithType.Sails:
                _currentSailingLevel++;
                _ai.maxSpeed = BalanceLibrary.Instance.GetSpeedByCount(_currentSailingLevel);
                break;

            case SmithLibrary.SmithType.Cargo:
                _currentCargoSlots++;
                SetCargoUI();
                break;

            case SmithLibrary.SmithType.Cannon:
                _cannonHandler.SetCannonLevel(_cannonHandler.CannonLevel + 1);
                break;
        }
        _upgradeCount++;
        _actor.HandleUpgrade(_upgradeCount);
    }

    #endregion



    private void FindCurrentTile()
    {
        Vector3 testPos = transform.position;
        testPos.y -= 0.2f;

        var hit_0 = Physics2D.OverlapCircle(testPos, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_0) _currentTile = hit_0.GetComponent<TileHandler>();
    }
}
