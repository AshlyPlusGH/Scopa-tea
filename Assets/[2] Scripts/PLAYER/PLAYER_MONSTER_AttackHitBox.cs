using NaughtyAttributes;
using UnityEngine;

public class PLAYER_MONSTER_AttackHitBox : MonoBehaviour
{
    private bool hasEntered = false;

    [Tag] public string survivorTag;

    void OnEnable()
    {
        hasEntered = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hasEntered && other.CompareTag(survivorTag))
        {
            hasEntered = true;
            OnPlayerHitBoxEntered(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(survivorTag))
        {
            hasEntered = false;
        }
    }

    private void OnPlayerHitBoxEntered(GameObject player)
    {
        Debug.Log("Triggered method executed.");

        player.GetComponent<PLAYER_SURVIVOR_Hitbox>().Death();
    }

}