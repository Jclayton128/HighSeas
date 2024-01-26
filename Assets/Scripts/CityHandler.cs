using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CityHandler : MonoBehaviour
{
    public Action<CargoLibrary.CargoType> CargoOnloadCompleted;
    public Action<CargoLibrary.CargoType, int> CargoOffloadCompleted;

    [SerializeField] Canvas _canvas = null;
    [SerializeField] SpriteRenderer _sr = null;
    [SerializeField] RectTransform[] _slots = null;
    [SerializeField] Image[] _tripleProduction = null;

    [SerializeField] Image[] _demandIcons = null;
    [SerializeField] Image[] _productionFillRings = null;
    [SerializeField] Image[] _demandFillRings = null;
    [SerializeField] Color _productionFillColor = Color.white;
    [SerializeField] Color _demandFillColor = Color.white;



    //settings
    float _productionRate = 0.03f;
    int _maxProduction = 3;
    float[] _demandRates = new float[4] { 0,0,0,0 };
    [SerializeField] float[] _cargoPaymentThresholds = new float[3];


    //state
    bool _isActive = false;
    [SerializeField] CargoLibrary.CargoType _cargoType = CargoLibrary.CargoType.Cargo0;
    public CargoLibrary.CargoType CargoType => _cargoType;
    [SerializeField] float[] _demands = new float[4];
    [SerializeField] float _currentProductionFactor;
    [SerializeField] int _productionInStock = 0;
    public int ProductionInStock => _productionInStock;

    Tween[] _demandTweens = new Tween[4];
    bool[] _isBuzzing = new bool[4] { false,false,false, false };
    float _timeSinceLastUpdate;

    bool _isLoadingProduct = false;
    float _loadFactor = 0;

    [SerializeField] bool _isOffloadingProduct = false;
    [SerializeField] float _offloadFactor = 0;
    [SerializeField] CargoLibrary.CargoType _cargoBeingOffloaded;
    bool _hasBeenDemod = false;


    #region Startup
    void Start()
    {
        HandleGameEnded();
        CityController.Instance.RegisterCity(this);
        _cargoType = CityController.Instance.GetNextCargoType();
        _sr.sprite = TileLibrary.Instance.GetRandomCitySprite();
        AssignDemandSprites();
        AssignProductionSprites();
        RandomlyAssignDemandRates();

        UpdateProductionImages();
        UpdateDemandIcons();
        UpdateProductionRings();

        GameController.Instance.GameModeStarted += HandleGameStarted;
        GameController.Instance.GameModeEnded += HandleGameEnded;
    }

    private void OnDestroy()
    {
        GameController.Instance.GameModeStarted -= HandleGameStarted;
        GameController.Instance.GameModeEnded -= HandleGameEnded;
    }

    private void HandleGameStarted()
    {
        _canvas.enabled = true;
        _isActive = true;
    }

    private void HandleGameEnded()
    {
        _canvas.enabled = false;
        _isActive = false;
    }

    private void AssignProductionSprites()
    {
        for (int i =0; i < _tripleProduction.Length; i++)
        {
            _tripleProduction[i].sprite = CargoLibrary.Instance.GetCargoSprite((int)_cargoType);
            _tripleProduction[i].color = CargoLibrary.Instance.GetCargoColor((int)_cargoType);
        }
    }

    private void AssignDemandSprites()
    {
        for (int i = 0; i < _demandIcons.Length; i++)
        {
            _demandIcons[i].sprite = CargoLibrary.Instance.GetCargoSprite(i);
            _demandIcons[i].color = CargoLibrary.Instance.GetCargoColor(i);
        }

        switch (_cargoType)
        {
            case CargoLibrary.CargoType.Cargo0:

                _demandIcons[0].color = Color.clear;
                _demandFillRings[0].color = Color.clear;

                _demandIcons[1].rectTransform.position = _slots[0].position;
                _demandIcons[2].rectTransform.position = _slots[1].position;
                _demandIcons[3].rectTransform.position = _slots[2].position;
                _demandFillRings[1].rectTransform.position = _slots[0].position;
                _demandFillRings[2].rectTransform.position = _slots[1].position;
                _demandFillRings[3].rectTransform.position = _slots[2].position;
                break;

            case CargoLibrary.CargoType.Cargo1:
                _demandIcons[1].color = Color.clear;
                _demandFillRings[1].color = Color.clear;

                _demandIcons[0].rectTransform.position = _slots[0].position;
                _demandIcons[2].rectTransform.position = _slots[1].position;
                _demandIcons[3].rectTransform.position = _slots[2].position;
                _demandFillRings[0].rectTransform.position = _slots[0].position;
                _demandFillRings[2].rectTransform.position = _slots[1].position;
                _demandFillRings[3].rectTransform.position = _slots[2].position;
                break;

            case CargoLibrary.CargoType.Cargo2:
                _demandIcons[2].color = Color.clear;
                _demandFillRings[2].color = Color.clear;

                _demandIcons[0].rectTransform.position = _slots[0].position;
                _demandIcons[1].rectTransform.position = _slots[1].position;
                _demandIcons[3].rectTransform.position = _slots[2].position;
                _demandFillRings[0].rectTransform.position = _slots[0].position;
                _demandFillRings[1].rectTransform.position = _slots[1].position;
                _demandFillRings[3].rectTransform.position = _slots[2].position;
                break;

            case CargoLibrary.CargoType.Cargo3:
                _demandIcons[3].color = Color.clear;
                _demandFillRings[3].color = Color.clear;

                _demandIcons[0].rectTransform.position = _slots[0].position;
                _demandIcons[1].rectTransform.position = _slots[1].position;
                _demandIcons[2].rectTransform.position = _slots[2].position;
                _demandFillRings[0].rectTransform.position = _slots[0].position;
                _demandFillRings[1].rectTransform.position = _slots[1].position;
                _demandFillRings[2].rectTransform.position = _slots[2].position;
                break;

        }

        for (int i = 0; i<_demandFillRings.Length; i++)
        {
            _demandFillRings[i].fillAmount = 0;
        }
    }

    private void RandomlyAssignDemandRates()
    {
        for (int i = 0; i < _demands.Length; i++)
        {
            if ((int)_cargoType == i) continue;
            _demandRates[i] = UnityEngine.Random.Range(0.01f, 0.03f)/6;
        }
    }

    #endregion

    #region Flow
    // Update is called once per frame
    void Update()
    {
        if (!_isActive) return;
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

        if (_isOffloadingProduct)
        {
            UpdateOffLoading();
        }

    }
     
    private void UpdateOffLoading()
    {
        _offloadFactor += Time.deltaTime * BalanceLibrary.Instance.OffloadRate;

        _demandFillRings[(int)_cargoBeingOffloaded].fillAmount = 1 - _offloadFactor;

        if (_offloadFactor >= 1)
        {
            _offloadFactor = 0;
            _isOffloadingProduct = false;
            int profit = SatisfyDemandByOneCargo(_cargoBeingOffloaded);
            //_cargoBeingOffloaded = CargoLibrary.CargoType.Count;
            CargoOffloadCompleted?.Invoke(_cargoBeingOffloaded, profit);
        }
    }

    private void UpdateLoading()
    {
        _loadFactor += Time.deltaTime * BalanceLibrary.Instance.OnloadRate;

        _productionFillRings[_productionInStock - 1].fillAmount = 1-_loadFactor;
        _productionFillRings[_productionInStock - 1].color = Color.grey;

        if (_loadFactor>= 1)
        {
            _loadFactor = 0;
            _productionInStock--;
            _isLoadingProduct = false;
            CargoOnloadCompleted?.Invoke(_cargoType);
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
            _productionFillRings[0].color = _productionFillColor;
            _productionFillRings[0].fillAmount = _currentProductionFactor;
            _productionFillRings[1].fillAmount = 0;
            _productionFillRings[2].fillAmount = 0;
        }
        else if (_productionInStock == 1)
        {
            _productionFillRings[0].color = Color.white;
            _productionFillRings[1].color = _productionFillColor;
            _productionFillRings[0].fillAmount = 1;
            _productionFillRings[1].fillAmount = _currentProductionFactor;
            _productionFillRings[2].fillAmount = 0;
        }
        else if (_productionInStock == 2)
        {
            _productionFillRings[0].color = Color.white;
            _productionFillRings[1].color = Color.white;
            _productionFillRings[2].color = _productionFillColor;
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
                    _demandTweens[i] = _demandIcons[i].transform.DOScale(1.3f, .7f).SetLoops(-1, LoopType.Yoyo);
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

    #endregion

    #region Cargo Handling

    public bool CheckIfCityWantsCargo(CargoLibrary.CargoType cargoToSell)
    {
        if (_demands[(int)cargoToSell] > _cargoPaymentThresholds[0])
        {
            return true;
        }
        else return false;
    }

    private int SatisfyDemandByOneCargo(CargoLibrary.CargoType cargoType)
    {
        var ot = DetermineProfitFromSale(cargoType);
        int profit = ot.Item1;
        SoundController.Instance.PlayClip(SoundLibrary.SoundID.SellCargo4, ot.Item2);

        if (profit == 0)
        {
            Debug.Log("Demand is too low to sell that here!");
            return 0;
        }

        _demands[(int)cargoType] -= BalanceLibrary.Instance.DemandSatisfactionByCargo;
        _demands[(int)cargoType] = Mathf.Clamp01(_demands[(int)cargoType]);
        UpdateDemandIcons();

        //JUICE TODO particle effect for cash produced
        //TODO Pay the player who delivered the cargo

        return profit;

    }

    /// <summary>
    /// returns profit, profit index
    /// </summary>
    /// <param name="cargoType"></param>
    /// <returns></returns>
    private (int,int) DetermineProfitFromSale(CargoLibrary.CargoType cargoType)
    {
        (int, int) ret;
        if (_demands[(int)cargoType] > _cargoPaymentThresholds[2])
        {
            //highes
            ret.Item1 = BalanceLibrary.Instance.HighDemandProfit;
            ret.Item2 = 3;
        }
        else if (_demands[(int)cargoType] <= _cargoPaymentThresholds[0])
        {
            //lowest
            ret.Item1 = 0;
            ret.Item2 = 0;
        }
        else if (_demands[(int)cargoType] <= _cargoPaymentThresholds[2] &&
        _demands[(int)cargoType] > _cargoPaymentThresholds[1])
        {
            //second highest
            ret.Item1 = BalanceLibrary.Instance.MidDemandProfit;
            ret.Item2 = 2;
        }
        else
        {
            //third highest
            ret.Item1 = BalanceLibrary.Instance.LowDemandProfit;
            ret.Item2 = 1;
        }
        return ret;
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

    public void StartOffLoad(CargoLibrary.CargoType cargoType)
    {
        _isOffloadingProduct = true;
        _offloadFactor = 0;
        _cargoBeingOffloaded = cargoType;
    }

    public void CancelOffLoad()
    {
        _isOffloadingProduct = false;
        _offloadFactor = 0;
    }

    #endregion

    #region Debug Tools

    public void Debug_DevelopCity()
    {
        _productionInStock = 3;
        _currentProductionFactor = 1;

        for (int i = 0; i < _demands.Length; i++)
        {
            _demands[i] = 1;
        }

        _demands[(int)_cargoType] = 0;

        UpdateProductionImages();

    }

    #endregion
    public void Debug_DemoCity()
    {
        if (_hasBeenDemod) return;
        _hasBeenDemod = true;
        _productionInStock = 2;
        _currentProductionFactor = 0.1f;
        UpdateProductionImages();

        _demands[0] = 0f;
        _demands[1] = 0.4f;
        _demands[2] = .6f;
        _demands[3] = 1f;
    }
}
