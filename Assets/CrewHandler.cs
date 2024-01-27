using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CrewHandler : MonoBehaviour
{
    public Action<int> CrewCountChanged;
    public Action CrewCountAtZero;

    [SerializeField] ParticleSystem _ps = null;

    //state
    [SerializeField] int _currentCrew =5 ;
    [SerializeField] int _maxCrew = 5;
    private void Start()
    {
        
    }

    public void GainOneCrew()
    {
        _currentCrew++;
        _currentCrew = Mathf.Clamp(_currentCrew, 1, 5);
        CrewCountChanged?.Invoke(_currentCrew);
    }

    public void GainFullCrew()
    {
        _currentCrew = _maxCrew;
        SoundController.Instance.PlayClip(SoundLibrary.SoundID.CrewRevive8);
        CrewCountChanged?.Invoke(_currentCrew);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CannonballHandler ch;
        if (collision.TryGetComponent<CannonballHandler>(out ch))
        {
            //JUICE TODO: emit crew-images as particle and wood bits.
            //JUICE TODO: audio of cannonball impact. Or should it live with Cannonball?

            _currentCrew--;
            SoundController.Instance.PlayClip(SoundLibrary.SoundID.CrewScream1);
            _ps.Emit(1);
            CrewCountChanged?.Invoke(_currentCrew); 

            if (_currentCrew <= 0)
            {
                CrewCountAtZero?.Invoke();
            }

            ch.TerminateCannonball();
        }
    }
}
