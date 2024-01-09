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

    private void Start()
    {
        _asp.Scan();
    }

    public void HandleMovement(Vector3 positionToBeUntraversable)
    {
        Bounds bounds = new Bounds(positionToBeUntraversable, Vector3.one*2);
        _asp.UpdateGraphs(bounds);
        
    }
}
