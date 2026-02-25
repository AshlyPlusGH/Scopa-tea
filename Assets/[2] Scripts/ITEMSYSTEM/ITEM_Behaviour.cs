using UnityEngine;
using PurrNet;
using PurrNet.Modules;
using System.Drawing;

public class ITEM_Behaviour : NetworkBehaviour
{
    public bool debug;

    [Space(10)]

    [SerializeField] private ITEM_Pointer pointer;

    public enum_ITEM_State state {get; private set;} = enum_ITEM_State.None;

    void Awake(){ Setup(); }
    void Setup(){ UpdateBehaviour(); }

    public void SetState(enum_ITEM_State newState){ if (debug){ Debug.Log(name + ": Set State: " + newState); } state = newState; pointer.behaviour.UpdateBehaviour(); }

    private void UpdateBehaviour()
    {
        switch (state)
        {
            case enum_ITEM_State.None:
                SetPhysics();
                SetDraggable(true);
                SetPickupEnabled(true);
                break;
            case enum_ITEM_State.Held:
                SetPhysics(true, true);
                SetDraggable(false);
                SetPickupEnabled(false);
                break;
            case enum_ITEM_State.Loose:
                SetPhysics();
                SetDraggable(true);
                SetPickupEnabled(true);
                break;
            case enum_ITEM_State.Stored:
                SetPhysics(true, true);
                SetDraggable(false);
                SetPickupEnabled(true);
                break;
        }
    }

    public void SetPhysics(bool isKinematic = false, bool haltMovement = false)
    {
        pointer.physics.isKinematic = isKinematic;
        if (haltMovement){ HaltMovement(); }
    }
        private void HaltMovement(){ pointer.physics.linearVelocity = Vector3.zero; pointer.physics.angularVelocity = Vector3.zero; }
    public void SetDraggable(bool enabled){ pointer.draggable.enabled = enabled; }
    public void SetPickupEnabled(bool enabled){ pointer.pickup.enabled = enabled; }
    public void SetSlotting(bool enabled){ pointer.inSlot.enabled = enabled; }

    [ServerRpc]
    public void SetOwnerFull(PlayerID newOwner)
    {
        GlobalOwnershipModule ownershipModule = pointer.networkManager.GetModule<GlobalOwnershipModule>(true);
        ownershipModule.GiveOwnership(this, newOwner);
        ownershipModule.GiveOwnership(pointer.pickup, newOwner);
        ownershipModule.GiveOwnership(pointer.draggable, newOwner);
        ownershipModule.GiveOwnership(pointer.inSlot, newOwner);
        ownershipModule.GiveOwnership(pointer.networkTransform, newOwner);
    }

    public void ResetLocalPosition(Transform parent = null){ RPC_UPDATESERVER_ResetLocalPosition(parent); }
        [ServerRpc]
        private void RPC_UPDATESERVER_ResetLocalPosition(Transform parent = null){ RPC_UPDATECLIENTS_ResetLocalPosition(parent); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_ResetLocalPosition(Transform parent = null){ ResettingLocalPosition(parent); }
        private void ResettingLocalPosition(Transform parent = null)
        {
            Vector3 itemScale = transform.localScale;

            if (parent != null){ transform.SetParent(parent); }

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = itemScale; //Fixes Unity floating point error!
        }
}

public enum enum_ITEM_State
{
    None,
    Held,
    Loose,
    Stored
}