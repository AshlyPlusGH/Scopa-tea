using UnityEngine;
using PurrNet;

public class ITEM_Behaviour : NetworkBehaviour
{
    [SerializeField] private soDATA_Item itemData;
    public soDATA_Item Data => itemData;

    private ISlot currentSlot;

    //private DraggableObject itemDraggable;
    private Rigidbody rb;

    void Awake()
    {
        //if (itemDraggable == null){ itemDraggable = GetComponent<DraggableObject>(); }
        if (rb == null){ rb = GetComponent<Rigidbody>(); }
    }

    public void TryPlaceInSlot(ISlot slot)
    {
        if (!isServer)  // Clients request, server decides
        {
            ServerPlaceInSlot(slot.SlotId);
            return;
        }
        if (itemData == null){ Debug.Log("No Item Data Detected!"); return; }

        if (slot.CanAccept(itemData.STAT_itemName))
        {
            currentSlot = slot;
            slot.AssignItem(itemData.STAT_itemName);

            FreezeItem();

            transform.position = slot.SnapPoint.position;
            transform.rotation = slot.SnapPoint.rotation;
        }
    }

    void FreezeItem()
    {
        //itemDraggable.enabled = false;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    [ServerRpc]
    private void ServerPlaceInSlot(int slotId)
    {
        var slot = ISlotRegistry.GetSlot(slotId);
        TryPlaceInSlot(slot);
    }
}