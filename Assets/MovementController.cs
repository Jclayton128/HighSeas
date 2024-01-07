using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class MovementController : MonoBehaviour
{
    public static MovementController Instance { get; private set; }

    //state
    AstarPath _asp;

    private void Awake()
    {
        Instance = this;
        _asp = GetComponent<AstarPath>();
    }

    public void HandleMovement()
    {
        _asp.Scan();
    }
}
