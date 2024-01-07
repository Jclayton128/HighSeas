using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CityHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr = null;

    [SerializeField] Image _singleProduction = null;
    [SerializeField] Image[] _doubleProduction = null;
    [SerializeField] Image[] _tripleProduction = null;

    [SerializeField] Image[] _demandIcons = null;

    //settings
    float _productionRate = 0.1f;
    int _maxProduction = 3;
    float[] _demandRates = new float[4] { 0,0,0,0 };
    float _demandSatisfactionPerCargo = 0.3f;
    [SerializeField] float[] _cargoPaymentThresholds = new float[4];

    //state
    [SerializeField] CargoLibrary.CargoType _cargoType = CargoLibrary.CargoType.Cargo0;
    [SerializeField] float[] _demands = new float[4];
    [SerializeField] float _currentProductionFactor;
    [SerializeField] int _productionInStock = 0;
    Tween[] _demandTweens = new Tween[3];
    int[] _currentRanks = new int[3];
    int[] _prevRanks = new int[3];

    void Start()
    {
        _sr.sprite = TileLibrary.Instance.GetRandomCitySprite();
        UpdateProductionImages();
        RandomlyAssignDemandRates();

    }

    private void RandomlyAssignDemandRates()
    {
        for (int i = 0; i < _demands.Length; i++)
        {
            if ((int)_cargoType == i) continue;
            _demandRates[i] = UnityEngine.Random.Range(0.03f, 0.06f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProduction();
        UpdateDemand();
    }

    private void UpdateProduction()
    {
        if (_productionInStock >= _maxProduction) return;

        _currentProductionFactor += Time.deltaTime * _productionRate * 
            UnityEngine.Random.Range(0.8f, 1.2f);

        if (_currentProductionFactor >= 1)
        {
            _productionInStock++;
            _currentProductionFactor = 0;

            UpdateProductionImages();
        }
    }

    private void UpdateProductionImages()
    {
        if (_productionInStock == 0)
        {
            _singleProduction.CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[0].CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[1].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[0].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(0, 0.0001f, true);
        }
        else if (_productionInStock == 1)
        {
            _singleProduction.CrossFadeAlpha(1, 0.0001f, true);
            _doubleProduction[0].CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[1].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[0].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(0, 0.0001f, true);
        }
        else if (_productionInStock == 2)
        {
            _singleProduction.CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[0].CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[1].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[0].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(0, 0.0001f, true);
        }
        else if (_productionInStock == 3)
        {
            _singleProduction.CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[0].CrossFadeAlpha(0, 0.0001f, true);
            _doubleProduction[1].CrossFadeAlpha(0, 0.0001f, true);
            _tripleProduction[0].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(1, 0.0001f, true);
        }
    }

    private void UpdateDemand()
    {
        for (int i = 0; i < _demands.Length; i++)
        {
            if (i == (int)_cargoType) continue;
            _demands[i] += Time.deltaTime * _demandRates[i];
            _demands[i] = Mathf.Clamp(_demands[i], 0, 1f);
        }
        UpdateDemandIcons();
    }

    private void UpdateDemandIcons()
    {
        _currentRanks = GetTopThreeIndices(_demands);

        _demandIcons[2].sprite = CargoLibrary.Instance.GetCargoSprite(_currentRanks[0]);
        _demandIcons[2].color = CargoLibrary.Instance.GetCargoColor(_currentRanks[0]);

        _demandIcons[1].sprite = CargoLibrary.Instance.GetCargoSprite(_currentRanks[1]);
        _demandIcons[1].color = CargoLibrary.Instance.GetCargoColor(_currentRanks[1]);

        _demandIcons[0].sprite = CargoLibrary.Instance.GetCargoSprite(_currentRanks[2]);
        _demandIcons[0].color = CargoLibrary.Instance.GetCargoColor(_currentRanks[2]);


        //Adjust demand amount for most demanded

        if (_demands[_currentRanks[0]] > _cargoPaymentThresholds[3])
        {
            //highest
            _demandIcons[2].CrossFadeAlpha(1, 0.0001f, true);
            _demandIcons[2].transform.localScale = (1.2f * Vector3.one);

        }
        else if (_demands[_currentRanks[0]] <= _cargoPaymentThresholds[0])
        {
            //lowest
            _demandIcons[2].CrossFadeAlpha(0.5f, 0.0001f, true);
            _demandIcons[2].transform.localScale = (0.5f * Vector3.one);
        }
        else if (_demands[_currentRanks[0]] <= _cargoPaymentThresholds[3] &&
            _demands[_currentRanks[0]] > _cargoPaymentThresholds[1])
        {
            //second highest
            _demandIcons[2].CrossFadeAlpha(1f, 0.0001f, true);
            _demandIcons[2].transform.localScale = (1f * Vector3.one);
        }
        else
        {
            //third highest
            _demandIcons[2].CrossFadeAlpha(1f, 0.0001f, true);
            _demandIcons[2].transform.localScale = (0.7f * Vector3.one);
        }

        //adjust demand for second most demanded

        if (_demands[_currentRanks[1]] > _cargoPaymentThresholds[3])
        {
            //highest
            _demandIcons[1].CrossFadeAlpha(1, 0.0001f, true);
            _demandIcons[1].transform.localScale = (1.2f * Vector3.one);

        }
        else if (_demands[_currentRanks[1]] <= _cargoPaymentThresholds[0])
        {
            //lowest
            _demandIcons[1].CrossFadeAlpha(0.2f, 0.0001f, true);
            _demandIcons[1].transform.localScale = (0.5f * Vector3.one);
        }
        else if (_demands[_currentRanks[1]] <= _cargoPaymentThresholds[3] &&
            _demands[_currentRanks[1]] > _cargoPaymentThresholds[1])
        {
            //second highest
            _demandIcons[1].CrossFadeAlpha(1f, 0.0001f, true);
            _demandIcons[1].transform.localScale = (1f * Vector3.one);
        }
        else
        {
            //third highest
            _demandIcons[1].CrossFadeAlpha(1f, 0.0001f, true);
            _demandIcons[1].transform.localScale = (0.7f * Vector3.one);
        }

        //adjust demand for third most demanded

        if (_demands[_currentRanks[2]] > _cargoPaymentThresholds[3])
        {
            //highest
            _demandIcons[0].CrossFadeAlpha(1, 0.0001f, true);
            _demandIcons[0].transform.localScale = (1.2f * Vector3.one);

        }
        else if (_demands[_currentRanks[2]] <= _cargoPaymentThresholds[0])
        {
            //lowest
            _demandIcons[0].CrossFadeAlpha(0.2f, 0.0001f, true);
            _demandIcons[0].transform.localScale = (0.5f * Vector3.one);
        }
        else if (_demands[_currentRanks[2]] <= _cargoPaymentThresholds[3] &&
            _demands[_currentRanks[2]] > _cargoPaymentThresholds[1])
        {
            //second highest
            _demandIcons[0].CrossFadeAlpha(1f, 0.0001f, true);
            _demandIcons[0].transform.localScale = (1f * Vector3.one);
        }
        else
        {
            //third highest
            _demandIcons[0].CrossFadeAlpha(1f, 0.0001f, true);
            _demandIcons[0].transform.localScale = (0.7f * Vector3.one);
        }

    }

    private int[] GetTopThreeIndices(float[] inputArray)
    {
        if (inputArray.Length < 4)
        {
            throw new ArgumentException("Input array must have at least 4 elements");
        }

        int[] resultArray = new int[3];

        // Find the index of the highest float
        int highestIndex = 0;
        for (int i = 1; i < inputArray.Length; i++)
        {
            if (inputArray[i] > inputArray[highestIndex])
            {
                highestIndex = i;
            }
        }

        resultArray[0] = highestIndex;

        // Find the index of the second highest float
        int secondHighestIndex = (highestIndex == 0) ? 1 : 0;
        for (int i = 0; i < inputArray.Length; i++)
        {
            if (i != highestIndex && inputArray[i] > inputArray[secondHighestIndex])
            {
                secondHighestIndex = i;
            }
        }

        resultArray[1] = secondHighestIndex;

        // Find the index of the third highest float
        int thirdHighestIndex = (highestIndex == 0 || secondHighestIndex == 0) ? 1 : 0;
        for (int i = 0; i < inputArray.Length; i++)
        {
            if (i != highestIndex && i != secondHighestIndex && inputArray[i] > inputArray[thirdHighestIndex])
            {
                thirdHighestIndex = i;
            }
        }

        resultArray[2] = thirdHighestIndex;

        return resultArray;
    }


    public void SatisfyDemandByOneCargo(CargoLibrary.CargoType cargoType)
    {
        if (_demands[(int)cargoType] < _cargoPaymentThresholds[0])
        {
            Debug.Log("Demand is too low to sell that here!");
            return;
        }
        _demands[(int)cargoType] -= _demandSatisfactionPerCargo;
        //JUICE TODO particle effect for cash produced
        //TODO Pay the player who delivered the cargo

    }
}
