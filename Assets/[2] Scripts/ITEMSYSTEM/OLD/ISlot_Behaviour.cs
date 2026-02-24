using UnityEngine;
using PurrNet;
using System.Collections.Generic;
using System;

public class ISlot_Behaviour : NetworkBehaviour, ISlot
{
    [SerializeField] private Transform snapPoint;

    [SerializeField] private SyncVar<string> heldItem = new();
    public string HeldItem => heldItem.value;

    public int SlotId { get; private set; }
    public Transform SnapPoint => snapPoint;

    public event Action OnInventoryChanged;

    private void Awake()
    {
        ISlotRegistry.Register(this);
    }

    public bool CanAccept(string val)
    {
        return heldItem.value == null;
    }

    [Server]
    public void AssignItem(string val)
    {
        heldItem.value = val;

        OnInventoryChanged?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        if (other.TryGetComponent<ITEM_Behaviour>(out var item))
        {
            item.TryPlaceInSlot(this);
        }
    }
}