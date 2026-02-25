using UnityEngine;

public class PLAYER_Raycast : MonoBehaviour
{
    public Camera targetCam;

    public GameObject observedObject {get; private set;}

    void Awake(){ Setup(); }
    void Setup(){ if (targetCam == null){ targetCam = FindFirstObjectByType<Camera>(); } }

    void Update()
    {
        observedObject = GetObservedObject();
    }

    public GameObject GetObservedObject(float maxDistance = Mathf.Infinity, LayerMask layerMask = new ())
    { 
        Vector3 center = new Vector3(
            targetCam.pixelWidth * 0.5f,
            targetCam.pixelHeight * 0.5f,
            0f
        );

        Ray ray = targetCam.ScreenPointToRay(center);

        // If no mask is assigned, use default raycast layers
        int maskToUse = layerMask.value == 0 
            ? Physics.DefaultRaycastLayers 
            : layerMask.value;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, maskToUse))
        {
            GameObject hitObject = hit.collider.gameObject;
            return hitObject;
        }

        return null;
    }
}