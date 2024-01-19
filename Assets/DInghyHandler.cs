using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DInghyHandler : MonoBehaviour
{
    AIPath _ai;
    ShipHandler _targetShip;

    private void Awake()
    {
        _ai = GetComponent<AIPath>();
    }

    public void InitializeDinghy(ShipHandler targetShip)
    {
        _targetShip = targetShip;
        _ai.destination = _targetShip.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipHandler collShip;
        if (collision.TryGetComponent<ShipHandler>(out collShip))
        {
            if (collShip == _targetShip)
            {
                _targetShip.GetComponent<CrewHandler>().GainFullCrew();
            }
            Destroy(gameObject);
        }

    }

}


