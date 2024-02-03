using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CloudHandler : MonoBehaviour
{
    SpriteRenderer _sr;
    [SerializeField] Sprite[] _cloudSprites = null;

    Tween _driftTween;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        int rand = UnityEngine.Random.Range(0, _cloudSprites.Length);
        _sr.sprite = _cloudSprites[rand];
        float ang = UnityEngine.Random.Range(-180f, 180f);
        transform.rotation = Quaternion.Euler(0, 0, ang);
        transform.localScale = Vector3.one * UnityEngine.Random.Range(0.8f, 1.2f);
    }

    private void Start()
    {
        DriftAway();
    }

    public void DriftAway()
    {
        float delta = (9- Mathf.Abs(transform.position.y) )/ 4f;
        float dir = Mathf.Sign(transform.position.x);
        _driftTween = transform.DOMoveX(dir * 22, 4f).SetEase(Ease.InSine).SetDelay(delta);
        _sr.DOFade(0, 3f).SetEase(Ease.OutSine).SetDelay(1.0f);
    }

}
