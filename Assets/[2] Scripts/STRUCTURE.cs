using UnityEngine;
using PurrNet;
using System.Collections.Generic;
using System.Linq;

public class STRUCTURE : NetworkBehaviour
{
    [Header("Inspector Only Vars: Do not change or reference except to update their status!")]

    [SerializeField] protected bool isPowered;
    public List<STRUCTURE_Generator> INSPECTOR_inputGenerators = new();

    [Space(10)]

    public List<GameObject> powerDependantComponents = new();
    
    protected readonly List<STRUCTURE_Generator> generators = new();
    public bool STAT_isPowered => isPowered;

    void Awake(){ Setup(); }
    /// <summary> Local non-networked Setup method </summary>
    void Setup()
    {
        foreach (var source in INSPECTOR_inputGenerators){ generators.Add(source); }

        UpdateComponents();
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        foreach (var src in generators){ src.onPowerChanged += OnSourcePowerChanged; }

        UpdatePowerState();
    }
    protected override void OnDespawned()
    {
        base.OnDespawned();

        foreach (var src in generators){ src.onPowerChanged -= OnSourcePowerChanged; }
    }

    /// <summary> Runs when a Source's Powering bool changes </summary>
    protected void OnSourcePowerChanged(STRUCTURE_Generator callSource){ UpdatePowerState(); }
    protected void UpdatePowerState(){ SetPowerState(AnySourcesPowered()); } //Internal Call: Updates all clients when power state changes on any one source in case no more sources powering!
    public void SetPowerState(bool state){ RPC_UPDATESERVER_SetPowerState(state); } //Public Call
        private void SettingPowerState(bool powered)
        {
            isPowered = powered;

            UpdateComponents();
        }
        [ServerRpc] //Call Server: Method will run on Server!
        protected void RPC_UPDATESERVER_SetPowerState(bool powered){ RPC_UPDATECLIENTS_SetPowerState(powered); } //Update all Clients
        [ObserversRpc] //Calls all Clients: Method will run on Client
        private void RPC_UPDATECLIENTS_SetPowerState(bool powered){ SettingPowerState(powered); } //Trigger Local Function on Client Instance

    public bool AnySourcesPowered()
    {
        return generators.Count > 0 && generators.Any(s => s.STAT_isPowering);
    }

    /// <summary> Called by generators when they connect </summary>
    public void RegisterSource(STRUCTURE_Generator src)
    {
        if (!isServer) return;

        if (generators.Contains(src)) return;

        generators.Add(src);
        src.onPowerChanged += OnSourcePowerChanged;

        UpdatePowerState();
    }
    /// <summary> Called by generators when they disconnect </summary>
    public void UnregisterSource(STRUCTURE_Generator src)
    {
        if (!isServer) return;

        if (!generators.Contains(src)) return;

        generators.Remove(src);
        src.onPowerChanged -= OnSourcePowerChanged;

        UpdatePowerState();
    }

    /// <summary> Sets connected GameObjects that make up this Structure to this Structure's Powered state </summary>
    public void UpdateComponents(){ foreach (var component in powerDependantComponents){ component.SetActive(isPowered); }}
}