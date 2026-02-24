using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class PLAYER_PickupItem : MonoBehaviour
{
    public PLAYER_Inventory inventory;
    public PLAYER_Raycast raycaster;

    [Space(10)]

    public GameObject pickupPromptPrefab;

    private GameObject activePickupPrompter;
    private Coroutine RUNNINGCOROUTINE_PickupItem;

    const float pickupRange = 10f; 

    public void TEST(){ PickupItem(FindFirstObjectByType<ITEM_Pickup>()); }

    void Awake()
    {
        Setup();
    }
    void Setup(){ activePickupPrompter = Instantiate(pickupPromptPrefab); UpdatePrompter(); }

    void Update()
    {
        GameObject observedObject = raycaster.GetObservedObject(pickupRange);

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
        if (RUNNINGCOROUTINE_PickupItem != null){ return; }
        if (inventory.QueryIsFull()){ return; }

        RUNNINGCOROUTINE_PickupItem = StartCoroutine(COROUTINE_PickupItem(item));
    }
        IEnumerator COROUTINE_PickupItem(ITEM_Pickup item)
        {
            item.Pickup(); //Call Item to be picked up!

            while (!item.physics.isKinematic){ yield return null; } //Pass to Next Frame if item not set Kinematic!

            inventory.Add(item.gameObject); //Add to Inventory when Object is Kinematic

            RUNNINGCOROUTINE_PickupItem = null;

            yield break;
        }

    void TryUpdatePrompter(GameObject observedObject = null)
    {
        if (observedObject == null){ UpdatePrompter(); }
        else if (observedObject.GetComponent<ITEM_Pickup>() == null){ UpdatePrompter(); }
        else { UpdatePrompter(true, observedObject.transform.position); }
    }
    void UpdatePrompter(bool queryActive = false, Vector3 newPosition = new())
    {
        if (!queryActive){ activePickupPrompter.SetActive(false); return; }

        activePickupPrompter.SetActive(true);
        activePickupPrompter.transform.position = newPosition;
    }
}