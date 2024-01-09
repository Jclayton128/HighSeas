using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoLibrary : MonoBehaviour
{

    public static CargoLibrary Instance { get; private set; }

    public enum CargoType { Cargo0, Cargo1, Cargo2, Cargo3, Count }
    [SerializeField] Sprite[] _cargoSprites = null;
    [SerializeField] Color[] _cargoColors = null;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetCargoSprite(int cargoTypeAsInt)
    {
        return _cargoSprites[cargoTypeAsInt];
    }

    public Color GetCargoColor(int cargoTypeAsInt)
    {
        return _cargoColors[cargoTypeAsInt];
    }
}
