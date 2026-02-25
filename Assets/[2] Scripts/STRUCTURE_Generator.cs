using UnityEngine;
using System.Collections.Generic;
using System;
using PurrNet;

public class STRUCTURE_Generator : STRUCTURE
{
    public bool debug;

    [Space(10)]

    [SerializeField] private bool isPowering; //Internal Data
    public bool STAT_isPowering => isPowering; //Pointer

    [Space(10)]

    public List<STRUCTURE_ITEM_Slot> inventory = new();

    public event Action<STRUCTURE_Generator> onPowerChanged; //Fired when powering state changes

    void Awake(){ Setup(); }
    void Setup(){ foreach (var ISlot_Behaviour in inventory){ ISlot_Behaviour.OnInventoryChanged += OnInventoryChanged; } }

    public void PowerOn(){ SetPoweringState(true); }
    public void PowerOff(){ SetPoweringState(false); }
    private void SetPoweringState(bool state){ RPC_UPDATESERVER_SetPoweringState(state); }
        public void SettingPoweringState(bool state)
        {
            isPowering = state;

            onPowerChanged?.Invoke(this); //STRUCTUREs listen to this
        }
        [ServerRpc]
        private void RPC_UPDATESERVER_SetPoweringState(bool state){ RPC_UPDATECLIENTS_SetPoweringState(state); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_SetPoweringState(bool state){ SettingPoweringState(state); }

    public void OnInventoryChanged(){ if (GetInventoryContents().Contains(enum_ITEM_Type.Fuse)){ PowerOn(); } else{ PowerOff(); }}
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