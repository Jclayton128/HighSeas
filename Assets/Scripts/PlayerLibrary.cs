using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLibrary : MonoBehaviour
{
    public static PlayerLibrary Instance { get; private set; }

    [SerializeField] Color[] _playerColors = null;
    [SerializeField] Sprite[] _playerIcon = null;

    private void Awake()
    {
        Instance = this;
    }

    public Color GetPlayerColor(int playerIndex)
    {
        return _playerColors[playerIndex];
    }
}
