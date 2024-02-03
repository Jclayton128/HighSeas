using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActorController : MonoBehaviour
{
    public static ActorController Instance { get; private set; }

    //settings
    [SerializeField] TileHandler[] _startingTiles = null;
    [SerializeField] ActorUIHandler[] _uiHandlers = null;
    [SerializeField] DInghyHandler _dingyPrefab = null;
    [SerializeField] ActorHandler _aiPrefab = null;
    [SerializeField] ActorHandler _piratePrefab = null;


    [SerializeField] int _maxPlayers = 4;
    public int MaxPlayers => _maxPlayers;

    //state
    List<ActorHandler> _activeActors = new List<ActorHandler>();
    List<ActorHandler> _activePirates = new List<ActorHandler>();

    List<DInghyHandler> _activeDinghys = new List<DInghyHandler>();



    public void RegisterNewPlayerAsActor(PlayerInput newPlayer)
    {
        ActorHandler ah;
        if (newPlayer)
        {
           ah = newPlayer.GetComponent<ActorHandler>();
        }
        else
        {
            ah = Instantiate(_aiPrefab);
        }

        _activeActors.Add(ah);
        int playerIndex = _activeActors.IndexOf(ah);
        //Debug.Log($"Added Player {playerIndex}");
        bool isPlayer;

        if (newPlayer) isPlayer = true;
        else isPlayer = false;

        ah.SetupNewActor(playerIndex, _uiHandlers[playerIndex], isPlayer);
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeStarted += HandleGameplayStarted;
        GameController.Instance.GameModeEnded += HandleGameplayEnded;
    }

    private void HandleGameplayStarted()
    {
        while (_activeActors.Count <_maxPlayers)
        {
            //Debug.Log("player added");
            RegisterNewPlayerAsActor(null);
        }

        for (int i = 0; i< _activeActors.Count; i++)
        {
            _activeActors[i].SetupShip(i, _startingTiles[i]);
        }
    }

    private void HandleGameplayEnded()
    {
        for (int i = 0; i < _activeActors.Count; i++)
        {
            _activeActors[i].NullifyActorAtGameEnd();
        }
    }

    public void DeregisterPlayerAsActor(PlayerInput playerToRemove)
    {
        if (_activeActors.Contains(playerToRemove.GetComponent<ActorHandler>()))
            _activeActors.Remove(playerToRemove.GetComponent<ActorHandler>());
    }



    public void Debug_GiveAllPlayers10Coins()
    {
        foreach (var actor in _activeActors)
        {
            actor.ModifyCoins(10);
        }
    }

    internal void Debug_DevelopDevShip()
    {
        _activeActors[0].Ship.InstallUpgrade(SmithLibrary.SmithType.Sails);
        _activeActors[0].Ship.InstallUpgrade(SmithLibrary.SmithType.Sails);
        _activeActors[0].Ship.InstallUpgrade(SmithLibrary.SmithType.Cannon);
        _activeActors[0].Ship.InstallUpgrade(SmithLibrary.SmithType.Cannon);
        _activeActors[0].Ship.InstallUpgrade(SmithLibrary.SmithType.Cargo);
        _activeActors[0].Ship.InstallUpgrade(SmithLibrary.SmithType.Cargo);
    }

    public void DispatchDinghy(int actorIndex)
    {
        DInghyHandler newDinghy = Instantiate(
            _dingyPrefab, _startingTiles[actorIndex].transform.position, Quaternion.identity);
        newDinghy.InitializeDinghy(_activeActors[actorIndex].Ship);

        _activeDinghys.Add(newDinghy);
    }

    public void DeregisterDinghy(DInghyHandler completedDinghy)
    {
        _activeDinghys.Remove(completedDinghy);
    }

    public Transform GetShipTransformOfActor(int actorIndex)
    {
        return _activeActors[actorIndex].Ship.transform;
    }

    public ActorHandler GetFirstPlayer()
    {
        return _activeActors[0];
    }

    public void SpawnPirate()
    {

        ActorHandler ah = Instantiate(_piratePrefab);
        _activePirates.Add(ah);
        int pirateIndex = _activePirates.Count - 1;
        ah.SetupNewPirate(pirateIndex, _startingTiles[pirateIndex + _maxPlayers]);
        SoundController.Instance.PlayClip(SoundLibrary.SoundID.PirateArrival10);
    }

    public TileHandler FindWealthiestActor()
    {
        ActorHandler ah = null;
        int coinToBeat = 0;
        foreach (var actor in _activeActors)
        {
            if (actor.Coins >= coinToBeat)
            {
                ah = actor;
                coinToBeat = ah.Coins;
            }
        }
        return ah.Ship.Tile;
    }
}


