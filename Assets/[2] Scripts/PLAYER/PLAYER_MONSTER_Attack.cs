using UnityEngine;
using PurrNet;

public class PLAYER_MONSTER_Attack : NetworkBehaviour
{
    public GameObject attacker;
    public Animator animator;

    private bool attacking;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !attacking){ Attack(); }
    }

    public void Attack(){attacking = true; attacker.SetActive(true); animator.SetTrigger("Attack");}

    public void OnAttackCompleted()
    {
        Debug.Log("Animation finished!");
        
        attacker.SetActive(false); attacking = false;
    }
}