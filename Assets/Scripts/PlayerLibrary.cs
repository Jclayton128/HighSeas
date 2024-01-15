using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLibrary : MonoBehaviour
{
    public static PlayerLibrary Instance { get; private set; }

    [SerializeField] Color[] _playerColors = null;
    [SerializeField] Sprite[] _playerIcon = null;
    [SerializeField] Sprite[] _ranks = null;
    [SerializeField] Sprite[] _panels = null;
    [SerializeField] string[] _shipNames = null;

    //state
    List<string> _shipNamesAvailable;

    private void Awake()
    {
        Instance = this;
        _shipNamesAvailable = new List<string>(_shipNames);
    }

    public Color GetPlayerColor(int playerIndex)
    {
        return _playerColors[playerIndex];
    }

    public string GetRandomAvailableShipName()
    {
        int rand = UnityEngine.Random.Range(0, _shipNamesAvailable.Count);
        string n = _shipNamesAvailable[rand];
        _shipNamesAvailable.Remove(n);
        return n;

    }

    public Sprite GetRank(int rank)
    {
        if (rank < _ranks.Length)
        {
            return _ranks[rank];
        }
        else return null;
    }
    public Sprite GetPanel(int playerIndex)
    {
        if (playerIndex < _panels.Length)
        {
            return _panels[playerIndex];
        }
        else return null;
    }
}
