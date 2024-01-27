using System;
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


    public Transform GetTransformOfCity(int cityIndex)
    {
        return _cities[cityIndex].transform;
    }

    #region AI Tools

    /// <summary>
    /// Returns the CityHandler with the highest LoadScore (ProductionScore / distance from test point)
    /// </summary>
    /// <returns></returns>
    public CityHandler FindBestCityToLoadFrom(Vector3 testPosition)
    {
        CityHandler winningCity = null;
        float loadScoreToBeat = 0;
        float workingScore;
        foreach (var city in _cities)
        {
            workingScore = (0.1f + city.ProductionScore) / (city.transform.position - testPosition).magnitude;

            if (workingScore > loadScoreToBeat)
            {
                winningCity = city;
                loadScoreToBeat = workingScore;
            }
        }
        //Debug.Log(inningCity + $"Load carg score: {loadScoreToBeat}");
        return winningCity;
    }

    internal CityHandler FindBestCityToSellAt(Vector3 testPosition, List<CargoLibrary.CargoType> cargosInHold)
    {
        CityHandler winningCity = null;
        float loadScoreToBeat = 0;
        float workingScore;
        foreach (var city in _cities)
        {
            workingScore = city.GetHighestDemandScoreFromAvailableCargos(cargosInHold) /
                (city.transform.position - testPosition).magnitude;

            if (workingScore > loadScoreToBeat)
            {
                winningCity = city;
                loadScoreToBeat = workingScore;
            }
        }

        return winningCity;
    }

    #endregion;

    #region Debugs
    public void Debug_DemoCityByIndex(int indexToDemo)
    {
        _cities[indexToDemo].Debug_DemoCity();
    }


    public void Debug_DevelopCities()
    {
        foreach (var city in _cities)
        {
            city.Debug_DevelopCity();
        }
    }

    #endregion
}
