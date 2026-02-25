using UnityEngine;

public class ITEM_Data : MonoBehaviour
{
    public bool debug;

    [Space(10)]

    [SerializeField] private ITEM_Pointer pointer;

    [Space(10)]

    [SerializeField] private soDATA_Item data; //Internal Data
    public soDATA_Item STAT_data => data; //Pointer

    void Awake(){ Setup(); }
    void Setup()
    {
        if (pointer == null){ pointer = GetComponent<ITEM_Pointer>(); }
        if (pointer == null){ Debug.Log("Error: Pointer not assigned!"); }
    }
}