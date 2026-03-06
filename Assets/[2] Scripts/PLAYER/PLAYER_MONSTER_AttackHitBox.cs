using UnityEngine;

public class PLAYER_MONSTER_AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PLAYER_SURVIVOR_Hitbox survivorHitbox = other.GetComponent<PLAYER_SURVIVOR_Hitbox>();
            if (survivorHitbox == null){ return; }

        KillSurvivor(survivorHitbox.pointer.survivor);
    }

    private void KillSurvivor(PLAYER_Survivor survivor){ survivor.Death(); }

}