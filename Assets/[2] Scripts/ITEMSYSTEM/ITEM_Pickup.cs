using UnityEngine;
using PurrNet;
using NaughtyAttributes;

public class ITEM_Pickup : NetworkBehaviour
{
    public ITEM_Pointer itemPointer;

    public OBJECT_Draggable draggable;
    public Rigidbody physics;

    public enum_ITEM_PickupState state {get; private set;} = enum_ITEM_PickupState.None;

    public void Pickup() //Local Call
    {
        if (state == enum_ITEM_PickupState.Held){ return; }

        RPC_UPDATESERVER_Pickup(); //Tell Server to Update Clients (Incl. Me)
    }
        private void ItemPickedUp() //Local Function
        {
            state = enum_ITEM_PickupState.Held;

            draggable.enabled = false; //Not Draggable in Inventory!
            physics.isKinematic = true;
            physics.linearVelocity = Vector3.zero;
            physics.angularVelocity = Vector3.zero;
        }

        [ServerRpc] //Call Server: Method will run on Server!
        private void RPC_UPDATESERVER_Pickup(){ RPC_UPDATECLIENT_Pickup(); } //Update all Clients
        [ObserversRpc] //Calls all Clients: Method will run on Client
        private void RPC_UPDATECLIENT_Pickup(){ ItemPickedUp(); } //Trigger Local Function on Client Instance

    public void Drop() //Local Call
    {
        if (state == enum_ITEM_PickupState.Dropped){ return; }

        RPC_UPDATESERVER_Drop(); //Tell Server to Update Clients (Incl. Me)
    }
        private void ItemDropped() //Local Function
        {
            physics.isKinematic = false;
            draggable.enabled = true; //Not Draggable in Inventory!

            state = enum_ITEM_PickupState.Dropped;
        }

        [ServerRpc] //Call Server: Method will run on Server!
        private void RPC_UPDATESERVER_Drop(){ RPC_UPDATECLIENT_Drop(); } //Update all Clients
        [ObserversRpc] //Calls all Clients: Method will run on Client
        private void RPC_UPDATECLIENT_Drop(){ ItemDropped(); } //Trigger Local Function on Client Instance
}

public enum enum_ITEM_PickupState
{
    None,
    Held,
    Dropped
}