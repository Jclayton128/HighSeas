using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CityHandler : MonoBehaviour
{
    public Action<CargoLibrary.CargoType> CargoLoadCompleted;


    [SerializeField] SpriteRenderer _sr = null;

    [SerializeField] Image[] _tripleProduction = null;

    [SerializeField] Image[] _demandIcons = null;
    [SerializeField] Image[] _productionFillRings = null;

    //settings
    float _productionRate = 0.03f;
    int _maxProduction = 3;
    float[] _demandRates = new float[4] { 0,0,0,0 };
    float _demandSatisfactionPerCargo = 0.33f;
    float _loadRate = 0.2f;
    [SerializeField] float[] _cargoPaymentThresholds = new float[3];


    //state
    [SerializeField] CargoLibrary.CargoType _cargoType = CargoLibrary.CargoType.Cargo0;
    public CargoLibrary.CargoType CargoType => _cargoType;
    [SerializeField] float[] _demands = new float[4];
    [SerializeField] float _currentProductionFactor;
    [SerializeField] int _productionInStock = 0;
    public int ProductionInStock => _productionInStock;

    Tween[] _demandTweens = new Tween[4];
    bool[] _isBuzzing = new bool[4] { false,false,false, false };
    float _timeSinceLastUpdate;

    [SerializeField] bool _isLoadingProduct = false;
    [SerializeField] float _loadFactor = 0;


    void Start()
    {
        _cargoType = CityController.Instance.GetNextCargoType();
        _sr.sprite = TileLibrary.Instance.GetRandomCitySprite();
        AssignDemandSprites();
        UpdateProductionImages();
        RandomlyAssignDemandRates();
        UpdateDemandIcons();
        UpdateProductionRings();
    }


    private void AssignDemandSprites()
    {
        for (int i = 0; i < _demandIcons.Length; i++)
        {
            _demandIcons[i].sprite = CargoLibrary.Instance.GetCargoSprite(i);
            _demandIcons[i].color = CargoLibrary.Instance.GetCargoColor(i);
        }
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
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate >= 0.5f)
        {
            UpdateProduction();
            UpdateDemand();
            _timeSinceLastUpdate = 0;
        }

        if (_isLoadingProduct && _productionInStock>0)
        {
            UpdateLoading();
        }

    }

    private void UpdateLoading()
    {
        _loadFactor += Time.deltaTime * _loadRate;

        _productionFillRings[_productionInStock - 1].fillAmount = 1-_loadFactor;
        _productionFillRings[_productionInStock - 1].color = Color.grey;

        if (_loadFactor>= 1)
        {
            _loadFactor = 0;
            _productionInStock--;
            CargoLoadCompleted?.Invoke(_cargoType);
            UpdateProductionImages();
            UpdateProductionRings();
        }
    }

    private void UpdateProduction()
    {

        if (_productionInStock >= _maxProduction) return;

        _currentProductionFactor += _productionRate * 
            UnityEngine.Random.Range(0.8f, 1.2f);

        if (_currentProductionFactor >= 1)
        {
            _productionInStock++;
            _currentProductionFactor = 0;

            UpdateProductionImages();
        }
        UpdateProductionRings();
    }

    private void UpdateProductionImages()
    {
        if (_productionInStock == 0)
        {
            _tripleProduction[0].CrossFadeAlpha(0.2f, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(0.2f, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(0.2f, 0.0001f, true);
        }
        else if (_productionInStock == 1)
        {
            _tripleProduction[0].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(0.2f, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(0.2f, 0.0001f, true);
        }
        else if (_productionInStock == 2)
        {
            _tripleProduction[0].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(0.2f, 0.0001f, true);
        }
        else if (_productionInStock == 3)
        {
            _tripleProduction[0].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[1].CrossFadeAlpha(1, 0.0001f, true);
            _tripleProduction[2].CrossFadeAlpha(1, 0.0001f, true);
        }
    }

    private void UpdateProductionRings()
    {

        if (_productionInStock == 0)
        {
            _productionFillRings[0].color = Color.white;
            _productionFillRings[0].fillAmount = _currentProductionFactor;
            _productionFillRings[1].fillAmount = 0;
            _productionFillRings[2].fillAmount = 0;
        }
        else if (_productionInStock == 1)
        {
            _productionFillRings[0].color = Color.white;
            _productionFillRings[1].color = Color.white;
            _productionFillRings[0].fillAmount = 1;
            _productionFillRings[1].fillAmount = _currentProductionFactor;
            _productionFillRings[2].fillAmount = 0;
        }
        else if (_productionInStock == 2)
        {
            _productionFillRings[0].color = Color.white;
            _productionFillRings[1].color = Color.white;
            _productionFillRings[2].color = Color.white;
            _productionFillRings[0].fillAmount = 1;
            _productionFillRings[1].fillAmount = 1;
            _productionFillRings[2].fillAmount = _currentProductionFactor;
        }
        else if (_productionInStock == 3)
        {
            _productionFillRings[0].color = Color.white;
            _productionFillRings[1].color = Color.white;
            _productionFillRings[2].color = Color.white;
            _productionFillRings[0].fillAmount = 1;
            _productionFillRings[1].fillAmount = 1;
            _productionFillRings[2].fillAmount = 1;
        }

    }



    private void UpdateDemand()
    {
        for (int i = 0; i < _demands.Length; i++)
        {
            if (i == (int)_cargoType) continue;
            _demands[i] += _demandRates[i];
            _demands[i] = Mathf.Clamp(_demands[i], 0, 1f);
        }
        UpdateDemandIcons();
    }

    private void UpdateDemandIcons()
    {
        //Adjust demand amount for most demanded
        for (int i = 0; i < _demands.Length; i++)
        {
            if (_demands[i] > _cargoPaymentThresholds[2])
            {
                //highest
                if (!_isBuzzing[i])
                {
                    _demandTweens[i].Kill();
                    _demandIcons[i].transform.DOScale(1.3f, .7f).SetLoops(-1, LoopType.Yoyo);
                    _demandIcons[i].CrossFadeAlpha(1f, 0.0001f, true);
                    //_demandIcons[i].transform.localScale = (1 * Vector3.one);
                    _isBuzzing[i] = true;
                }
            }
            else if (_demands[i] <= _cargoPaymentThresholds[0])
            {
                _demandTweens[i].Kill();
                _isBuzzing[i] = false;
                _demandIcons[i].CrossFadeAlpha(0.2f, 0.0001f, true);
                _demandIcons[i].transform.localScale = (0.5f * Vector3.one);
            }
            else if (_demands[i] <= _cargoPaymentThresholds[2] &&
            _demands[i] > _cargoPaymentThresholds[1])
            {
                //second highest
                _demandTweens[i].Kill();
                _isBuzzing[i] = false;
                _demandIcons[i].CrossFadeAlpha(1f, 0.0001f, true);
                _demandIcons[i].transform.localScale = (1f * Vector3.one);
            }
            else
            {
                //third highest
                _demandTweens[i].Kill();
                _isBuzzing[i] = false;
                _demandIcons[i].CrossFadeAlpha(1f, 0.0001f, true);
                _demandIcons[i].transform.localScale = (0.7f * Vector3.one);
            }
        }
    }

    public bool CheckIfCityWantsCargo(CargoLibrary.CargoType cargoToSell)
    {
        if (_demands[(int)cargoToSell] > _cargoPaymentThresholds[0])
        {
            return true;
        }
        else return false;
    }

    public void SatisfyDemandByOneCargo(CargoLibrary.CargoType cargoType)
    {
        if (_demands[(int)cargoType] < _cargoPaymentThresholds[0])
        {
            Debug.Log("Demand is too low to sell that here!");
            return;
        }
        _demands[(int)cargoType] -= _demandSatisfactionPerCargo;
        _demands[(int)cargoType] = Mathf.Clamp01(_demands[(int)cargoType]);
        UpdateDemandIcons();

        //JUICE TODO particle effect for cash produced
        //TODO Pay the player who delivered the cargo

    }

    public void StartOnload()
    {
        _isLoadingProduct = true;
        _loadFactor = 0;
    }

    public void CancelOnload()
    {
        _isLoadingProduct = false;
        _loadFactor = 0;
    }
}
