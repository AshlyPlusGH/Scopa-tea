using PurrNet;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventIfServer : NetworkBehaviour
{
    [SerializeField] private UnityEvent unityEvent;

    public void Trigger(){ if (isServer){ unityEvent.Invoke(); }}
}