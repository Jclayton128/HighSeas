using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public static int TilesTraversable = 6;
    public static int TilesNonTraversable = 7;

    public static int LayerMask_AllTiles = (1 << TilesTraversable) | (1 << TilesNonTraversable);
    public static int LayerMask_TilesTraversable = (1 << TilesTraversable);
    public static int LayerMask_TilesNonTraversable = (1 << TilesNonTraversable);

}
