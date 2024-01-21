using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial Step")]
public class TutorialStep : ScriptableObject
{
    public enum SpecialOptions { None, ZoomOnPlayer, ZoomOnFirstCity,
        ZoomOnFirstSmith, ZoomOnCastle}

    public enum ClearingOptions { TimeDelay, ContactTutorialDude, PressDown};

    [Multiline(3)] [SerializeField] string _tutorialText = "default text";
    [SerializeField] Sprite[] _sprites = null;
    [SerializeField] bool _hasTutorialDude = false;
    public bool HasTutorialDude => _hasTutorialDude;
    [SerializeField] Vector3 _tutorialDudeLocation = Vector3.zero;
    public Vector3 TutorialDudeLocation => _tutorialDudeLocation;
    [SerializeField] float _frontsideWaitTime = 0f;
    public float FrontSideWaitTime => _frontsideWaitTime;

    public string TutorialText => _tutorialText;
    public Sprite[] Sprites => _sprites;
    [SerializeField] ClearingOptions _clearingOption = ClearingOptions.ContactTutorialDude;
    public ClearingOptions ClearingOption => _clearingOption;

    [SerializeField] float _timeDelayForAutoClear = 5f;
    public float TimeDelayForAutoClear => _timeDelayForAutoClear;

    [SerializeField] SpecialOptions _specialOption = SpecialOptions.None;
    public SpecialOptions SpecialOption => _specialOption;

}
