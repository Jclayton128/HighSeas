using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class ShipHandler : MonoBehaviour
{
    public Action<List<CargoLibrary.CargoType>, int> CargoChanged;
    public Action ShipCollided;

    [SerializeField] SpriteRenderer _baseShipSR = null;
    [SerializeField] SpriteRenderer[] _sailSRs = null;
    [SerializeField] SpriteRenderer[] _cargoSlots = null;
    AIPath _ai;

    //settings
    int _maxSailingLevels = 2;
    int _startingSailingLevels = 0;

    int _maxCannonLevels = 2;
    int _startingCannonLevels = 0;

    int _maxCargoSlots = 3;
    int _startingCargoSlots = 1;
    [SerializeField] ParticleSystem _coinPS = null;

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
    CrewHandler _crewHandler;
    public Vector3 Velocity { get; private set; }
    bool _hasCrew = true;

    private void Awake()
    {
        _currentCargoSlots = _startingCargoSlots;
        _currentSailingLevel = _startingSailingLevels;
        _cannonHandler = GetComponentInChildren<CannonHandler>();
        _cannonHandler.SetCannonLevel(_startingCannonLevels);
        _seeker = GetComponent<Seeker>();
        _ai = GetComponent<AIPath>();
        _cargoInHold = new List<CargoLibrary.CargoType>();
        _crewHandler = GetComponent<CrewHandler>();
        _crewHandler.CrewCountAtZero += HandleZeroCrew;
        _crewHandler.CrewCountChanged += HandleCrewReturned;
    }


    private void OnDestroy()
    {
        _crewHandler.CrewCountAtZero -= HandleZeroCrew;
    }

    private void HandleZeroCrew()
    {
        _hasCrew = false;
        _ai.maxSpeed = 0;
        gameObject.tag = "Dinghy";
        gameObject.layer = 10;
    }

    private void HandleCrewReturned(int count)
    {
        if (_hasCrew) return;
        _hasCrew = true;
        _ai.maxSpeed = BalanceLibrary.Instance.GetSpeedByCount(_currentSailingLevel);
        gameObject.tag = "Targetable";
        gameObject.layer = 8;
    }

    private void Start()
    {
        _timeForNextMove = Time.time + _timeBetweenMoves;
        //FindCurrentTile();w
        SetCargoUI();
    }

    public void SetupShip(int index, ActorHandler handler)
    {
        _actor = handler;
        RenderShip();
    }

    private void RenderShip()
    {
        _baseShipSR.sprite = PlayerLibrary.Instance.GetShipBaseSpriteByCannon(_cannonHandler.CannonLevel);

        foreach (var sr in _sailSRs) sr.sprite = null;

        for (int i = 0; i <= _currentSailingLevel; i++)
        {
            _sailSRs[i].sprite = PlayerLibrary.Instance.GetSailSprite(_actor.ActorIndex, i);
        }

        foreach (var cargoSlot in _cargoSlots) cargoSlot.sprite = null;

        for (int i = 0; i < _cargoInHold.Count; i++)
        {
            _cargoSlots[i].sprite = CargoLibrary.Instance.GetCargoSprite(_cargoInHold[i]);
        }

    }

    public void NullifyShip()
    {
        _ai.maxSpeed = 0;
        _cannonHandler.NullifyCannon();
        //_ai.maxSpeed = BalanceLibrary.Instance.GetSpeedByCount(_currentSailingLevel);
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

    public void EmitCoins(int numberToEmit)
    {
        _coinPS.Emit(numberToEmit);
    }

    public void RemoveOneCargo(CargoLibrary.CargoType cargoRemoved)
    {
        if (_cargoInHold.Contains(cargoRemoved))
        {
            _cargoInHold.Remove(cargoRemoved);
        }

        CheckForFreeCargoSpace();
        SetCargoUI();
        RenderShip();
        
        _actor.HandleCargoSold();
    }

    public void AddOneCargo(CargoLibrary.CargoType cargoAdded)
    {
        if (!HasFreeCargoSpace) return;

        _cargoInHold.Add(cargoAdded);
        CheckForFreeCargoSpace();
        SetCargoUI();
        //_crewHandler.GainOneCrew();
        RenderShip();
        _actor.HandleCargoLoaded();
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
                RenderShip();
                break;

            case SmithLibrary.SmithType.Cargo:
                _currentCargoSlots++;
                SetCargoUI();
                break;

            case SmithLibrary.SmithType.Cannon:
                _cannonHandler.SetCannonLevel(_cannonHandler.CannonLevel + 1);
                RenderShip();
                break;
        }
        _upgradeCount++;
        _actor.HandleUpgrade(_upgradeCount);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            ShipCollided?.Invoke();
        }
    }                                                                                   


    private void FindCurrentTile()
    {
        Vector3 testPos = transform.position;
        testPos.y -= 0.2f;

        var hit_0 = Physics2D.OverlapCircle(testPos, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_0) _currentTile = hit_0.GetComponent<TileHandler>();
    }
}
