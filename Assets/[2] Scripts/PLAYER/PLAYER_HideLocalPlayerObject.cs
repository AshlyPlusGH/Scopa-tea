using PurrNet;
using UnityEngine;
using NaughtyAttributes;

public class PLAYER_HideLocalPlayerObject : NetworkBehaviour
{
    [Layer] public int hiddenLayer;

    protected override void OnSpawned(){ base.OnSpawned(); if (isOwner){ SetLayers(); }}

    void SetLayers()
    {
        SetLayerRecursively(gameObject, hiddenLayer);
    }

    void SetLayerRecursively(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;

        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}