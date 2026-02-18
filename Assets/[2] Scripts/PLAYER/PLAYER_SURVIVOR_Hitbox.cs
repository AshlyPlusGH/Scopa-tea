using UnityEngine;

public class PLAYER_SURVIVOR_Hitbox : MonoBehaviour
{
    public GameObject playerParent;

    public void Death(){ Destroy(playerParent); }
}