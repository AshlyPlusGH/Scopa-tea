using UnityEngine;
using PurrNet;

public class ITEM_InItemSlot : NetworkBehaviour
{
    public OBJECT_Draggable draggable;

    public void Slot() //Local Call
    {
        RPC_UPDATESERVER_Slotted(); //Tell Server to Update Clients (Incl. Me)
    }

        private void ItemSlotted() //Local Function
        {
            draggable.enabled = false; //Not Draggable in Inventory!
        }

        [ServerRpc]
        private void RPC_UPDATESERVER_Slotted() //Server Updater
        {
            RPC_UPDATECLIENT_Slotted();
        }
        [ObserversRpc]
        private void RPC_UPDATECLIENT_Slotted()
        {
            ItemSlotted(); //Trigger Local Function on Client Instance
        }
}