using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public bool isFull = false;
    public bool selected = false;

    public Image image;
    public GameObject selectedShader;

    public CustomizePlayer customizePlayer;

    public void AddItem(string name, Sprite sprite)
    {
        itemName = name;
        image.sprite = sprite;
        isFull = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        if (eventData.button == PointerEventData.InputButton.Left && isFull)
        {
            Debug.Log("Selecting");
            if (selectedShader.activeInHierarchy)
            {
                selected = false;
                selectedShader.SetActive(false);
            } else
            {
                selected = true;
                selectedShader.SetActive(true);
            }

            EquipItem();

        }
    }

    private void EquipItem()
    {
        if (this.itemName == "clownHat")
        {
            Debug.Log("toggle clownHat");
            customizePlayer.ToggleClownHat();
        }
        else if (this.itemName == "eyepatch")
        {
            customizePlayer.ToggleEyepatch();
        }
    }

}