using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class ShipHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _baseShipSR = null;
    AIPath _ai;

    //state
    float _timeBetweenMoves = 1f;
    float _timeForNextMove = 0;
    [SerializeField] TileHandler _currentTile;
    TileHandler _prevTile;
    Seeker _seeker;
    Path _currentPath;
    DestinationHandler _destinationHandler;


    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _ai = GetComponent<AIPath>();
    }

    private void Start()
    {
        _timeForNextMove = Time.time + _timeBetweenMoves;
        //FindCurrentTile();w
    }

    public void SetPlayerIndex(int index)
    {
        _baseShipSR.color = PlayerLibrary.Instance.GetPlayerColor(index);   
    }

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
        //_prevTile = _currentTile;
        //FindCurrentTile();
        //if (_currentTile != _prevTile) MovementController.Instance.
        //        HandleMovement(transform.position);

        //if (Time.time >= _timeForNextMove)
        //{
        //    CalculateDesiredMove();
        //    _timeForNextMove = Time.time + _timeBetweenMoves;
        //}
    }

    private void CalculateDesiredMove()
    {
        //Generate Path to destination, avoiding land and other ships (impassable)
        //due to the graph already marking those as impassable terrain.

        if (_destinationHandler.CurrenTile == _currentTile) return;


        _currentPath =
            _seeker.StartPath(transform.position, _destinationHandler.transform.position,
            ExecuteMovementAttempt);
    }

    private void ExecuteMovementAttempt(Path p)
    {
        ////if (p.error)
        ////{
        ////    Debug.Log(p.errorLog);
        ////}

        //_currentPath = p;

        ////Calculate int direction of next move on the Path
        //Vector3 dir = (_currentPath.vectorPath[1] - transform.position).normalized;


        //int dirInt = FindIntDirectionFromNextStepVector(dir);
        //Debug.Log($"{dir} leads to {dirInt}");
        ////Debug.Log("next move should be " + dir);

        ////Check if movement in desired direction is possible.
        //if (!_currentTile) FindCurrentTile();
        //var proposedDestination = _currentTile.GetNeighboringTile_Traversable(dirInt);
        //if (proposedDestination == null) return;

        ////Execute Movement
        //_currentTile = proposedDestination;
        //transform.position = _currentTile.transform.position;
        //MovementController.Instance.HandleMovement();
        
    }

    private int FindIntDirectionFromNextStepVector(Vector3 vector3)
    {
        if (vector3.y > 0) return 0;
        else if (vector3.y < 0) return 2;
        else if (vector3.x > 0) return 1;
        else if (vector3.x < 0) return 3;
        else return -1;
    }

    private void FindCurrentTile()
    {
        Vector3 testPos = transform.position;
        testPos.y -= 0.2f;

        var hit_0 = Physics2D.OverlapCircle(testPos, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_0) _currentTile = hit_0.GetComponent<TileHandler>();
    }
}
