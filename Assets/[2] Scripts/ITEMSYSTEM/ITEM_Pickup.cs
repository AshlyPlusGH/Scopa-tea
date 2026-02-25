using UnityEngine;
using PurrNet;

public class ITEM_Pickup : NetworkBehaviour
{
    public ITEM_Pointer pointer;

    void Awake(){ Setup(); }
    void Setup()
    {
        if (pointer == null){ pointer = GetComponent<ITEM_Pointer>(); }
        if (pointer == null){ Debug.Log("Error: Pointer not assigned!"); }
    }

    public void Pickup() //Local Call
    {
        if (pointer.behaviour.state == enum_ITEM_State.Held){ return; }

        RPC_UPDATESERVER_Pickup(); //Tell Server to Update Clients (Incl. Me)
    }
        private void ItemPickedUp() //Local Function
        {
            if (pointer.inSlot.slotted){ pointer.inSlot.UnSlot(); }

            pointer.behaviour.SetState(enum_ITEM_State.Held);
        }

        [ServerRpc] //Call Server: Method will run on Server!
        private void RPC_UPDATESERVER_Pickup(){ RPC_UPDATECLIENTS_Pickup(); } //Update all Clients
        [ObserversRpc] //Calls all Clients: Method will run on Client
        private void RPC_UPDATECLIENTS_Pickup(){ ItemPickedUp(); } //Trigger Local Function on Client Instance

    public void Drop() //Local Call
    {
        if (pointer.behaviour.state == enum_ITEM_State.Loose){ Debug.Log("Error: Item was told to be Dropped but was already Loose!"); return; }

        RPC_UPDATESERVER_Drop(); //Tell Server to Update Clients (Incl. Me)
    }
        private void ItemDropped() //Local Function
        {
            pointer.behaviour.SetState(enum_ITEM_State.Loose);
        }

        [ServerRpc] //Call Server: Method will run on Server!
        private void RPC_UPDATESERVER_Drop(){ RPC_UPDATECLIENT_Drop(); } //Update all Clients
        [ObserversRpc] //Calls all Clients: Method will run on Client
        private void RPC_UPDATECLIENT_Drop(){ ItemDropped(); } //Trigger Local Function on Client Instance
}