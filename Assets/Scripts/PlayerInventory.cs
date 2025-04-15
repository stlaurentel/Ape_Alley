using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //public List<string> ownedItems;
    //public GameObject itemSlot;
    public ItemSlot[] itemSlot;

    public bool hasEyepatch = true;
    public bool hasClownHat = false;



    public void AddItem(string name, Sprite sprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(name, sprite);
                Debug.Log("Added item: " + name);
                return;
            }
            Debug.Log("All slots were full. Could not add item.");
        }
    }

    public bool HasItem(string otherName)
    {
        foreach (ItemSlot slot in itemSlot)
        {
            if (slot != null && slot.itemName == otherName)
            {
                Debug.Log("Player already has item " + otherName);
                return true;
            }
        }
        return false;
    }

    public void DeselectAll()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].selected = false;
        }
    }
}
