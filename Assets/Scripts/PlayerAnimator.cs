using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviourPun
{
    private Animator animator;
    private Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);
        animator.SetBool("isMoving", moveInput != Vector2.zero);
    }
}
