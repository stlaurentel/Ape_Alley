using UnityEngine;

public class BreakoutInteractionText : MonoBehaviour
{
    public GameObject interactionUI; 
    public static BreakoutInteractionText Instance;
    public bool touchingSpace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        touchingSpace = false;
        Instance = this;

        if (interactionUI != null) {
            interactionUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (touchingSpace == true) {
            interactionUI.SetActive(true);
        }
        else if (touchingSpace == false) {
            interactionUI.SetActive(false);
        }
    }
}
