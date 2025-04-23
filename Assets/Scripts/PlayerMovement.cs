using UnityEngine;
using Photon.Pun;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Animator playerAnimator;
    public Animator eyePatchAnimator;
    public Animator clownHatAnimator;

    public Tilemap tilemap;
    public Tilemap oob;

    public bool canMove = true;
    public bool playerTyping = false;
    private Vector2 lastMove;

    private CustomizePlayer customization;
    private PlayerInventory inventory;
    public GameObject popupBubble;

    // clothing here
    public GameObject EyewearSlot;
    public GameObject inventoryCanvas;

    void Start()
    {
        inventory = transform.Find("PlayerInventory").GetComponent<PlayerInventory>();

        rb = GetComponent<Rigidbody2D>();

        playerAnimator = GetComponent<Animator>();

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

    [PunRPC]
    public void SetTypingState(bool isTyping) {
        if (!photonView.IsMine) return;
        enabled = !isTyping;
        playerTyping = isTyping;
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

        if (ePress && !(playerTyping))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeInHierarchy);

            if (inventoryCanvas.activeInHierarchy) {
                moveInput = new Vector2(0, 0); 
                canMove = false;
                
            } else
            {
                canMove = true;
            }
        }

        if (canMove)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize(); // Ensures diagonal movement isn't faster
        }

        if (moveInput != lastMove)
        {
            photonView.RPC("AnimationDirection", RpcTarget.Others, moveInput);
            AnimationDirection(moveInput);
            lastMove = moveInput;
        }

        //photonView.RPC("AnimationDirection", RpcTarget.Others, moveInput);

        //AnimationDirection(moveInput);
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

        if (other.CompareTag("Item"))
        {
            Debug.Log("Player is touching an obtainable item.");
            Item item = other.GetComponent<Item>();
            if (!inventory.HasItem(item.itemName))
            {
                inventory.AddItem(item.itemName, item.sprite);
                CreateBubblePopup("Collected " + item.itemName + "!", rb.position);
            }
        }
        
        if (other.CompareTag("BreakoutSpace")) {
            BreakoutMinigameTrigger.Instance.touchingSpace = true;
            Debug.Log("Touching BreakoutSpace");

            BreakoutMinigameTrigger.Instance.triggerCanvas.SetActive(true);
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

    public void CreateBubblePopup(string message, Vector3 position)
    {
        GameObject bubble = Instantiate(popupBubble, position, Quaternion.identity);
        PopupBubble popup = bubble.GetComponent<PopupBubble>();
        popup.Show(message);
    }

    [PunRPC]
    public void AnimationDirection(Vector2 move)
    {
        // this is going to be shrunk down
        // just have it like this to test the separate animators
        if (move.y > 0)
        {
            playerAnimator.SetInteger("facing", 1);
            eyePatchAnimator.SetInteger("facing", 1);
            clownHatAnimator.SetInteger("facing", 1);
        }
        else if (move.y < 0)
        {
            playerAnimator.SetInteger("facing", 2);
            eyePatchAnimator.SetInteger("facing", 2);
            clownHatAnimator.SetInteger("facing", 2);
        }
        else if (move.x > 0)
        {
            playerAnimator.SetInteger("facing", 3);
            eyePatchAnimator.SetInteger("facing", 3);
            clownHatAnimator.SetInteger("facing", 3);
        }
        else if (move.x < 0)
        {
            playerAnimator.SetInteger("facing", 4);
            eyePatchAnimator.SetInteger("facing", 4);
            clownHatAnimator.SetInteger("facing", 4);
        }
        else
        {
            playerAnimator.SetInteger("facing", 0);
            eyePatchAnimator.SetInteger("facing", 0);
            clownHatAnimator.SetInteger("facing", 0);
        }
    }

}