using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class InspectorButtonToUnityEvent : MonoBehaviour
{
    public UnityEvent unityEvent;

    [Button] private void Activate(){ unityEvent.Invoke(); }
}