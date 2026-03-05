using System.Collections.Generic;
using PurrNet;
using UnityEngine;

public class PLAYER_Inventory : NetworkBehaviour
{
    [SerializeField] private List<Transform> inventory = new ();
    //[SerializeField] private List<UI_ISlot> inventoryUI = new ();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){ Drop(); }
    }

    public void Add(GameObject item)
    { 
        int freeSpacePos = GetFreeSpacePos();
        Transform freeSpace = GetFreeSpace();
        if (freeSpace == null){ return; }

        item.GetComponent<NetworkTransform>().GiveOwnership(localPlayer);
        item.GetComponent<ITEM_Behaviour>().ResetLocalPosition(freeSpace);
    }
        //void UpdateUI(UI_ISlot iSlot, Sprite sprite){ iSlot.UpdateUI(sprite); }
        int GetFreeSpacePos()
        {
            int i = 0;
            foreach (var space in inventory)
            {
                if (space.childCount == 0){ return i; }
                i++;
            }

            return -1;
        }
        Transform GetFreeSpace()
        {
            foreach (var space in inventory)
            {
                if (space.childCount == 0){ return space; }
            }

            return null;
        }

    public void Drop()
    {
        GameObject item = null;

        foreach (var slot in inventory){ if (slot.childCount > 0){ item = slot.GetChild(0).gameObject; }}

            if (item == null){ return; }

        ITEM_Pickup itemPickup = item.GetComponent<ITEM_Pickup>();
            if (itemPickup == null){ item.transform.parent = null; return; }
        itemPickup.Drop();
    }

    public bool QueryIsFull(){ return GetFreeSpace() == null; }
}