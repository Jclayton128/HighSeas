using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _winningShipTMP = null;


    //settings
    string _winningShipFollow = " \r\nis \r\nLord of the Sea!";

    //state
   string[] _playerNames = new string[4];

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterPlayerName(string name, int playerIndex)
    {
        _playerNames[playerIndex] = name;
    }

    public void SetWinningPlayerData(int winningActorIndex)
    {
        _winningShipTMP.text = _playerNames[winningActorIndex] + _winningShipFollow;
        //TODO complete rest of winning panel
    }
}
