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
    
    //state
    [SerializeField] List<ActorHandler> _activePlayersAsActors = new List<ActorHandler>();

    public void RegisterNewPlayerAsActor(PlayerInput newPlayer)
    {
        ActorHandler ah = newPlayer.GetComponent<ActorHandler>();
        _activePlayersAsActors.Add(ah);
        int playerIndex = _activePlayersAsActors.IndexOf(ah);
        //Debug.Log($"Added Player {playerIndex}");
        ah.SetupNewActor(playerIndex, _startingTiles[playerIndex], _uiHandlers[playerIndex]);

    }

    public void DeregisterPlayerAsActor(PlayerInput playerToRemove)
    {
        if (_activePlayersAsActors.Contains(playerToRemove.GetComponent<ActorHandler>()))
            _activePlayersAsActors.Remove(playerToRemove.GetComponent<ActorHandler>());
    }

    private void Awake()
    {
        Instance = this;
    }
}
