using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnAwake : MonoBehaviour
{
    public UnityEvent unityEvent;

    void Awake(){ unityEvent.Invoke(); }
}