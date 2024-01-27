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
    [SerializeField] int _maxPlayers = 2;

    //state
    [SerializeField] List<ActorHandler> _activeActors = new List<ActorHandler>();
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
            Debug.Log("player added");
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
}


