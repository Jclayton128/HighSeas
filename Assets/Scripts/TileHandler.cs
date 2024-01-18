using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandler : MonoBehaviour
{

    //ref
    [SerializeField] SpriteRenderer _srBase;
    [SerializeField] SpriteRenderer _srObject;



    //settings
    [SerializeField] Transform[] _searchPoints = null;
    [SerializeField] TileController.TileType _tileType = TileController.TileType.ShallowWater;
    [SerializeField] TileController.TileObject _tileObject = TileController.TileObject.Nothing;
    [SerializeField] CityHandler _cityPrefab = null;
    [SerializeField] SmithHandler _smithPrefab = null;
    [SerializeField] GameObject _castlePrefab = null;


    //state
    public TileController.TileType TileType => _tileType;
    TileHandler[] _neighbors_All = new TileHandler[4];
    public TileHandler[] Neighbors => _neighbors_All;
    TileHandler[] _neighbors_Traversable = new TileHandler[4];
    public CityHandler City { get; private set; }
    public SmithHandler Smith { get; private set; }
    public GameObject Castle { get; private set; }


    private void Start()
    {
        SetDataFromTileType(TileLibrary.Instance);
        FindNeighbors_All();
        FindNeighbors_Traversable();
    }

    private void FindNeighbors_All()
    {
        var hit_0 = Physics2D.OverlapCircle(_searchPoints[0].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_0) _neighbors_All[0] = hit_0.GetComponent<TileHandler>();

        var hit_1 = Physics2D.OverlapCircle(_searchPoints[1].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_1) _neighbors_All[1] = hit_1.GetComponent<TileHandler>();

        var hit_2 = Physics2D.OverlapCircle(_searchPoints[2].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_2) _neighbors_All[2] = hit_2.GetComponent<TileHandler>();

        var hit_3 = Physics2D.OverlapCircle(_searchPoints[3].position, 0.1f, Layers.LayerMask_AllTiles);
        if (hit_3) _neighbors_All[3] = hit_3.GetComponent<TileHandler>();
    }

    private void FindNeighbors_Traversable()
    {
        var hit_0 = Physics2D.OverlapCircle(_searchPoints[0].position, 0.1f, Layers.LayerMask_TilesTraversable);
        if (hit_0) _neighbors_Traversable[0] = hit_0.GetComponent<TileHandler>();

        var hit_1 = Physics2D.OverlapCircle(_searchPoints[1].position, 0.1f, Layers.LayerMask_TilesTraversable);
        if (hit_1) _neighbors_Traversable[1] = hit_1.GetComponent<TileHandler>();

        var hit_2 = Physics2D.OverlapCircle(_searchPoints[2].position, 0.1f, Layers.LayerMask_TilesTraversable);
        if (hit_2) _neighbors_Traversable[2] = hit_2.GetComponent<TileHandler>();

        var hit_3 = Physics2D.OverlapCircle(_searchPoints[3].position, 0.1f, Layers.LayerMask_TilesTraversable);
        if (hit_3) _neighbors_Traversable[3] = hit_3.GetComponent<TileHandler>();
    }


    [ExecuteInEditMode]
    public void SetDataFromTileType(TileLibrary tileLibRef)
    {
        SetBaseTile(tileLibRef);
        SetTileObject(tileLibRef);
    }

    private void SetBaseTile(TileLibrary tileLibRef)
    {
        if (_tileType == TileController.TileType.ShallowWater)
        {
            _srBase.sprite = tileLibRef.GetRandomWaterSprite();
            gameObject.layer = Layers.TilesTraversable;
        }
        else if (_tileType == TileController.TileType.Land)
        {
            _srBase.sprite = tileLibRef.GetRandomLandSprite();
            gameObject.layer = Layers.TilesNonTraversable;
        }
        else if (_tileType == TileController.TileType.DecoyWater)
        {
            _srBase.sprite = tileLibRef.GetRandomWaterSprite();
            gameObject.layer = 1;
        }
        else if (_tileType == TileController.TileType.Pier)
        {
            _srBase.sprite = tileLibRef.GetRandomPierSprite();
            gameObject.layer = Layers.TilesTraversable;
            gameObject.AddComponent<PierHandler>();
        }
    }

    private void SetTileObject(TileLibrary tileLibRef)
    {
        if (_tileObject == TileController.TileObject.Nothing)
        {
            _srObject.sprite = null;
        }
        else if (_tileObject == TileController.TileObject.City)
        {
            if (!City) City = GetComponentInChildren<CityHandler>();
            if (City) return;
            _srObject.sprite = null;
            City =Instantiate(_cityPrefab, transform);
        }
        else if (_tileObject == TileController.TileObject.Smith)
        {
            if (!Smith) Smith = GetComponentInChildren<SmithHandler>();
            if (Smith) return;
            _srObject.sprite = null;
            Smith = Instantiate(_smithPrefab, transform);
        }
        else if (_tileObject == TileController.TileObject.Castle)
        {
            if (!Castle) Castle = GetComponentInChildren<CannonHandler>().transform.parent.gameObject;
            if (Castle ) return;
            _srObject.sprite = null;
            Castle = Instantiate(_castlePrefab, transform);
        }
    }

    public TileHandler GetNeighboringTile_All(int direction)
    {
        //Debug.Log("received " + direction);
        if (direction <0 || direction > 3)
        {
            //Debug.LogWarning("Invalid direction!");
            return null;
        }
        else if (_neighbors_All[direction] != null)
        {
            return _neighbors_All[direction];
        }
        else
        {
            //Debug.Log("No neighbor in that direction...");
            return null;
        }
    }
    public TileHandler GetNeighboringTile_Traversable(int direction)
    {
        //Debug.Log("received " + direction);
        if (direction < 0 || direction > 3)
        {
            Debug.LogWarning("Invalid direction!");
            return null;
        }
        else if (_neighbors_Traversable[direction] != null)
        {
            return _neighbors_Traversable[direction];
        }
        else
        {
            //Debug.Log("No neighbor in that direction...");
            return null;
        }
    }

}
