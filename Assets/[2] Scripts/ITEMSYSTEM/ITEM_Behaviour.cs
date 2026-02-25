using UnityEngine;
using PurrNet;

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
                break;
            case enum_ITEM_State.Held:
                SetPhysics(true, true);
                SetDraggable(false);
                break;
            case enum_ITEM_State.Loose:
                SetPhysics();
                SetDraggable(true);
                break;
            case enum_ITEM_State.Stored:
                SetPhysics(true, true);
                SetDraggable(false);
                break;
        }
    }
    public void SetPhysics(bool isKinematic = false, bool haltMovement = false){ RPC_UPDATESERVER_SetPhysics(isKinematic, haltMovement); }
        public void SettingPhysics(bool isKinematic = false, bool haltMovement = false)
        {
            pointer.physics.isKinematic = isKinematic;
            if (haltMovement){ HaltMovement(); }
        }
            private void HaltMovement(){ pointer.physics.linearVelocity = Vector3.zero; pointer.physics.angularVelocity = Vector3.zero; }
        [ServerRpc]
        private void RPC_UPDATESERVER_SetPhysics(bool isKinematic = false, bool haltMovement = false){ RPC_UPDATECLIENTS_SetPhysics(isKinematic, haltMovement); }
        [ObserversRpc]
        private void RPC_UPDATECLIENTS_SetPhysics(bool isKinematic = false, bool haltMovement = false){ SettingPhysics(isKinematic, haltMovement); }
    public void SetDraggable(bool enabled){ RPC_UPDATESERVER_SetDraggable(enabled); }
        private void SettingDraggable(bool enabled){ pointer.draggable.enabled = enabled; }
        [ServerRpc] private void RPC_UPDATESERVER_SetDraggable(bool enabled){ RPC_UPDATECLIENTS_SetDraggable(enabled); }
        [ObserversRpc] private void RPC_UPDATECLIENTS_SetDraggable(bool enabled){ SettingDraggable(enabled); }
    public void SetPickupEnabled(bool enabled){ RPC_UPDATESERVER_SetPickupEnabled(enabled); }
        private void SettingPickupEnabled(bool enabled){ pointer.pickup.enabled = enabled; }
        [ServerRpc] private void RPC_UPDATESERVER_SetPickupEnabled(bool enabled){ RPC_UPDATECLIENTS_SetPickupEnabled(enabled); }
        [ObserversRpc] private void RPC_UPDATECLIENTS_SetPickupEnabled(bool enabled){ SettingPickupEnabled(enabled); }
    public void SetSlotting(bool enabled){ RPC_UPDATESERVER_SetSlotting(enabled); }
        private void SettingSlotting(bool enabled){ pointer.inSlot.enabled = enabled; }
        [ServerRpc] private void RPC_UPDATESERVER_SetSlotting(bool enabled){ RPC_UPDATECLIENTS_SetSlotting(enabled); }
        [ObserversRpc] private void RPC_UPDATECLIENTS_SetSlotting(bool enabled){ SettingSlotting(enabled); }

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