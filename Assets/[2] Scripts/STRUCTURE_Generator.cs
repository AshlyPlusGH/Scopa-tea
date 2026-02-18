using UnityEngine;
using System.Collections.Generic;
using System;

public class STRUCTURE_Generator : STRUCTURE
{
    [Space(10)]

    [SerializeField] protected bool INSPECTOR_isPowering;

    public bool IsPowering { get; private set; }
    public List<ISlot_Behaviour> inventory = new();
    public soDATA_Item fuseDefinition;

    // Fired on the server when power changes.
    public event Action<STRUCTURE_Generator> OnPowerChanged;

    void Awake()
    {
        foreach (var ISlot_Behaviour in inventory)
        {
            ISlot_Behaviour.OnInventoryChanged += OnInventoryChanged;
        }
    }

    /// <summary>
    /// Server-authoritative setter for power state.
    /// Call this ONLY on the server.
    /// </summary>
    public void SetPowering(bool powered)
    {
        if (!isServer) return;          // Guard: only server may change state
        if (IsPowering == powered) return;

        IsPowering = powered;
        INSPECTOR_isPowering = IsPowering;

        OnPowerChanged?.Invoke(this);   // STRUCTURE listens to this on the server
    }

    public void OnInventoryChanged(){ if (GetInventoryContents().Contains(fuseDefinition.STAT_itemName)){ PowerOn(); }else{ PowerOff(); }}
    public List<string> GetInventoryContents()
    {
        List<string> contents = new();

        foreach (var iSlot in inventory)
        {
            contents.Add(iSlot.HeldItem);
        }

        return contents;
    }

    public void PowerOn(){ SetPowering(true); }
    public void PowerOff(){ SetPowering(false); }
}