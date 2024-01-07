using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLibrary : MonoBehaviour
{
    public static TileLibrary Instance { get; private set; }

    //settings
    [SerializeField] Sprite[] _waterSprites = null;
    [SerializeField] Sprite[] _landSprites = null;
    [SerializeField] Sprite[] _pierSprites = null;
    [SerializeField] Sprite[] _citySprites = null;



    private void Awake()
    {
        Instance = this;
    }


    [ExecuteInEditMode]
    public Sprite GetRandomWaterSprite()
    {
        int rand = UnityEngine.Random.Range(0, _waterSprites.Length);
        return _waterSprites[rand];
    }

    [ExecuteInEditMode]
    public Sprite GetRandomLandSprite()
    {
        int rand = UnityEngine.Random.Range(0, _landSprites.Length);
        return _landSprites[rand];

    }
    public Sprite GetRandomPierSprite()
    {
        int rand = UnityEngine.Random.Range(0, _pierSprites.Length);
        return _pierSprites[rand];

    }
    public Sprite GetRandomCitySprite()
    {
        int rand = UnityEngine.Random.Range(0, _citySprites.Length);
        return _citySprites[rand];

    }
}
