using UnityEngine;
using PurrNet;

public class OBJECT_Draggable : NetworkBehaviour
{
    #region https://www.youtube.com/watch?v=saMBLw-JdHU Code Origin
    public float force = 600;
	public float damping = 6;
	public float distance = 15;

	private LineRenderer lr;
	private Transform lineRenderLocation;
    private Camera targetCam;

    private Vector3 dragPosition;
    private Vector3 originalDragPosition;
    private Vector3 dragStartPosition;

    public enum_OBJECT_DraggableState state {get; private set;} //NEEDS TO BE NETWORKED

    Transform jointTrans;
	float dragDepth;

    static string draggableLayerName = "Draggable";

    //Addition
    void Awake(){ Setup(); }
    void Setup()
    { 
        lr = FindFirstObjectByType<OBJECT_DraggableLineRenderer>().GetComponent<LineRenderer>();
        lineRenderLocation = lr.transform;

        targetCam = Camera.main;
        if (targetCam == null){ targetCam = FindFirstObjectByType<Camera>(); }

        DestroyRope();
    }
    //End Addition

    void OnMouseDown ()
	{
		HandleInputBegin (Input.mousePosition);
	}
	
	void OnMouseUp ()
	{
		HandleInputEnd (Input.mousePosition);
	}
	
	void OnMouseDrag ()
	{
		HandleInput (Input.mousePosition);
	}
	
	public void HandleInputBegin(Vector3 screenPosition)
    {
        state = enum_OBJECT_DraggableState.Dragging; //NETWORK SYNC

        jointTrans = null; // reset before starting a new drag

        var ray = targetCam.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(draggableLayerName))
            {
                dragDepth = CameraPlane.CameraToPointDepth(targetCam, hit.point);
                jointTrans = AttachJoint(hit.rigidbody, hit.point);
                dragStartPosition = transform.position;
                dragPosition = hit.point;
                originalDragPosition = dragPosition;
            }
        }

        lr.positionCount = 2;
    }
	
	public void HandleInput (Vector3 screenPosition)
	{
		if (jointTrans == null)
			return;
		var worldPos = targetCam.ScreenToWorldPoint (screenPosition);
		jointTrans.position = CameraPlane.ScreenToWorldPlanePoint (targetCam, dragDepth, screenPosition);

        dragPosition = originalDragPosition + (transform.position - dragStartPosition);

		DrawRope();
	}
	
	public void HandleInputEnd(Vector3 screenPosition)
    {
        DestroyRope();

        if (jointTrans != null)
        {
            Destroy(jointTrans.gameObject);
            jointTrans = null;
        }

        state = enum_OBJECT_DraggableState.Loose; //NETWORK SYNC
    }
	
	Transform AttachJoint (Rigidbody rb, Vector3 attachmentPosition)
	{
		GameObject go = new GameObject ("Attachment Point");
		go.hideFlags = HideFlags.HideInHierarchy; 
		go.transform.position = attachmentPosition;
		
		var newRb = go.AddComponent<Rigidbody> ();
		newRb.isKinematic = true;
		
		var joint = go.AddComponent<ConfigurableJoint> ();
		joint.connectedBody = rb;
		joint.configuredInWorldSpace = true;
		joint.xDrive = NewJointDrive (force, damping);
		joint.yDrive = NewJointDrive (force, damping);
		joint.zDrive = NewJointDrive (force, damping);
		joint.slerpDrive = NewJointDrive (force, damping);
		joint.rotationDriveMode = RotationDriveMode.Slerp;
		
		return go.transform;
	}
	
	private JointDrive NewJointDrive (float force, float damping)
	{
		JointDrive drive = new JointDrive ();
		drive.positionSpring = force;
		drive.positionDamper = damping;
		drive.maximumForce = Mathf.Infinity;
		return drive;
	}

	private void DrawRope()
	{
		if(jointTrans == null)
		{
			return;
		}

		lr.SetPosition(0, lineRenderLocation.position);
		lr.SetPosition(1, dragPosition);
    }

	private void DestroyRope()
	{
		lr.positionCount = 0;
	}
    #endregion
}

public enum enum_OBJECT_DraggableState
{
    None,
    Loose,
    Dragging
}