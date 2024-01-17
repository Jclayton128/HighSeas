using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public enum TileType
    {
        Land, ShallowWater, DecoyWater, Pier, City, Water5,
        Water6, Water7, Water8, Water9, Water10, Water11, Water12, Water13, Water14, Water15,
        Water16, Water17, Water18, Water19
    }

    public enum TileObject
    {
        Nothing, City, Castle, Tree, Rock, Smith
    }
    
    public static TileController Instance { get; private set; }
    [SerializeField] TileLibrary _tileLibraryReference;
    [SerializeField] float _waveAmplitude = 1.2f;
    [SerializeField] float _waveFrequency = 1f;

    //state
    [SerializeField] TileHandler[] _tileCollection = null;
    public float WaveAmplitude => _waveAmplitude;
    public float WaveFrequency => _waveFrequency;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetAllTileDataFromTileType();
    }

    [ExecuteInEditMode] [ContextMenu("Force Tile Sprite Update")]
    public void SetAllTileDataFromTileType()
    {
        foreach (var tile in _tileCollection)
        {
            tile.SetDataFromTileType(_tileLibraryReference);
        }
    }
}
