using UnityEngine;

public class AnimationSync : MonoBehaviour
{
    public Animator playerAnimator;  // Reference to the player's Animator
    public Animator glassesAnimator; // Reference to the glasses' Animator

    void Update()
    {
        if (playerAnimator != null && glassesAnimator != null)
        {
            // Get the "facing" parameter value from the player
            int facingValue = playerAnimator.GetInteger("facing");

            // Apply the same "facing" value to the glasses Animator
            glassesAnimator.SetInteger("facing", facingValue);
        }
    }
}
