using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    public enum SoundID { CannonFire0, CrewScream1, EnterPort2,
    LoadCargo3, SellCargo4, StartUpgrade5, FinishUpgrade6, WheelCreak7, CrewRevive8,
    GameOverTriumph9
    }

    public static SoundLibrary Instance { get; private set; }

    [SerializeField] AudioClip[] _cannonFires = null;
    [SerializeField] AudioClip[] _crewScreams = null;
    [SerializeField] AudioClip[] _enterPort = null;
    [SerializeField] AudioClip[] _loadCargo = null;
    [SerializeField] AudioClip[] _sellCargo = null;
    [SerializeField] AudioClip[] _startUpgrade = null;
    [SerializeField] AudioClip[] _finishUpgrade = null;
    [SerializeField] AudioClip[] _wheelCreak = null;
    [SerializeField] AudioClip[] _crewRevive = null;
    [SerializeField] AudioClip[] _gameOverTriumph = null;


    private void Awake()
    {
        Instance = this;
    }

    public AudioClip GetClip(SoundID typeOfClip)
    {
        return GetClip(typeOfClip, -1);
    }

    public AudioClip GetClip(SoundID typeOfClip,int forcedIndex)
    {
        int rand;
        switch (typeOfClip)
        {
            case SoundID.CannonFire0:
                if (forcedIndex >= 0 && forcedIndex < _cannonFires.Length)
                {
                    return _cannonFires[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _cannonFires.Length);
                    return _cannonFires[rand];
                }


            case SoundID.CrewScream1:
                if (forcedIndex >= 0 && forcedIndex < _crewScreams.Length)
                {
                    return _crewScreams[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _crewScreams.Length);
                    return _crewScreams[rand];
                }

            case SoundID.EnterPort2:
                if (forcedIndex >= 0 && forcedIndex < _enterPort.Length)
                {
                    return _enterPort[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _enterPort.Length);
                    return _enterPort[rand];
                }

            case SoundID.LoadCargo3:
                if (forcedIndex >= 0 && forcedIndex < _loadCargo.Length)
                {
                    return _loadCargo[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _loadCargo.Length);
                    return _loadCargo[rand];
                }

            case SoundID.SellCargo4:
                if (forcedIndex >= 0 && forcedIndex < _sellCargo.Length)
                {
                    return _sellCargo[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _sellCargo.Length);
                    return _sellCargo[rand];
                }

            case SoundID.StartUpgrade5:
                if (forcedIndex >= 0 && forcedIndex < _startUpgrade.Length)
                {
                    return _startUpgrade[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _startUpgrade.Length);
                    return _startUpgrade[rand];
                }

            case SoundID.FinishUpgrade6:
                if (forcedIndex >= 0 && forcedIndex < _finishUpgrade.Length)
                {
                    return _finishUpgrade[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _finishUpgrade.Length);
                    return _finishUpgrade[rand];
                }

            case SoundID.WheelCreak7:
                if (forcedIndex >= 0 && forcedIndex < _wheelCreak.Length)
                {
                    return _wheelCreak[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _wheelCreak.Length);
                    return _wheelCreak[rand];
                }

            case SoundID.CrewRevive8:
                if (forcedIndex >= 0 && forcedIndex < _crewRevive.Length)
                {
                    return _crewRevive[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _crewRevive.Length);
                    return _crewRevive[rand];
                }

            case SoundID.GameOverTriumph9:
                if (forcedIndex >= 0 && forcedIndex < _gameOverTriumph.Length)
                {
                    return _gameOverTriumph[forcedIndex];
                }
                else
                {
                    rand = UnityEngine.Random.Range(0, _gameOverTriumph.Length);
                    return _gameOverTriumph[rand];
                }






            default: return _cannonFires[0];

        }

    }
}
