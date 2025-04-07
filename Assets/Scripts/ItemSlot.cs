using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public string name;
    public Sprite sprite;
    public bool isFull = false;

    private SpriteRenderer itemSpriteRenderer;

    void Start()
    {
        Transform child = transform.Find("ItemSprite");
        if (child != null)
        {
            itemSpriteRenderer = child.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("ItemSprite child not found on " + gameObject.name);
        }
    }

    public void AddItem(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
        itemSpriteRenderer.sprite = sprite;
        isFull = true;
    }
}
