using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DestinationHandler : MonoBehaviour
{
    public Action<TileHandler> DestinationChanged;

    /// <summary>
    /// 0: NW, 1: NE, 2: SE, 3: SW
    /// </summary>
    [SerializeField] SpriteRenderer[] _cornerSRs = null;

    //State
    Tween _scaleTween;
    TileHandler _currentTileHandler;
    public TileHandler CurrenTile => _currentTileHandler;
    [SerializeField] int _playerIndex = 0;

    private void Start()
    {
        SetDataFromPlayerIndex();
        FindCurrentTile();
        BeginScalePulsing();
    }

    public void SetPlayerIndex(int newIndex)
    {
        _playerIndex = newIndex;
        SetDataFromPlayerIndex();
    }

    private void SetDataFromPlayerIndex()
    {
        foreach (var sr in _cornerSRs)
        {
            sr.color = PlayerLibrary.Instance.GetPlayerColor(_playerIndex);
        }

        switch (_playerIndex)
        {
            case 0:
                _cornerSRs[0].sortingOrder = 4;
                _cornerSRs[1].sortingOrder = 1;
                _cornerSRs[2].sortingOrder = 3;
                _cornerSRs[3].sortingOrder = 2;
                break;

            case 1:
                _cornerSRs[0].sortingOrder = 2;
                _cornerSRs[1].sortingOrder = 4;
                _cornerSRs[2].sortingOrder = 1;
                _cornerSRs[3].sortingOrder = 3;
                break;

            case 2:
                _cornerSRs[0].sortingOrder = 3;
                _cornerSRs[1].sortingOrder = 2;
                _cornerSRs[2].sortingOrder = 4;
                _cornerSRs[3].sortingOrder = 1;
                break;

            case 3:
                _cornerSRs[0].sortingOrder = 1;
                _cornerSRs[1].sortingOrder = 3;
                _cornerSRs[2].sortingOrder = 2;
                _cornerSRs[3].sortingOrder = 4;
                break;


            case 4:
            case 5:
            case 6:
            case 7:
                _cornerSRs[0].sortingOrder = 5;
                _cornerSRs[1].sortingOrder = 5;
                _cornerSRs[2].sortingOrder = 5;
                _cornerSRs[3].sortingOrder = 5;
                break;

        }
    }

    private void BeginScalePulsing()
    {
        //JUICE TODO: sync these pulses up so that random joining players still pulse together
        _scaleTween = transform.DOScale(1.1f, 0.7f).SetLoops(-1, LoopType.Yoyo);
    }
    private void FindCurrentTile()
    {
        var hit_0 = Physics2D.OverlapCircle(transform.position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_0) _currentTileHandler= hit_0.GetComponent<TileHandler>();
    }


    public void CommandMove(int direction)
    {
        if (!_currentTileHandler) FindCurrentTile();

        var proposedDestination = _currentTileHandler.GetNeighboringTile_All(direction);
        if (proposedDestination == null) return;

        _currentTileHandler = proposedDestination;
        transform.position = _currentTileHandler.transform.position;

        DestinationChanged?.Invoke(proposedDestination);
    }

    public void JumpToTile(TileHandler destinationTile)
    {
        transform.position = destinationTile.transform.position;
        DestinationChanged?.Invoke(destinationTile);
    }

    internal void Destroy()
    {
        foreach (var sr in _cornerSRs)
        {
            sr.enabled = false;
        }
    }
}
