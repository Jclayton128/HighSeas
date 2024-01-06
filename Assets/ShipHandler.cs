using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _baseShipSR = null;

    public void SetPlayerIndex(int index)
    {
        _baseShipSR.color = PlayerLibrary.Instance.GetPlayerColor(index);   
    }
}
