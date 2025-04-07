using UnityEngine;
using Photon.Pun;
using UnityEngine.Tilemaps;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviourPun
{

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator playerAnimator;
    private Animator eyePatchAnimator;

    public Tilemap tilemap;
    public Tilemap oob;

    public bool canMove = true;

    private CustomizePlayer customization;

    // clothing here
    public GameObject EyewearSlot;
    public GameObject inventory;

    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();

        playerAnimator = GetComponent<Animator>();
        eyePatchAnimator = transform.Find("EyewearSlot").GetComponent<Animator>();
        print(eyePatchAnimator);
        if (eyePatchAnimator != null)
        {
            eyePatchAnimator.Play("eyepatch_down"); // Ensure it starts with an animation
            print("playing eyepatch_down");
        }

        customization = GetComponent<CustomizePlayer>();

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
        var ePress = Input.GetKeyDown(KeyCode.E);
        var oPress = Input.GetKeyDown(KeyCode.O);

        if (oPress && EyewearSlot != null)
        {
            EyewearSlot.SetActive(!EyewearSlot.activeInHierarchy);
            customization.ToggleEyepatch();
        }

        if (ePress)
        {
            inventory.SetActive(!inventory.activeInHierarchy);

            if (inventory.activeInHierarchy) {
                canMove = false;
            } else
            {
                canMove = true;
            }
        }

        if (!canMove) return;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize(); // Ensures diagonal movement isn't faster

        // this is going to be shrunk down & placed in its own function
        // just have it like this to test the separate animators
        if (moveInput.y > 0)
        {
            playerAnimator.SetInteger("facing", 1);
            eyePatchAnimator.SetInteger("facing", 1);
        }
        else if (moveInput.y < 0)
        {
            playerAnimator.SetInteger("facing", 2);
            eyePatchAnimator.SetInteger("facing", 2);
        }
        else if (moveInput.x > 0)
        {
            playerAnimator.SetInteger("facing", 3);
            eyePatchAnimator.SetInteger("facing", 3);
        }
        else if (moveInput.x < 0)
        {
            playerAnimator.SetInteger("facing", 4);
            eyePatchAnimator.SetInteger("facing", 4);
        } else
        {
            playerAnimator.SetInteger("facing", 0);
            eyePatchAnimator.SetInteger("facing", 0);
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
        if (other.CompareTag("Door"))
        {
            Door door = other.GetComponent<Door>();  // Get the Door component
            if (door != null)  // Ensure the component exists
            {
                transform.position = new Vector2(door.x_coord, door.y_coord);
                Debug.Log("Player entered a door.");
            }
            else
            {
                Debug.LogWarning("No Door component found on the object with the 'Door' tag.");
            }
        }
        if (other.CompareTag("BreakoutSpace")) {
            BreakoutMinigameTrigger.Instance.touchingSpace = true;
            BreakoutInteractionText.Instance.touchingSpace = true;
            Debug.Log("Touching BreakoutSpace");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            Debug.Log("Player left the door.");
        }
        if (other.CompareTag("BreakoutSpace")) {
            BreakoutMinigameTrigger.Instance.touchingSpace = false;
            BreakoutInteractionText.Instance.touchingSpace = false;
            Debug.Log("Off BreakoutSpace");
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
