using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A utility class for working with LayerMask operations in Unity,
/// providing methods to check if a GameObject is in a LayerMask and
/// to create custom LayerMasks from a list of layers.
/// </summary>
public class LayerMaskHelper : MonoBehaviour
{
    /// <summary>
    /// Checks if a specified GameObject is part of a given LayerMask.
    /// </summary>
    /// <param name="gameObject">The GameObject to check.</param>
    /// <param name="layerMask">The LayerMask to check against.</param>
    /// <returns>Returns true if the GameObject's layer is in the LayerMask, otherwise false.</returns>
    public static bool IsObjectInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        if ((layerMask.value & (1 << gameObject.layer)) > 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Creates a LayerMask from an array of layer indices.
    /// </summary>
    /// <param name="layers">An array of layer indices to include in the LayerMask.</param>
    /// <returns>Returns a LayerMask that contains the specified layers.</returns>
    public static LayerMask CreateLayerMask(params int[] layers)
    {
        LayerMask layerMask = 0;
        foreach (int layer in layers)
        {
            layerMask |= (1 << layer);
        }

        return layerMask;
    }
}
