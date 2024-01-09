using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputGameControllerLinkage : MonoBehaviour
{
    public void OnPlayerJoined (PlayerInput newPlayer)
    {
        ActorController.Instance.RegisterNewPlayerAsActor(newPlayer);
    }

    public void OnPlayerLeft (PlayerInput exitingPlayer)
    {
        ActorController.Instance.DeregisterPlayerAsActor(exitingPlayer);
    }
}
