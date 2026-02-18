using UnityEngine;
using System;

public class STRUCTURE_Generator : STRUCTURE
{
    [Space(10)]

    [SerializeField] protected bool INSPECTOR_isPowering;

    public bool IsPowering { get; private set; }

    // Fired on the server when power changes.
    public event Action<STRUCTURE_Generator> OnPowerChanged;

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

    public void PowerOn(){ SetPowering(true); }
    public void PowerOff(){ SetPowering(false); }
}