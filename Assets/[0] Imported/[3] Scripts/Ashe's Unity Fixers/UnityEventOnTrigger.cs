using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;
    [SerializeField] private bool triggerOn2D = true;
    [SerializeField] private bool triggerOn3D = true;

    //2D
    void OnTriggerEnter2D(Collider2D other){ if (triggerOn2D){ return; } unityEvent.Invoke(); }
    void OnCollisionEnter2D(Collision2D other){ if (triggerOn2D){ return; } unityEvent.Invoke(); }

    //3D
    void OnTriggerEnter(Collider other){ if (triggerOn3D){ return; } unityEvent.Invoke(); }
    void OnCollisionEnter(Collision other){ if (triggerOn3D){ return; } unityEvent.Invoke(); }
}