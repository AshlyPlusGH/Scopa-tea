using UnityEngine;
using System.Collections.Generic;
using System;
using PurrNet;
using UnityEngine.Events;

public class STRUCTURE_Door : STRUCTURE
{
    public bool debug;

    [Space(10)]

    [SerializeField] private bool openState; //Internal Data
    public bool STAT_openState => openState; //Pointer

    [Space(10)]

    public List<STRUCTURE_ITEM_Slot> inventory = new();

    [Space(10)]

    [SerializeField] private UnityEvent OnOpen;
    [SerializeField] private UnityEvent OnClose;

    void Awake(){ Setup(); }
    void Setup(){ foreach (var ISlot_Behaviour in inventory){ ISlot_Behaviour.OnInventoryChanged += OnInventoryChanged; } }

    public void Open(){ if (debug){ Debug.Log("Attempting Open!"); } if (openState){ return; } SetOpenState(true); }
    public void Close(){ if (debug){ Debug.Log("Attempting Close!"); } if (!openState){ return; } SetOpenState(false); }
    private void SetOpenState(bool state){ RPC_UPDATESERVER_SetOpenState(state); }
        public void SettingOpenState(bool state)
        {
            switch (state)
            {
                case true:
                    OnOpen.Invoke();
                    if (debug){ Debug.Log("Opening!"); }
                    break;
                case false:
                    OnClose.Invoke();
                    if (debug){ Debug.Log("Closing!"); }
                    break;
            }

            openState = state;
        }
        [ServerRpc]
        private void RPC_UPDATESERVER_SetOpenState(bool state){ RPC_UPDATECLIENTS_SetOpenState(state); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_SetOpenState(bool state){ SettingOpenState(state); }

    public void OnInventoryChanged()
    {
            if (!isServer){ return; } //Server AUTH
            
            List<enum_ITEM_Type> inventoryContents = GetInventoryContents();
            if (inventoryContents.Count == 0){ Close(); return; }
        foreach (var item in inventoryContents){ if (item != enum_ITEM_Type.Fuse){ Close(); return; }}
            
        Open();
    }
    public List<enum_ITEM_Type> GetInventoryContents()
    {
        List<enum_ITEM_Type> contents = new();

        foreach (var iSlot in inventory)
        {
            soDATA_Item itemData = iSlot.STAT_heldItemData; if (itemData == null){ continue; }
            
            contents.Add(itemData.STAT_type);
        }

        return contents;
    }
}