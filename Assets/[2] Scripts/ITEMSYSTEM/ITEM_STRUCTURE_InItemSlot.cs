using UnityEngine;
using PurrNet;
using System.Collections;
using System;

public class ITEM_STRUCTURE_InItemSlot : NetworkBehaviour
{
    public bool debug;

    [Space(10)]

    [SerializeField] public ITEM_Pointer pointer;

    public bool slotted {get; private set;} = false;

    void Awake(){ Setup(); }
    void Setup()
    {
        if (pointer == null){ pointer = GetComponent<ITEM_Pointer>(); }
        if (pointer == null){ Debug.Log("Error: Pointer not assigned!"); }
    }

    public bool Slot(STRUCTURE_ITEM_Slot source, Action OnSlottedUpdateSlot = null) //Local Call
    {
        if (pointer.behaviour.state == enum_ITEM_State.Held){ return false; }

        StartCoroutine(COROUTINE_ItemSlotted(source, OnSlottedUpdateSlot));

        return true;
    }
        private IEnumerator COROUTINE_ItemSlotted(STRUCTURE_ITEM_Slot source, Action OnSlottedUpdateSlot = null)
        {
            pointer.itemCollider.enabled = false;
        
                RPC_UPDATESERVER_Slotted(); //Tell Server to Update Clients (Incl. Me)

                while (!slotted){ yield return null; } //Pass Frames

                OnSlottedUpdateSlot(); //Item is Ready to be slotted call Slot to Update Contents!

                while (!pointer.physics.isKinematic){ yield return null; } //Pass to Next Frame if item not set Kinematic! To avoid Incorrect position due to Gravity.

                pointer.behaviour.ResetLocalPosition(source.STAT_snapPoint);

                while (transform.localPosition != Vector3.zero){ yield return null; } //Wait for Reset Position to Apply

            yield return new WaitForFixedUpdate(); // allow physics to settle
            pointer.itemCollider.enabled = true;

            yield break;
        }
        private void ItemSlotted() //Local Function
        {
            slotted = true;

            pointer.behaviour.SetState(enum_ITEM_State.Stored);
        }
        [ServerRpc]
        private void RPC_UPDATESERVER_Slotted() //Server Updater
        {
            RPC_UPDATECLIENTS_Slotted();
        }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_Slotted()
        {
            ItemSlotted(); //Trigger Local Function on Client Instance
        }
    public void UnSlot() //Local Call
    {
        if (!slotted){ return; }
        if (pointer.behaviour.state != enum_ITEM_State.Stored){ Debug.Log("Error: Item thought Slotted when Unslotting but Behaviour State not updated!"); return; }

        RPC_UPDATESERVER_UnSlotted();
    }
        private void ItemUnSlotted() //Local Function
        {
            slotted = false;

            if (pointer.behaviour.state == enum_ITEM_State.Stored){ pointer.behaviour.SetState(enum_ITEM_State.Loose); Debug.Log("Item Unslotted without state change. Assumed Loose!"); }
        }
        [ServerRpc]
        private void RPC_UPDATESERVER_UnSlotted() //Server Updater
        {
            RPC_UPDATECLIENTS_UnSlotted();
        }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_UnSlotted()
        {
            ItemUnSlotted(); //Trigger Local Function on Client Instance
        }
}