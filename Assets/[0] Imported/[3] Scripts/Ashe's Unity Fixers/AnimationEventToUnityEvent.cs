using UnityEngine;
using UnityEngine.Events;

public class AnimationEventToUnityEvent : MonoBehaviour
{
    public UnityEvent unityEvent;

    public void RelayEvent()
    {
        unityEvent.Invoke();
    }
}