using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    public static CityController Instance { get; private set; }

    //state
    int _indexer = -1;

    private void Awake()
    {
        Instance = this;
    }

    public CargoLibrary.CargoType GetNextCargoType()
    {
        var cargo = (CargoLibrary.CargoType)_indexer;
        _indexer++;
        if (_indexer >= (int)CargoLibrary.CargoType.Count) _indexer = 0;
        return cargo;
    }
}
