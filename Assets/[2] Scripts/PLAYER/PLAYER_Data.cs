using TMPro;
using UnityEngine;

public class PLAYER_Data : MonoBehaviour
{
    public static ENUM_playerRole playerRole = ENUM_playerRole.Survivor;

    void Awake()
    {
        playerRole = ENUM_playerRole.Survivor;
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(OnChanged);
    }

    void OnChanged(int index)
    {
        SetPlayerRole(index);
    }

    public void SetPlayerRole(int index){ if (index == 0){ playerRole = ENUM_playerRole.Survivor; return; } if (index == 1){ playerRole = ENUM_playerRole.Monster; return; }}
}

public enum ENUM_playerRole
{
    Survivor,
    Monster
}