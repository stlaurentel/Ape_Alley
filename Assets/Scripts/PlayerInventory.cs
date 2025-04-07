using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //public List<string> ownedItems;
    //public GameObject itemSlot;
    public ItemSlot[] itemSlot;

    public bool hasEyepatch = true;
    public bool hasClownHat = false;

    // Update is called once per frame
    void Update()
    {
        
    }

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
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].name == otherName)
            {
                Debug.Log("Player already has item " + otherName);
                return true;
            }
        }
        return false;
    }
}
