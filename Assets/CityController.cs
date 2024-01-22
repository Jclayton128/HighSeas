using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    public static CityController Instance { get; private set; }

    //state
    int _indexer = 0;
    List<CityHandler> _cities = new List<CityHandler>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterCity(CityHandler city)
    {
        _cities.Add(city);
    }

    public CargoLibrary.CargoType GetNextCargoType()
    {
        var cargo = (CargoLibrary.CargoType)_indexer;
        _indexer++;
        if (_indexer >= (int)CargoLibrary.CargoType.Empty) _indexer = 0;
        return cargo;
    }

    public void Debug_DevelopCities()
    {
        foreach (var city in _cities)
        {
            city.Debug_DevelopCity();
        }
    }

    public Transform GetTransformOfCity(int cityIndex)
    {
        return _cities[cityIndex].transform;
    }

    public void Debug_DemoCityByIndex(int indexToDemo)
    {
        _cities[indexToDemo].Debug_DemoCity();
    }
}
