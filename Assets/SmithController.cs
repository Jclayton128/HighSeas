using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithController : MonoBehaviour
{
   

    public static SmithController Instance { get; private set; }

    //state
    int _indexer = 0;
    List<SmithHandler> _smiths = new List<SmithHandler>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterSmith(SmithHandler smith)
    {
        _smiths.Add(smith);
    }
    public SmithLibrary.SmithType GetNextSmithType()
    {
        var smithType = (SmithLibrary.SmithType)_indexer;
        _indexer++;
        if (_indexer >= (int)CargoLibrary.CargoType.Empty) _indexer = 0;
        return smithType;
    }

    public void Debug_DevelopSmiths()
    {
        foreach (var smith in _smiths)
        {
            smith.Debug_DevelopSmith();
        }
    }
}
