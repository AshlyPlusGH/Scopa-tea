using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class DEBUG_InspectorButtonToUnityEvent : MonoBehaviour
{
    public UnityEvent unityEvent;

    [Button] private void Activate(){ unityEvent.Invoke(); }
}