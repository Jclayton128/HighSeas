using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SmithHandler : MonoBehaviour
{

    public Action<SmithLibrary.SmithType> UpgradeCompleted;
    SmithLibrary.SmithType _smithType;
    public SmithLibrary.SmithType SmithType => _smithType;

    [SerializeField] Image _smithTypeImage = null;
    [SerializeField] Image _upgradeRing = null;
    [SerializeField] TextMeshProUGUI _upgradeCostTMP = null;

    //state
    float _upgradeFactor = 0;
    int _upgradesGiven = 0;
    int _currentUpgradeCost = 0;
    public int CurrentUpgradeCost => _currentUpgradeCost;
    bool _isUpgrading = false;



    private void Start()
    {
        SetSmithType();
        SetUpgradeCost();
        SmithController.Instance.RegisterSmith(this);
    }

    private void SetSmithType()
    {
        _smithType = SmithController.Instance.GetNextSmithType();
        _smithTypeImage.sprite = SmithLibrary.Instance.GetUpgradeSprite(_smithType);
        _smithTypeImage.color = Color.black;

    }

    private void SetUpgradeCost()
    {
        _currentUpgradeCost = BalanceLibrary.Instance.GetUpgradeCostByCount(_upgradesGiven);
        _upgradeCostTMP.text = _currentUpgradeCost.ToString();
    }

    public void Debug_DevelopSmith()
    {
        _upgradesGiven++;
        SetUpgradeCost();
    }

    

    private void Update()
    {
        if (_isUpgrading)
        {
            _upgradeFactor += Time.deltaTime * BalanceLibrary.Instance.UpgradeRate;
            _upgradeRing.fillAmount = _upgradeFactor;
            if (_upgradeFactor >= 1)
            {
                FinishUpgrade();
            }
        }
    }

    public void StartUpgrade()
    {
        _isUpgrading = true;
        _upgradeFactor = 0;
        _upgradeRing.fillAmount = _upgradeFactor;
    }

    private void FinishUpgrade()
    {
        _upgradeFactor = 0;
        _upgradeRing.fillAmount = _upgradeFactor;
        _isUpgrading = false; 
        UpgradeCompleted?.Invoke(_smithType);
        _upgradesGiven++;
        SetUpgradeCost();
    }

    public void CancelUpgrade()
    {
        _upgradeFactor = 0;
        _upgradeRing.fillAmount = _upgradeFactor;
        _isUpgrading = false;
    }
}
