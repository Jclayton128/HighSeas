using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{

    //ref
    SpriteRenderer _sr;

    //state
    [SerializeField] TileController.TileType _tileType = TileController.TileType.ShallowWater;

    private void Start()
    {
        SetSpriteFromTileType(TileLibrary.Instance);
    }
    
    [ExecuteInEditMode]
    public void SetSpriteFromTileType(TileLibrary tileLibRef)
    {
        _sr = GetComponent<SpriteRenderer>();
        if (_tileType == TileController.TileType.ShallowWater)
        {
            _sr.sprite = tileLibRef.GetRandomWaterSprite();
        }
        else if (_tileType == TileController.TileType.Land)
        {
            _sr.sprite = tileLibRef.GetRandomLandSprite();
        }
    }

}
