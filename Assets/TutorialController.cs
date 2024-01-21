using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance { get; private set; }

    [SerializeField] TutorialStep[] _tutorialSteps = null;
    [SerializeField] Image _tutorialBackground = null;
    [SerializeField] Image[] _tutorialImages = null;
    [SerializeField] TextMeshProUGUI _tutorialTMP = null;
    [SerializeField] TutorialHandler _tutorialDudePrefab = null;

    //state
    [SerializeField] int _currentTutorialIndex = -1;
    bool _isInTutorialMode = false;
    public bool IsInTutorialMode => _isInTutorialMode;
    float _timeToPushTutorial = 0;
    TutorialStep.ClearingOptions _currentClearingOption;
    float _timeToAutoClear;
    TutorialHandler _currentTutorialDude;
    TutorialStep _currentTutStep;


    private void Awake()
    {
        Instance = this;
        _tutorialBackground.CrossFadeAlpha(0, 0.0001f, false);
        foreach (var image in _tutorialImages)
        {
            image.CrossFadeAlpha(0, 0.0001f, false);
        }
        _tutorialTMP.text = "";
    }

    public void StartTutorial()
    {
        _isInTutorialMode = true;
        AdvanceTutorial();
    }

    private void Update()
    {
        if (!_isInTutorialMode) return;
        if (Time.time >= _timeToPushTutorial)
        {
            _timeToPushTutorial = Mathf.Infinity;
            PushCurrentTutorialStep();
        }

        if (Time.time >= _timeToAutoClear)
        {
            AdvanceTutorial();
        }
    }

    private void PushCurrentTutorialStep()
    {
        _tutorialBackground.CrossFadeAlpha(1, 1f, true);
        _tutorialTMP.text = _currentTutStep.TutorialText;

        Sprite[] sprites = _currentTutStep.Sprites;

        foreach (var image in _tutorialImages)
        {
            image.CrossFadeAlpha(0, 0.0001f, false);
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            _tutorialImages[i].sprite = sprites[i];
            _tutorialImages[i].CrossFadeAlpha(1, 1f, true);
        }

        if (_currentTutStep.SpecialOption == TutorialStep.SpecialOptions.ZoomOnPlayer)
        {
            CameraController.Instance.SetZoom(CameraController.ZoomLevel.Close,
                ActorController.Instance.GetShipTransformOfActor(0));
        }
        else if (_currentTutStep.SpecialOption == TutorialStep.SpecialOptions.ZoomOnFirstCity)
        {
            CameraController.Instance.SetZoom(CameraController.ZoomLevel.Close,
               CityController.Instance.GetTransformOfCity(0));
        }
        else if(_currentTutStep.SpecialOption == TutorialStep.SpecialOptions.ZoomOnFirstSmith)
        {
            CameraController.Instance.SetZoom(CameraController.ZoomLevel.Close,
                SmithController.Instance.GetTransformOfSmith(0));
        }
        else if (_currentTutStep.SpecialOption == TutorialStep.SpecialOptions.ZoomOnFirstSmith)
        {
            Debug.LogWarning("Castle step not implemented!");
        }
        else
        {
            CameraController.Instance.SetZoom(CameraController.ZoomLevel.Normal, null);
        }

        if (_currentTutStep.HasTutorialDude)
        {
            Destroy(_currentTutorialDude.gameObject);
            _currentTutorialDude = Instantiate(_tutorialDudePrefab, _currentTutStep.TutorialDudeLocation, Quaternion.identity);
        }

    }

    public void HandleTutorialDudeContact()
    {
        if (_currentClearingOption == TutorialStep.ClearingOptions.ContactTutorialDude)
        {
            AdvanceTutorial();
        }
    }

    public void AdvanceTutorial()
    {
        _currentTutorialIndex++;
        _currentTutStep = _tutorialSteps[_currentTutorialIndex];

        _timeToPushTutorial = Time.time +
            _currentTutStep.FrontSideWaitTime;

        _currentClearingOption = _currentTutStep.ClearingOption;
        if (_currentClearingOption == TutorialStep.ClearingOptions.TimeDelay)
        {
            _timeToAutoClear = Time.time +
                _currentTutStep.TimeDelayForAutoClear;
        }
        else
        {
            _timeToAutoClear = Mathf.Infinity;
        }
    }
}
