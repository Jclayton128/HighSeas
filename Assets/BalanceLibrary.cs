using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceLibrary : MonoBehaviour
{
    public static BalanceLibrary Instance { get; private set; }

    
    
    [SerializeField] float _onloadRate = 0.3f;
    public float OnloadRate => _onloadRate;
    [SerializeField] float _offloadRate = 0.3f;
    public float OffloadRate => _offloadRate;
    [SerializeField] int _lowDemandProfit = 3;
    public int LowDemandProfit => _lowDemandProfit;
    [SerializeField] int _midDemandProfit = 4;
    public int MidDemandProfit => _midDemandProfit;
    [SerializeField] int _highDemandProfit = 5;
    public int HighDemandProfit => _highDemandProfit;

    [SerializeField] float _demandSatisfactionByCargo = 0.33f;
    public float DemandSatisfactionByCargo => _demandSatisfactionByCargo;

    [SerializeField] List<int> _upgradeCosts = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
    [SerializeField] float _upgradeRate = 0.2f;
    public float UpgradeRate => _upgradeRate;

    [SerializeField] List<float> _speeds = new List<float> { 1, 1.5f, 2 };

    private void Awake()
    {
        Instance = this;
    }

    public int GetUpgradeCostByCount(int previousUpgrades)
    {
        int cost;
        if (previousUpgrades >= _upgradeCosts.Count)
        {
            cost = _upgradeCosts[_upgradeCosts.Count - 1];
        }
        else
        {
            cost = _upgradeCosts[previousUpgrades];
        }
        return cost;
    }

    public float GetSpeedByCount(int previousUpgrades)
    {
        if (previousUpgrades >= _speeds.Count) return _speeds[_speeds.Count-1];
        return _speeds[previousUpgrades];
    }


}
