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
    private bool inShed;
    public Tilemap tilemap;
    public Tilemap oob;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        oob = GameObject.Find("OOB").GetComponent<Tilemap>();

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
        if (IsPlayerMovingIntoValidTile())
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
        BoundsInt bounds = tilemap.cellBounds;

        // Convert player's world position to local position relative to the tilemap
        Vector3Int playerCellPosition = tilemap.WorldToCell(rb.position + moveSpeed * Time.fixedDeltaTime * moveInput);

        return bounds.Contains(playerCellPosition);
    }

    private bool IsPlayerMovingIntoValidTile()
    {
        Vector3Int targetCellPosition = tilemap.WorldToCell(rb.position + moveSpeed * Time.fixedDeltaTime * moveInput);

        TileBase walkableTile = tilemap.GetTile(targetCellPosition);  // Tile in Tilemap
        TileBase oobTile = oob.GetTile(targetCellPosition);    // Tile in OOB

        // Allow movement if it's inside Tilemap but not in OOB
        return walkableTile != null && oobTile == null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("within");
        if (other.CompareTag("Shed"))
        {
            inShed = true;
            Debug.Log("Player entered the shed.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shed"))
        {
            inShed = false;
            Debug.Log("Player left the shed.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("Wall collision");
        }
    }


}
