using UnityEngine;
using UnityEngine.UI;

public class UI_ISlot : MonoBehaviour
{
    public Image image;

    public void UpdateUI(Sprite sprite){ image.sprite = sprite; }
}