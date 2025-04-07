using UnityEngine;

public class AnimationSync : MonoBehaviour
{
    public Animator playerAnimator;

    // turn this into a list
    public Animator glassesAnimator;
    public Animator clownHatAnimator;

    void Update()
    {
        if (playerAnimator != null && glassesAnimator != null)
        {
            // Get the "facing" parameter value from the player
            int facingValue = playerAnimator.GetInteger("facing");

            // Apply the same "facing" value to the glasses Animator
            glassesAnimator.SetInteger("facing", facingValue);
        }

        if (playerAnimator != null && glassesAnimator != null)
        {
            int facingValue = playerAnimator.GetInteger("facing");
            clownHatAnimator.SetInteger("facing", facingValue);
        }
    }
}
