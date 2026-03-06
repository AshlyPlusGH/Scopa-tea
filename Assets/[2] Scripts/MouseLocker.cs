using UnityEngine;

public class MouseLocker : MonoBehaviour
{
    [SerializeField] private ENUM_MouseState onTriggerMouseState = ENUM_MouseState.Unlocked;

    [SerializeField] private bool isTriggeredOnAwake = true;

    void Awake(){ if (isTriggeredOnAwake){ Trigger(); }}

    public void Trigger()
    {
        switch (onTriggerMouseState)
        {
            case ENUM_MouseState.Locked:
                LockMouse();
                break;
            case ENUM_MouseState.Unlocked:
                UnlockMouse();
                break;
        }
    }

    public void LockMouse(){ Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    public void UnlockMouse(){ Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
}

enum ENUM_MouseState
{
    Locked,
    Unlocked
}