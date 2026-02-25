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

    [SerializeField, Label("Item Name")] private string soDATA_itemName = "NO NAME";
    [SerializeField, Label("Item Name")] private enum_ITEM_Type soDATA_type = enum_ITEM_Type.None;

    ////
    
    [Header("-Type-")]

    // POINTERS //

    public string STAT_itemName => soDATA_itemName;
    public enum_ITEM_Type STAT_type => soDATA_type;

    ////
}

public enum enum_ITEM_Type
{
    None,
    Fuse
}