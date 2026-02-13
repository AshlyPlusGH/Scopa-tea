using UnityEngine;
using PurrNet;

[RequireComponent(typeof(Rigidbody))]
public class DraggableObject : NetworkBehaviour
{
    public SyncVar<bool> isHeld { get; private set; } = new SyncVar<bool>() { value = false };

    private Camera cam;
    private Rigidbody rb;

    private Vector3 offset;
    private float dragDistance;

    float dragSpeed = 12f;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        cam = FindFirstObjectByType<DraggableObjectCamera>().GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void OnMouseDown()
    {
        if (isHeld.value)
            return;

        // Tell server we want to grab it
        RequestSetHeldRpc(true);

        dragDistance = Vector3.Distance(cam.transform.position, transform.position);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(cam.transform.forward * -1, transform.position);

        if (plane.Raycast(ray, out float hitDist))
        {
            offset = transform.position - ray.GetPoint(hitDist);
        }

        rb.useGravity = false;
    }

    void OnMouseDrag()
    {
        if (!isHeld.value)
            return;

        // Client computes the target point
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos = ray.origin + ray.direction * dragDistance;

        // Send target to server
        SendDragTargetRpc(targetPos);
    }

    void OnMouseUp()
    {
        rb.useGravity = true;

        // Tell server we released it
        RequestSetHeldRpc(false);
    }

    // -------------------------
    // RPCs
    // -------------------------

    [ServerRpc]
    private void RequestSetHeldRpc(bool held)
    {
        isHeld.value = held;

        if (!held)
            rb.linearVelocity = Vector3.zero;
    }

    [ServerRpc]
    private void SendDragTargetRpc(Vector3 target)
    {
        if (!isHeld.value)
            return;

        Vector3 direction = target - rb.position;
        rb.linearVelocity = direction * dragSpeed;
    }
}