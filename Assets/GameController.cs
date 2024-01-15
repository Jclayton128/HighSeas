using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }


    //settings
    [SerializeField] int _coinsRequiredToWin = 25;
    int _winningActorIndex = -1;


    private void Awake()
    {
        Instance = this;
    }

    public void HandleCoinsGained(int actorIndex, int playerCurrentCoin)
    {
        if (playerCurrentCoin >= _coinsRequiredToWin)
        {
            //execute game over flow
            _winningActorIndex = actorIndex;
        }
    }
}
