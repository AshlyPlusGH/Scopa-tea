using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera targetCam;
    public Transform target;

    void Awake(){ Setup(); }
    void Setup()
    {
        if (targetCam == null){ targetCam = Camera.main; }
        if (targetCam == null){ targetCam = FindFirstObjectByType<Camera>(); }
        if (target == null){ target = transform; }
    }

    void Update()
    {
        target.LookAt(targetCam.transform);
    }
}