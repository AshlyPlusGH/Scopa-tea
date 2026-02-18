using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(
    fileName = "[Item] New Item",
    menuName = "++ITEM",
    order = 0
)]
public class soDATA_Item : ScriptableObject
{
    // INTERNAL DATA //

    [SerializeField, Label("Item Name")] 
    private string soDATA_itemName = "NO NAME";

    ////
    
    [Header("-Type-")]

    // POINTERS //

    public string STAT_itemName => soDATA_itemName;

    ////
}