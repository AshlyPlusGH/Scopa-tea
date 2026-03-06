using System.Collections;
using UnityEngine;

public class PLAYER_PickupItem : MonoBehaviour
{
    [SerializeField] private PLAYER_Inventory inventory;
    [SerializeField] private PLAYER_Raycast raycaster;

    [Space(10)]

    [SerializeField] private GameObject pickupPromptPrefab;

    [Space(10)]

    [SerializeField] private LayerMask pickupRaycastLayerMask;

    private GameObject activePickupPrompter;
    private Coroutine RUNNINGCOROUTINE_PickupItem;

    public const float RULE_PLAYER_pickupRange = 10f; 
    private float pickupRange => RULE_PLAYER_pickupRange;

    public void TEST(){ PickupItem(FindFirstObjectByType<ITEM_Pickup>()); }

    void Awake()
    {
        Setup();
    }
    void Setup(){ activePickupPrompter = Instantiate(pickupPromptPrefab); UpdatePrompter(); }

    void Update()
    {
        GameObject observedObject = raycaster.GetObservedObject(pickupRange, pickupRaycastLayerMask);

        TryUpdatePrompter(observedObject);

        TryPickupItem(observedObject);
    }

    void TryPickupItem(GameObject observedObject = null)
    {
        if (!Input.GetKeyDown(KeyCode.E)){ return; }

        if (observedObject == null){ return; }
        if (observedObject.GetComponent<ITEM_Pickup>() == null){ return; }

        PickupItem(observedObject.GetComponent<ITEM_Pickup>());
    }
    void PickupItem(ITEM_Pickup item)
    {
        if (RUNNINGCOROUTINE_PickupItem != null){ return; } //Script busy, don't attempt Pickup!
        if (inventory.QueryIsFull()){ InventoryFull(); return; } //Inventory Full

        RUNNINGCOROUTINE_PickupItem = StartCoroutine(COROUTINE_PickupItem(item));
    }
        IEnumerator COROUTINE_PickupItem(ITEM_Pickup item)
        {
            item.Pickup();

            while (!item.pointer.physics.isKinematic){ yield return null; } //Pass to Next Frame if item not set Kinematic! To avoid Incorrect position due to Gravity.

            inventory.Add(item.gameObject); //Add to Inventory when Object is Kinematic

            RUNNINGCOROUTINE_PickupItem = null;

            yield break;
        }

    void InventoryFull(){}

    void TryUpdatePrompter(GameObject observedObject = null)
    {
        if (observedObject == null){ UpdatePrompter(); }
        else if (observedObject.GetComponent<ITEM_Pickup>() == null){ UpdatePrompter(); }
        else if (observedObject.GetComponent<ITEM_Pickup>().pointer.behaviour.state == enum_ITEM_State.Held){ UpdatePrompter(); }
        else { UpdatePrompter(true, observedObject.transform.position); }
    }
    void UpdatePrompter(bool queryActive = false, Vector3 newPosition = new())
    {
        if (!queryActive){ activePickupPrompter.SetActive(false); return; }

        activePickupPrompter.SetActive(true);
        activePickupPrompter.transform.position = newPosition;
    }
}