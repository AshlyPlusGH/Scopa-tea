using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UnityEventOnKey : MonoBehaviour
{
    public enum TriggerType { KeyDown, KeyUp, KeyHeld }

    [Header("Input Action")]
    public InputActionReference action;

    [Header("Trigger")]
    public TriggerType triggerType = TriggerType.KeyDown;

    [Header("Event")]
    public UnityEvent unityEvent;

    private void OnEnable()
    {
        if (action == null) return;

        switch (triggerType)
        {
            case TriggerType.KeyDown:
                action.action.started += OnActionStarted;
                break;

            case TriggerType.KeyUp:
                action.action.canceled += OnActionCanceled;
                break;

            case TriggerType.KeyHeld:
                action.action.performed += OnActionPerformed;
                break;
        }

        action.action.Enable();
    }

    private void OnDisable()
    {
        if (action == null) return;

        action.action.started -= OnActionStarted;
        action.action.canceled -= OnActionCanceled;
        action.action.performed -= OnActionPerformed;

        action.action.Disable();
    }

    private void OnActionStarted(InputAction.CallbackContext ctx)
    {
        if (triggerType == TriggerType.KeyDown)
            unityEvent.Invoke();
    }

    private void OnActionCanceled(InputAction.CallbackContext ctx)
    {
        if (triggerType == TriggerType.KeyUp)
            unityEvent.Invoke();
    }

    private void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        if (triggerType == TriggerType.KeyHeld)
            unityEvent.Invoke();
    }
}