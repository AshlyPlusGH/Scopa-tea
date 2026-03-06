using UnityEngine;

public class PLAYER_Pointer : MonoBehaviour
{
    [Header("Components: Automatically Enabled Locally")]
    public PLAYER_Inventory inventory;
    public PLAYER_PickupItem pickup;
    public PLAYER_Raycast raycaster;

    [Space(10)]

    [Header("Survivor Components: Automatically Enabled Locally. Use for Survivors")]
    public PLAYER_Survivor survivor;
    public PLAYER_SURVIVOR_Hitbox survivorHitbox;

    [Header("Monster Components: Automatically Enabled Locally. Use for Monsters")]
    public PLAYER_MONSTER_Attack monsterAttacker;
    public PLAYER_MONSTER_AttackHitBox monsterAttackHitbox;

    public void EnableAll()
    {
        inventory.enabled = true;
        pickup.enabled = true;
        raycaster.enabled = true;

        if (survivor != null) survivor.enabled = true;
        if (survivorHitbox != null) survivorHitbox.enabled = true;

        if (monsterAttacker != null) monsterAttacker.enabled = true;
        if (monsterAttackHitbox != null) monsterAttackHitbox.enabled = true;
    }
}