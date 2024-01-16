using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballHandler : MonoBehaviour
{

    //references
    SpriteRenderer _sr;
    Collider2D _coll;


    //instance state
    float _remainingLifetime;
    Vector3 _velocity;
    bool _isActive = false;

    public void InitializeCannonball()
    {
        _sr = GetComponent<SpriteRenderer>();
        _coll = GetComponent<Collider2D>();
    }

    public void SetupCannonballInstance(Vector3 spawnPosition, Vector3 targetPosition)
    {
        _sr.enabled = true;
        _coll.enabled = true;

        transform.position = spawnPosition;
        Vector3 spread = (targetPosition - spawnPosition);
        Vector3 dirToTarget = spread.normalized;
        float distToTarget = spread.magnitude;
        _velocity = dirToTarget * CannonController.Instance.CannonballSpeed;
        _remainingLifetime = distToTarget / CannonController.Instance.CannonballSpeed;
        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive) return;

        transform.position += _velocity * Time.deltaTime;
        _remainingLifetime -= Time.deltaTime;
        if (_remainingLifetime <= 0)
        {
            TerminateCannonball();
        }

    }

    private void TerminateCannonball()
    {
        _isActive = false;
        _sr.enabled = false;
        _coll.enabled = false;
        //JUICE TODO check for tile tile and emit particle splash;
        Invoke(nameof(Delay_TerminateCannonball), 2f);
    }

    private void Delay_TerminateCannonball()
    {
        CannonController.Instance.ReturnCannonball(this);
    }
}
