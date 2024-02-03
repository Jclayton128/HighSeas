using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Action GameModeStarted;
    public Action GameModeEnded;
    public static GameController Instance { get; private set; }

    public enum WheelOptions { EnterRegularGame0, EnterTutorialGame1, Option2, Option3,
        Option4, Option5, Option6, Option7}

    //settings
    [SerializeField] int _coinsRequiredToWin = 75;
    [SerializeField] int _coinsRequiredToSummonPirate_0 = 20;
    [SerializeField] int _coinsRequiredToSummonPirate_1 = 35;
    [SerializeField] int _coinsRequiredToSummonPirate_2 = 50;
    [SerializeField] int _coinsRequiredToSummonPirate_3 = 60;
    [SerializeField] int _winningActorIndex = -1;
    [SerializeField] string _pressDownToStart = "Press Down";
    [SerializeField] string _leftRightToSelect = "Left/Right To Rotate, Down To Select";
    [SerializeField] string _startNewGame = $"Multiplayer Game (Race to 50 Coins)";
    [SerializeField] string _startTutorial = $"Single Player Tutorial";
    [SerializeField] string _placeholderOption = $"Placeholder option...";
    [SerializeField] string _adjustSound = $"Left/Right to Adjust Sound, Up to return";
    [SerializeField] string _adjustMusic = $"Left/Right to Adjust Music, Up to return";
    [SerializeField] string _viewCredits = $"Credits go here. Up to return";

    //state
    [SerializeField] UIController.Context _currentContext = UIController.Context.Pretitle;
    public UIController.Context Context => _currentContext;
    [SerializeField] WheelOptions _currentWheelOption = WheelOptions.EnterTutorialGame1;
    public WheelOptions WheelOption => _currentWheelOption;
    bool _hasSummonedPirate_0 = false;
    bool _hasSummonedPirate_1 = false;
    bool _hasSummonedPirate_2 = false;
    bool _hasSummonedPirate_3 = false;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIController.Instance.SetContext(UIController.Context.Pretitle, 0.001f);
        //JUICE TODO have some clouds drift by, play ocean/pirate music
        Invoke(nameof(Delay_Start), 3f);
    }

    private void Delay_Start()
    {
        RequestNewGameContext(UIController.Context.Title);
    }

    private void StartGameplay()
    {
        //TODO turn off tutorials
        if (RequestNewGameContext(UIController.Context.Gameplay)) GameModeStarted?.Invoke();
    }

    private void StartTutorialGameplay()
    {
        _coinsRequiredToWin = 10;
        TutorialController.Instance.StartTutorial();
        if (RequestNewGameContext(UIController.Context.Gameplay)) GameModeStarted?.Invoke();
    }

    private void EndGame()
    {
        _currentContext = UIController.Context.GameOver;
        CameraController.Instance.SetZoom(CameraController.ZoomLevel.Close,
            ActorController.Instance.GetShipTransformOfActor(_winningActorIndex));
        CannonController.Instance.NullifyAllCannons();
        UIController.Instance.SetContext(UIController.Context.GameOver);
        StatsController.Instance.SetWinningPlayerData(_winningActorIndex);
        GameModeEnded?.Invoke();
    }

    public void RequestReloadAfterGameover(int requestingActor)
    {
        //Debug.Log($"{requestingActor} wants to reload. winner was {_winningActorIndex}");
        if (requestingActor != _winningActorIndex)
        {
            Debug.Log("Leaving game over screen can only be done by winning player");
            return;
        }
        SceneManager.LoadScene(0);
    }

    public bool RequestNewGameContext(UIController.Context gameContext)
    {
        if (UIController.Instance.IsSwappingContext) return false;
        switch (gameContext)
        {
            case UIController.Context.Pretitle:
                //UIController.Instance.SetContext(UIController.Context.Pretitle);
                //_currentContext = UIController.Context.Pretitle;
                break;

            case UIController.Context.Title:
                UIController.Instance.SetContext(UIController.Context.Title);
                _currentContext = UIController.Context.Title;
                Invoke(nameof(Delay_EnterTitleContext), 2f);
                break;

            case UIController.Context.Wheel:
                UIController.Instance.SetContext(UIController.Context.Wheel);
                _currentContext = UIController.Context.Wheel;
                UIController.Instance.SetHelperText(_leftRightToSelect);
                break;

            case UIController.Context.Gameplay:
                UIController.Instance.SetContext(UIController.Context.Gameplay);
                _currentContext = UIController.Context.Gameplay;
                StartGameplay();
                break;
        }
        return true;
    }

    private void Delay_EnterTitleContext()
    {
        UIController.Instance.SetHelperText(_pressDownToStart);
    }

    public void RequestWheelRotation(int stepDirection)
    {
        //if (UIController.Instance.IsRotatingWheel) return;

        if (stepDirection > 0)
        {
            _currentWheelOption = (WheelOptions)UIController.Instance.IncreaseWheelAndGetCurrentStep();
        }
        else if (stepDirection < 0)
        {
            _currentWheelOption = (WheelOptions)UIController.Instance.DecreaseWheelAndGetCurrentStep();
        }
        PushWheelTextToUI();
        SoundController.Instance.PlayClip(SoundLibrary.SoundID.WheelCreak7);
    }

    private void PushWheelTextToUI()
    {
        string text;
        switch (_currentWheelOption)
        {
            case WheelOptions.EnterRegularGame0:
                text = _startNewGame;
                break;
            case WheelOptions.EnterTutorialGame1:
                text = _startTutorial;
                break;

            case WheelOptions.Option2:
                text = _placeholderOption;
                break;

            case WheelOptions.Option3:
                text = _placeholderOption;
                break;

            case WheelOptions.Option4:
                text = _placeholderOption;
                break;

            case WheelOptions.Option5:
                text = _placeholderOption;
                break;

            case WheelOptions.Option6:
                text = _placeholderOption;
                break;

            case WheelOptions.Option7:
                text = _placeholderOption;
                break;

            default: text = _placeholderOption;

                break;
        }

        UIController.Instance.SetHelperText(text);
    }

    public void EnterCurrentWheelOption()
    {
        if (UIController.Instance.IsSwappingContext) return;
        switch (_currentWheelOption)
        {
            case WheelOptions.EnterRegularGame0:
                StartGameplay();
                break;

            case WheelOptions.EnterTutorialGame1:
                StartTutorialGameplay();
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

        if (playerCurrentCoin > _coinsRequiredToSummonPirate_0 && !_hasSummonedPirate_0)
        {
            _hasSummonedPirate_0 = true;
            ActorController.Instance.SpawnPirate();
        }
        if (playerCurrentCoin > _coinsRequiredToSummonPirate_1 && !_hasSummonedPirate_1)
        {
            _hasSummonedPirate_1 = true;
            ActorController.Instance.SpawnPirate();
        }
        if (playerCurrentCoin > _coinsRequiredToSummonPirate_2 && !_hasSummonedPirate_2)
        {
            _hasSummonedPirate_2 = true;
            ActorController.Instance.SpawnPirate();
        }
        if (playerCurrentCoin > _coinsRequiredToSummonPirate_3 && !_hasSummonedPirate_3)
        {
            _hasSummonedPirate_3 = true;
            ActorController.Instance.SpawnPirate();
        }


    }


    [ContextMenu("force player 0 win")]
    public void Debug_ForcePlayer0Win()
    {
        HandleCoinsGained(0, _coinsRequiredToWin+1);
    }
}
