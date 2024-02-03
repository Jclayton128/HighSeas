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
    [SerializeField] Sprite[] _crewSprites = null;
    [SerializeField] Sprite _blankCrew = null;
    [SerializeField] Sprite[] _blueSails = null;
    [SerializeField] Sprite[] _greenSails = null;
    [SerializeField] Sprite[] _redSails = null;
    [SerializeField] Sprite[] _yellowSails = null;
    [SerializeField] Sprite[] _blackSails = null;
    [SerializeField] Sprite[] _baseShipsByCannon = null;

    [SerializeField] AI_Simple _ai = null;

    //state
    List<string> _shipNamesAvailable;

    private void Awake()
    {
        Instance = this;
        _shipNamesAvailable = new List<string>(_shipNames);
    }

    public Color GetPlayerColor(int playerIndex)
    {
        if (playerIndex < 0) return _playerColors[4];
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

    public Sprite GetCrewSprite(int playerIndex)
    {
        if (playerIndex < 0) return _blankCrew;
        return _crewSprites[playerIndex];
    }

    public Sprite GetSailSprite(int playerIndex, int sailLevel)
    {
        switch (playerIndex)
        {
            case 0:
                return _blueSails[sailLevel];
            case 1:
                return _greenSails[sailLevel];
            case 2:
                return _redSails[sailLevel];
            case 3:
                return _yellowSails[sailLevel];

            case 4:
            case 5:
            case 6:
            case 7:
                //Debug.Log("blacksaid");
                return _blackSails[sailLevel];

            default: return null;

        }
    }

    public Sprite GetShipBaseSpriteByCannon(int cannonLevel)
    {
        return _baseShipsByCannon[cannonLevel];
    }

    public AI_Simple GetAI()
    {
        return _ai;
    }
}

