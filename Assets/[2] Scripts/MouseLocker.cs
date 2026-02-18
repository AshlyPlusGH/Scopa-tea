using UnityEngine;

public class MouseLocker : MonoBehaviour
{
    void Awake(){ LockMouse(); }

    public void LockMouse(){ Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    public void UnlockMouse(){ Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
}