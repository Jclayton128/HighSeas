using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithLibrary : MonoBehaviour
{
    public static SmithLibrary Instance { get; private set; }
    public enum SmithType { Sails, Cargo, Cannon }

    /// <summary>
    /// 0: Sails, 1: Cargo, 2: Cannon
    /// </summary>
    [Tooltip("0: Sails, 1: Cargo, 2: Cannon")]
    [SerializeField] Sprite[] _upgradeSprites = null;


    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetUpgradeSprite(SmithType smithType)
    {
        return _upgradeSprites[(int)smithType];
    }
}
