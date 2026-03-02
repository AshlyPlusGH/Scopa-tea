using UnityEngine;

public class LIGHT_SetColor : MonoBehaviour
{
    [SerializeField] private Light target;

    [SerializeField] private Color customColor;

    void Awake(){  }

    public void SetColorCustom(){ target.color = customColor; }
    public void SetColorGreen(){ target.color = Color.green; }
    public void SetColorRed(){ target.color = Color.red; }
}