using UnityEngine;
using PurrNet;

public class PLAYER_MONSTER_Attack : NetworkBehaviour
{
    public PLAYER_Pointer pointer;

    [SerializeField] public GameObject attacker;
    [SerializeField] public Animator animator;

    private bool attacking;

    void Update()
    {
        if (attacking){ return; }
        if (!Input.GetMouseButtonDown(0)){ return; }

        GameObject observedObject = pointer.raycaster.GetObservedObject(PLAYER_PickupItem.RULE_PLAYER_pickupRange);
        if (observedObject != null){ if (observedObject.GetComponent<ITEM_Pickup>() != null){ return; }} // Check for Items in Front of player to prevent attacking!

        Attack();
    }

    public void Attack()
    {
        if (!isOwner){ return; }

        RPC_UPDATESERVER_Attack();
    }
        [ServerRpc]
        private void RPC_UPDATESERVER_Attack(){ RPC_UPDATECLIENTS_Attack(); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_Attack(){ ACTION_Attack(); }
        private void ACTION_Attack()
        {
            attacking = true;
            
            attacker.SetActive(true);
            
            animator.SetTrigger("Attack");
        }

        public void OnAttackCompleted()
        {
            attacker.SetActive(false);

            attacking = false;
        }
}