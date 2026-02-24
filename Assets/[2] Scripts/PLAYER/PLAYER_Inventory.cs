using System.Collections.Generic;
using PurrNet;
using UnityEngine;

public class PLAYER_Inventory : MonoBehaviour
{
    [SerializeField] private List<Transform> inventory = new ();
    //[SerializeField] private List<UI_ISlot> inventoryUI = new ();

    public void Add(GameObject item)
    { 
        int freeSpacePos = GetFreeSpacePos();
        Transform freeSpace = GetFreeSpace();
        if (freeSpace != null){ AddItemToSpace(item.transform, freeSpace); }
    }
        void AddItemToSpace(Transform item, Transform space)
        {
            item.SetParent(space);

            item.localPosition = Vector3.zero;
            item.localRotation = Quaternion.identity;
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

    public bool QueryIsFull(){ return GetFreeSpace() == null; }
}