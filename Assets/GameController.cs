using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameController : MonoBehaviour
{
    public Action GameModeStarted;
    public Action GameModeEnded;
    public static GameController Instance { get; private set; }

    public enum WheelOptions { EnterRegularGame0, AdjustSound1, AdjustMusic2, Credits3,
    Option4, Option5, Option6, Option7}

    //settings
    [SerializeField] int _coinsRequiredToWin = 25;
    int _winningActorIndex = -1;

    //state
    UIController.Context _currentContext = UIController.Context.Pretitle;
    public UIController.Context Context => _currentContext;
    [SerializeField] WheelOptions _currentWheelOption;
    public WheelOptions WheelOption => _currentWheelOption;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIController.Instance.SetContext(UIController.Context.Pretitle, 0.001f);
        //JUICE TODO have some clouds drift by, play ocean/pirate music
        Invoke(nameof(Delay_Start), 5f);
    }

    private void Delay_Start()
    {
        RequestNewGameContext(UIController.Context.Title);
    }

    [ContextMenu("start game")]
    public void StartGameplay()
    {
        if (RequestNewGameContext(UIController.Context.Gameplay)) GameModeStarted?.Invoke();
    }

    [ContextMenu("ENd game")]
    public void EndGame()
    {
        CameraController.Instance.SetZoom(CameraController.ZoomLevel.Close,
            ActorController.Instance.GetShipTransformOfActor(_winningActorIndex));
        CannonController.Instance.NullifyAllCannons();
        UIController.Instance.SetContext(UIController.Context.GameOver);
        GameModeEnded?.Invoke();
    }

    public bool RequestNewGameContext(UIController.Context gameContext)
    {
        if (UIController.Instance.IsSwappingContext) return false;
        switch (gameContext)
        {
            case UIController.Context.Pretitle:
                UIController.Instance.SetContext(UIController.Context.Pretitle);
                _currentContext = UIController.Context.Pretitle;
                break;

            case UIController.Context.Title:
                UIController.Instance.SetContext(UIController.Context.Title);
                _currentContext = UIController.Context.Title;
                break;

            case UIController.Context.Wheel:
                UIController.Instance.SetContext(UIController.Context.Wheel);
                _currentContext = UIController.Context.Wheel;
                break;

            case UIController.Context.Gameplay:
                UIController.Instance.SetContext(UIController.Context.Gameplay);
                _currentContext = UIController.Context.Gameplay;
                StartGameplay();
                break;
        }
        return true;
    }

    public void RequestWheelRotation(int stepDirection)
    {
        if (UIController.Instance.IsRotatingWheel) return;

        if (stepDirection > 0)
        {
            _currentWheelOption = (WheelOptions)UIController.Instance.IncreaseWheelAndGetCurrentStep();
        }
        else if (stepDirection < 0)
        {
            _currentWheelOption = (WheelOptions)UIController.Instance.DecreaseWheelAndGetCurrentStep();
        }

    }

    public void EnterCurrentWheelOption()
    {
        switch (_currentWheelOption)
        {
            case WheelOptions.EnterRegularGame0:
                StartGameplay();
                break;
        }
    }

    public void HandleCoinsGained(int actorIndex, int playerCurrentCoin)
    {
        if (playerCurrentCoin >= _coinsRequiredToWin)
        {
            //execute game over flow
            _winningActorIndex = actorIndex;
            EndGame();
        }
    }


    [ContextMenu("force player 0 win")]
    public void Debug_ForcePlayer0Win()
    {
        HandleCoinsGained(0, _coinsRequiredToWin+1);
    }
}
