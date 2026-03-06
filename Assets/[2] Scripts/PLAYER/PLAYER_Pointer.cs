using UnityEngine;

public class PLAYER_Pointer : MonoBehaviour
{
    [Header("Components: Automatically Enabled Locally")]
    public PLAYER_Inventory inventory;
    public PLAYER_PickupItem pickup;
    public PLAYER_Raycast raycaster;

    [Space(10)]

    [Header("Survivor Components: Automatically Enabled with survivors")]
    public PLAYER_Survivor survivor;
    public PLAYER_SURVIVOR_Hitbox survivorHitbox;

    public void EnableAll()
    {
        inventory.enabled = true;
        pickup.enabled = true;
        raycaster.enabled = true;

        if (survivor != null) survivor.enabled = true;
        if (survivorHitbox != null) survivorHitbox.enabled = true;
    }
}