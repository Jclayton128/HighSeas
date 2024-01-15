using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance { get; private set; }

    [SerializeField] GameObject _debugPanel;

    bool _isInDebugMode = false;
    public bool IsInDebugMode => _isInDebugMode;


    private void Awake()
    {
        Instance = this;
        _debugPanel.SetActive(_isInDebugMode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _isInDebugMode = !_isInDebugMode;
            _debugPanel.SetActive(_isInDebugMode);
        }

        if (_isInDebugMode && Input.GetKeyDown(KeyCode.M))
        {
            ActorController.Instance.Debug_GiveAllPlayers10Coins();
        }
        if (_isInDebugMode && Input.GetKeyDown(KeyCode.C))
        {
            CityController.Instance.Debug_DevelopCities();
        }
        if (_isInDebugMode && Input.GetKeyDown(KeyCode.S))
        {
            SmithController.Instance.Debug_DevelopSmiths();
        }
    }
}
