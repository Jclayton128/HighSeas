using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public static CannonController Instance { get; private set; }

    [SerializeField] CannonballHandler _cannonballPrefab = null;

    //state
    float _cannonballSpeed = 2f;
    public float CannonballSpeed => _cannonballSpeed;
     List<CannonballHandler> _activeCannonBalls = new List<CannonballHandler>();
    Queue<CannonballHandler> _pooledCannonBalls = new Queue<CannonballHandler>();
    List<CannonHandler> _activeCannons = new List<CannonHandler>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterCannon(CannonHandler newCannon)
    {
        _activeCannons.Add(newCannon);
    }

    public void NullifyAllCannons()
    {
        for (int i = 0; i < _activeCannons.Count; i++)
        {
            _activeCannons[i].NullifyCannon();
        }
    }

    public CannonballHandler RequisitionCannonball()
    {
        CannonballHandler cannonball;
        if (_pooledCannonBalls.Count > 0)
        {
            cannonball = _pooledCannonBalls.Dequeue();
            cannonball.gameObject.SetActive(true);
        }
        else
        {
            cannonball = Instantiate(_cannonballPrefab);
            cannonball.InitializeCannonball();
            _activeCannonBalls.Add(cannonball);
        }
        return cannonball;
    }

    public void ReturnCannonball(CannonballHandler unneededCannonball)
    {
        _pooledCannonBalls.Enqueue(unneededCannonball);
        _activeCannonBalls.Remove(unneededCannonball);
        unneededCannonball.gameObject.SetActive(false);
    }

    
}
