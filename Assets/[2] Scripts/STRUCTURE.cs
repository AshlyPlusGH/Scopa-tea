using UnityEngine;
using PurrNet;
using System.Collections.Generic;
using System.Linq;

public class STRUCTURE : NetworkBehaviour
{
    [SerializeField] protected bool INSPECTOR_isPowered;
    public List<STRUCTURE_Generator> INSPECTOR_inputGenerators = new();

    [Space(10)]

    public List<GameObject> powerDependantComponents = new();
    

    //Internal Only Vars
        protected readonly List<STRUCTURE_Generator> generators = new();

        // Server-side authoritative state
        protected bool isPoweredServer;
        // Client-side cached state
        protected bool isPoweredClient;

    void Awake()
    {
        INSPECTOR_isPowered = isPoweredClient;

        foreach (var source in INSPECTOR_inputGenerators)
            generators.Add(source);

        UpdateComponents();
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        // Subscribe to generator events on server only
        foreach (var src in generators)
            src.OnPowerChanged += OnSourcePowerChanged;

        EvaluatePowerServer();
    }

    protected override void OnDespawned()
    {
        base.OnDespawned();

        foreach (var src in generators)
            src.OnPowerChanged -= OnSourcePowerChanged;
    }

    // Called only on server when a generator changes
    protected void OnSourcePowerChanged(STRUCTURE_Generator _)
    {
        EvaluatePowerServer();
    }

    // SERVER: authoritative evaluation
    protected void EvaluatePowerServer()
    {
        bool powered = AllSourcesPowered();

        if (powered == isPoweredServer)
            return;

        isPoweredServer = powered;

        // Notify all clients
        RpcSetPowerState(powered);
    }

    // CLIENT: receives authoritative state
    [ServerRpc]
    protected void RpcSetPowerState(bool powered)
    {
        isPoweredClient = powered;
        INSPECTOR_isPowered = isPoweredClient;

        Debug.Log($"{name} powered (client): {powered}");

        // Trigger client-side effects
        UpdateComponents();
    }

    // Server-only logic
    public bool AllSourcesPowered()
    {
        return generators.Count > 0 && generators.All(s => s.IsPowering);
    }

    // Called by generators when they connect/disconnect
    public void RegisterSource(STRUCTURE_Generator src)
    {
        if (!isServer) return;

        if (generators.Contains(src)) return;

        generators.Add(src);
        src.OnPowerChanged += OnSourcePowerChanged;

        EvaluatePowerServer();
    }

    public void UnregisterSource(STRUCTURE_Generator src)
    {
        if (!isServer) return;

        if (!generators.Contains(src)) return;

        generators.Remove(src);
        src.OnPowerChanged -= OnSourcePowerChanged;

        EvaluatePowerServer();
    }

    public void UpdateComponents()
    {
        foreach (var component in powerDependantComponents)
        {
            component.SetActive(isPoweredClient);
        }
    }
}