using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    //public Sprite sprite;
    public bool isFull = false;

    public Image image;

    public GameObject selectedShader;
    public bool selected = false;

    public CustomizePlayer customizePlayer;

    void Start()
    {
        
    }

    public void AddItem(string name, Sprite sprite)
    {
        //this.name = name;
        itemName = name;
        //this.sprite = sprite;
        image.sprite = sprite;
        isFull = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
        if (eventData.button == PointerEventData.InputButton.Left && isFull)
        {
            Debug.Log("Selecting");
            if (!selected)
            {
                customizePlayer.inventory.DeselectAll();
                selectedShader.SetActive(true);
                selected = true;
                EquipItem();
            }
            else
            {
                customizePlayer.inventory.DeselectAll();
                EquipItem();
            }

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