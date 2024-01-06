using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{

    //ref
    SpriteRenderer _sr;


    //settings
    [SerializeField] Transform[] _searchPoints = null;
    [SerializeField] TileController.TileType _tileType = TileController.TileType.ShallowWater;

    //state
    [SerializeField] TileHandler[] _neighbors_Orthogonal = new TileHandler[4];


    private void Start()
    {
        SetDataFromTileType(TileLibrary.Instance);
        FindNeighbors_Orthogonal();
    }

    private void FindNeighbors_Orthogonal()
    {
        var hit_0 = Physics2D.OverlapCircle(_searchPoints[0].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_0) _neighbors_Orthogonal[0] = hit_0.GetComponent<TileHandler>();

        var hit_1 = Physics2D.OverlapCircle(_searchPoints[1].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_1) _neighbors_Orthogonal[1] = hit_1.GetComponent<TileHandler>();

        var hit_2 = Physics2D.OverlapCircle(_searchPoints[2].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_2) _neighbors_Orthogonal[2] = hit_2.GetComponent<TileHandler>();

        var hit_3 = Physics2D.OverlapCircle(_searchPoints[3].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_3) _neighbors_Orthogonal[3] = hit_3.GetComponent<TileHandler>();
    }



    [ExecuteInEditMode]
    public void SetDataFromTileType(TileLibrary tileLibRef)
    {
        _sr = GetComponent<SpriteRenderer>();
        if (_tileType == TileController.TileType.ShallowWater)
        {
            _sr.sprite = tileLibRef.GetRandomWaterSprite();
            gameObject.layer = Layers.TilesTraversable;
        }
        else if (_tileType == TileController.TileType.Land)
        {
            _sr.sprite = tileLibRef.GetRandomLandSprite();
            gameObject.layer = Layers.TilesNonTraversable;
        }
    }

    public TileHandler GetNeighboringTile(int direction)
    {
        if (direction <0 || direction > 3)
        {
            Debug.LogWarning("Invalid direction!");
            return null;
        }
        else if (_neighbors_Orthogonal[direction] != null)
        {
            return _neighbors_Orthogonal[direction];
        }
        else
        {
            Debug.Log("No neighbor in that direction...");
            return null;
        }

    }

}
