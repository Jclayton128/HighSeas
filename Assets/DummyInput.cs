using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInput : MonoBehaviour
{
    [SerializeField] DestinationHandler _targetDestinationHandler = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _targetDestinationHandler.CommandMove(0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _targetDestinationHandler.CommandMove(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _targetDestinationHandler.CommandMove(2);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _targetDestinationHandler.CommandMove(3);
        }
    }
}
