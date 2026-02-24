using UnityEngine;

public class ITEM_Data : MonoBehaviour
{
    [SerializeField] private soDATA_Item INPUT_data;

    public soDATA_Item data {get; private set;}

    void Awake(){ Setup(); }

    void Setup(){ data = INPUT_data; }
}