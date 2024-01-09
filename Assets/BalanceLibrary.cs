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



    private void Awake()
    {
        Instance = this;
    }


}
