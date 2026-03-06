using UnityEngine;

public class TRIGGER_Escape : MonoBehaviour
{
    #region 2D
        // Enter
        void OnTriggerEnter2D(Collider2D other)
        {
            PLAYER_SURVIVOR_Hitbox survivorHitbox = other.gameObject.GetComponent<PLAYER_SURVIVOR_Hitbox>();
            if (survivorHitbox == null){ return; }
            PLAYER_Survivor survivor = survivorHitbox.pointer.survivor;
            if (survivor == null){ return; }
            survivor.Escaped();
        }
        void OnCollisionEnter2D(Collision2D other)
        {
            PLAYER_SURVIVOR_Hitbox survivorHitbox = other.gameObject.GetComponent<PLAYER_SURVIVOR_Hitbox>();
            if (survivorHitbox == null){ return; }
            PLAYER_Survivor survivor = survivorHitbox.pointer.survivor;
            if (survivor == null){ return; }
            survivor.Escaped();
        }
    #endregion

    #region 3D
        // Enter
        void OnTriggerEnter(Collider other)
        {
            PLAYER_SURVIVOR_Hitbox survivorHitbox = other.gameObject.GetComponent<PLAYER_SURVIVOR_Hitbox>();
            if (survivorHitbox == null){ return; }
            PLAYER_Survivor survivor = survivorHitbox.pointer.survivor;
            if (survivor == null){ return; }
            survivor.Escaped();
        }
        void OnCollisionEnter(Collision other)
        {
            PLAYER_SURVIVOR_Hitbox survivorHitbox = other.gameObject.GetComponent<PLAYER_SURVIVOR_Hitbox>();
            if (survivorHitbox == null){ return; }
            PLAYER_Survivor survivor = survivorHitbox.pointer.survivor;
            if (survivor == null){ return; }
            survivor.Escaped();
        }
    #endregion
}