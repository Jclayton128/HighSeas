using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public enum TileType
    {
        Land, ShallowWater, DeepWater, Water3, Water4, Water5,
        Water6, Water7, Water8, Water9, Water10, Water11, Water12, Water13, Water14, Water15,
        Water16, Water17, Water18, Water19
    }
    
    public static TileController Instance { get; private set; }
    [SerializeField] TileLibrary _tileLibraryReference;

    //state
    [SerializeField] TileHandler[] _tileCollection = null;



    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("Force Tile Sprite Update")]
    public void ForceTilesSpriteFromTileType()
    {
        foreach (var tile in _tileCollection)
        {
            tile.SetDataFromTileType(_tileLibraryReference);
        }
    }
}
