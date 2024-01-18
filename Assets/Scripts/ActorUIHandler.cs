using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ActorUIHandler : MonoBehaviour
{
    //ref
    [SerializeField] TextMeshProUGUI _playerNameTMP = null;
    [SerializeField] TextMeshProUGUI _coinCountTMP = null;
    [SerializeField] Image _playerRank = null;
    [SerializeField] Image[] _cargoSlots = null;
    [SerializeField] Image _panel = null;
    [SerializeField] Image[] _crews = null;

    //state
    ActorHandler _actor;

    private void Start()
    {
        _playerNameTMP.text = "N/A";
        HandleRankUpdated(0);
        HandleUpdatedCoinCount(0);
        HandleUpdatedSlots(null, 0);
        HandleUpdatedCrew(0);
    }


    public void AssignActor(ActorHandler actor, int playerIndex)
    {
        _actor = actor;

        _panel.sprite = PlayerLibrary.Instance.GetPanel(playerIndex);
        _playerNameTMP.text = PlayerLibrary.Instance.GetRandomAvailableShipName();
        _playerRank.sprite = PlayerLibrary.Instance.GetRank(0);

        HandleUpdatedCoinCount(0);

        actor.ActorCoinCountUpdated += HandleUpdatedCoinCount;
        actor.ActorRankUpdated += HandleRankUpdated;
        actor.ActorCargoSlotsUpdated += HandleUpdatedSlots;
        actor.ActorCrewUpdated += HandleUpdatedCrew;

    }



    private void OnDestroy()
    {
        _actor.ActorCoinCountUpdated -= HandleUpdatedCoinCount;
        _actor.ActorRankUpdated -= HandleRankUpdated;
        _actor.ActorCargoSlotsUpdated -= HandleUpdatedSlots;
        _actor.ActorCrewUpdated -= HandleUpdatedCrew;
    }

    private void HandleRankUpdated(int obj)
    {
        _playerRank.sprite = PlayerLibrary.Instance.GetRank(obj);
    }

    private void HandleUpdatedSlots(List<CargoLibrary.CargoType> cargos, int openSlots)
    {
        if (cargos.Count > _cargoSlots.Length)
        {
            Debug.LogWarning("more cargos than UI can render!");
        }
        for (int i =0; i < _cargoSlots.Length; i++)
        {
            if (i < openSlots && cargos != null)
            {
                if (i >= cargos.Count)
                {
                    _cargoSlots[i].sprite = null;
                    //_cargoSlots[i].sprite = CargoLibrary.Instance.GetCargoSprite(cargos[i]);
                }
                else
                {
                    _cargoSlots[i].sprite = CargoLibrary.Instance.GetCargoSprite(cargos[i]);
                }

            }
            else _cargoSlots[i].sprite = CargoLibrary.Instance.GetCargoSprite(CargoLibrary.CargoType.Blocked);
        }
    }
    private void HandleUpdatedCrew(int crewCount)
    {
        for (int i = 0; i < _crews.Length; i++)
        {
            if (i < crewCount) _crews[i].sprite = PlayerLibrary.Instance.GetCrewSprite(_actor.ActorIndex);
            else _crews[i].sprite = PlayerLibrary.Instance.GetCrewSprite(-1);
        }
    }

    private void HandleUpdatedCoinCount(int obj)
    {
        _coinCountTMP.text = obj.ToString();
    }
}
