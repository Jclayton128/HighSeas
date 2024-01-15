using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoLibrary : MonoBehaviour
{

    public static CargoLibrary Instance { get; private set; }

    public enum CargoType { Cargo0, Cargo1, Cargo2, Cargo3, Empty, Blocked }
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
    public Sprite GetCargoSprite(CargoType cargoTypeAsInt)
    {
        return _cargoSprites[(int)cargoTypeAsInt];
    }

    public Color GetCargoColor(int cargoTypeAsInt)
    {
        return _cargoColors[cargoTypeAsInt];
    }
}
