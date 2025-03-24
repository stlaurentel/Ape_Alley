using UnityEngine;
using Photon.Pun;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator anim;
    public Tilemap tilemap;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tilemap = tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();

        if (!tilemap)
        {
            Debug.Log("NO TILEMAP!");
        }

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
        if (IsPlayerInsideTilemapBounds())
        {
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * moveInput);
            //Debug.Log("in bounds");
        } else
        {
            Debug.Log("Out of bounds");
        }
    }

    private bool IsPlayerInsideTilemapBounds()
    {
        // Get the Tilemap's bounds
        BoundsInt bounds = tilemap.cellBounds;

        // Convert player's world position to local position relative to the tilemap
        Vector3Int playerCellPosition = tilemap.WorldToCell(rb.position + moveSpeed * Time.fixedDeltaTime * moveInput);

        // Check if the player is within the bounds of the tilemap
        return bounds.Contains(playerCellPosition);
    }
}
