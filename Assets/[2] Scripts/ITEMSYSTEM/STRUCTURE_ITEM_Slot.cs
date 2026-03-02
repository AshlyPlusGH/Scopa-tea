using UnityEngine;
using PurrNet;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;

public class STRUCTURE_ITEM_Slot : NetworkBehaviour
{
    public bool debug;

    [Space(10)]

    //THIS STRUCTURE SHOULD ONLY UPDATE CLIENTS FROM SERVERSIDE TO AVOID CONFLICTS

    [SerializeField] private Transform snapPoint; //Internal Data
    [DoNotSerialize] public Transform STAT_snapPoint => snapPoint; //Pointer

    [Space(5)]

    [SerializeField] private soDATA_Item heldItemData = null; //Internal Data
    [DoNotSerialize] public soDATA_Item STAT_heldItemData => heldItemData; //Pointer

    [Space(10)]

    [SerializeField] private UnityEvent OnItemSlotted;
    [SerializeField] private UnityEvent OnItemUnSlotted;

    public event Action OnInventoryChanged;

    private soDATA_Item slottingItem; //To hold slotting item while it sets its own behaviour!
    private ITEM_STRUCTURE_InItemSlot slottedItem;

    public bool disableTriggers;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"ENTER: {other.name} | Collider: {other.GetInstanceID()} | Root: {other.transform.root.name}");

        if (disableTriggers) return;

        if (!isServer) return; //SERVER ONLY UPDATES FOR TRIGGER ENTERS
        if (heldItemData != null) return; //Do not stack items

        ITEM_STRUCTURE_InItemSlot item = other.GetComponent<ITEM_STRUCTURE_InItemSlot>(); if (item == null){ return; }

        if (item.slotted) return;

        slottedItem = item;
        SlotItem(item);
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"EXIT: {other.name} | Collider: {other.GetInstanceID()} | Root: {other.transform.root.name}");

        if (disableTriggers) return;

        if (!isServer) return; //SERVER ONLY UPDATES FOR TRIGGER ENTERS

        ITEM_STRUCTURE_InItemSlot item = other.GetComponent<ITEM_STRUCTURE_InItemSlot>(); if (item == null){ return; }

        if (item != slottedItem) return; //Only look for stored Item leaving Slot
        UnSlotItem();
    }

    public void SlotItem(ITEM_STRUCTURE_InItemSlot item)
    {
        if (debug){ Debug.Log("Attempting to Slot Item!"); }

        slottingItem = item.pointer.dataContainer.STAT_data;
        if (item.Slot(this, UpdateSlotContents)){ if (debug){ Debug.Log("Slotting Item!"); } } //Item Slotting!
        else { slottedItem = null; slottingItem = null; } //Item can't Slot!

        OnItemSlotted.Invoke();
    }
    public void UnSlotItem()
    {
        if (debug){ Debug.Log("Unslotting Item!"); }

        slottedItem = null; slottingItem = null;
        UpdateSlotContents();

        OnItemUnSlotted.Invoke();
    }
        private void UpdateSlotContents()
        {
            RPC_UPDATESERVER_UpdateSlotContents();
        }
        private void UpdatingSlotContents()
        {
            heldItemData = slottingItem;

            if (!isServer){ return; }

            OnInventoryChanged.Invoke(); //Trigger Inventory changed. SERVERAUTH
        }
        [ServerRpc]
        private void RPC_UPDATESERVER_UpdateSlotContents(){ RPC_UPDATECLIENTS_UpdateSlotContents(); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_UpdateSlotContents(){ UpdatingSlotContents(); }
}