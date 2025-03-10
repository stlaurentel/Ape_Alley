using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Disable movement for remote players
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize(); // Ensures diagonal movement isn't faster

        if (moveInput.y > 0)
        {
            anim.SetInteger("facing", 1);
        }
        else if (moveInput.y < 0)
        {
            anim.SetInteger("facing", 2);
        }
        else if (moveInput.x > 0)
        {
            anim.SetInteger("facing", 3);
        }
        else if (moveInput.x < 0)
        {
            anim.SetInteger("facing", 4);
        } else
        {
            anim.SetInteger("facing", 0);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * moveInput);
    }
}
