using UnityEngine;
using PurrNet;
using System.Collections.Generic;

public interface ISlot
{
    int SlotId { get; }
    Transform SnapPoint { get; }

    string HeldItem { get; }  // Getter only

    bool CanAccept(string val);
    void AssignItem(string val); // Server only
}

public static class ISlotRegistry
{
    private static readonly Dictionary<int, ISlot> slots = new();

    public static void Register(ISlot slot)
    {
        slots[slot.SlotId] = slot;
    }

    public static ISlot GetSlot(int id)
    {
        return slots[id];
    }
}