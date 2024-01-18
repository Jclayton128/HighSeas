using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetHandler : MonoBehaviour
{

    /// <summary>
    /// 0: NW, 1: NE, 2: SE, 3: SW
    /// </summary>
    [SerializeField] SpriteRenderer[] _cornerSRs = null;

    //State
    Tween _scaleTween;
    TileHandler _currentTileHandler;
    public TileHandler CurrenTile => _currentTileHandler;
    int _playerIndex = -1;

    private void Start()
    {
        SetDataFromPlayerIndex();
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

        }
    }

    private void BeginScalePulsing()
    {
        //JUICE TODO: sync these pulses up so that random joining players still pulse together
        _scaleTween = transform.DOScale(1.1f, 0.35f).SetLoops(-1, LoopType.Yoyo);
    }
}
