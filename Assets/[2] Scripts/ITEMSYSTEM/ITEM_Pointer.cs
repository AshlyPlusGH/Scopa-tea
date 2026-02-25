using PurrNet;
using UnityEngine;

public class ITEM_Pointer : MonoBehaviour
{
    public NetworkManager networkManager;

    public Rigidbody physics;
    public Collider itemCollider;
    public NetworkTransform networkTransform;

    public OBJECT_Draggable draggable;

    public ITEM_Behaviour behaviour;
    public ITEM_Data dataContainer;
    public ITEM_Pickup pickup;
    public ITEM_STRUCTURE_InItemSlot inSlot;

    void Awake(){ Setup(); }
    void Setup(){ if (networkManager == null){ networkManager = FindFirstObjectByType<NetworkManager>(); }}
}