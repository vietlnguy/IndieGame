using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    public float moveSpeed = 3f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer moveRangeSR;
    private EdgeCollider2D moveRangeCollider;
    private Animator animator;
    private bool isHovered = false;
    private bool isSelected = false;
    private Camera mainCam;
    public int offset = 0; // Optional manual offset if needed
    private GameObject moveRange;
    private GameObject parentObj;
    private Rigidbody2D rigidBody;
    private Rigidbody2D[] rigidBodies;
    private Vector3 originalPosition;
    private SpriteRenderer attackRangeSR;
    private bool movedYet = false;
    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        rigidBody = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        parentObj = GameObject.Find("Characters");
        rigidBodies = parentObj.GetComponentsInChildren<Rigidbody2D>();

        moveRange = GameObject.Find("MoveRange");
        moveRangeSR = moveRange.GetComponent<SpriteRenderer>();
        moveRangeCollider = moveRange.GetComponent<EdgeCollider2D>();
        
        attackRangeSR = transform.Find("AttackRange").GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
        mainCam = Camera.main;
    }

    void Update()
    {
        HandleSelection();
        HandleMovement();
    }

    void LateUpdate()
    {
        // Multiply by -100 to invert Y (lower on screen = higher order)
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + offset;
    }

    void HandleSelection() {
        
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            // Character not selected yet and is clicked
            if (hit != null && hit.gameObject == gameObject && !isSelected) {
                isSelected = true;
                spriteRenderer.color = highlightColor;
                originalPosition = gameObject.transform.position;

                //Move Range stuff
                moveRange.transform.position = transform.position;
                //TODO moveRange.transform.scale. Scale with character stats.
                moveRangeSR.enabled = true;

                //Attack Range stuff
                //TODO attackRange.transform.scale. Scale with character stats.
                attackRangeSR.enabled = true;
            }
            
            //Character is already selected and clicked again
            else if (hit != null && hit.gameObject == gameObject && isSelected){
                //Do nothing
            }

            //Character is already selected and clicked enemy or interactable
            /*
            else if (hit != null && hit.gameObject == enemyGameObj && isSelected){
                - show combat UI
                - start combat
            }
            
            else if (hit != null && hit.gameObject == interactableObj && isSelected){
                - show interaction UI
                - start interaction
            }

            */

            //Clicked anything else
            else {
                //If character was selected then should deselect character etc.
                if (isSelected) {
                    spriteRenderer.color = originalColor;
                    animator.SetBool("isWalking", false);
                    
                    //Move Range stuff
                    moveRangeSR.enabled = false;

                    //Attack Range stuff
                    attackRangeSR.enabled = false;
                    
                    isSelected = false;
                }
            }
        }

    }

    void HandleMovement()
    {
        if (!isSelected) {
            return;
        }

        else {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction.y += 1;
            if (Input.GetKey(KeyCode.S)) direction.y -= 1;
            if (Input.GetKey(KeyCode.A)) direction.x -= 1;
            if (Input.GetKey(KeyCode.D)) direction.x += 1;

            if (direction != Vector3.zero)
            {
                transform.position += direction.normalized * moveSpeed * Time.deltaTime;
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (originalPosition != gameObject.transform.position && !movedYet) {
                //Switch other characters rigidbodies
                foreach (var r in rigidBodies) {
                    if (r != rigidBody ) {
                        r.simulated = !r.simulated;
                    }
                }

                //Enable the moveRangeCollider
                moveRangeCollider.enabled = true;


                //Bring up Confirmation UI,
                

            }
        }
    }

    void OnMouseEnter()
    {   
        if (!isSelected) {
            spriteRenderer.color = highlightColor;
        }

    }

    void OnMouseExit()
    {
        if (!isSelected){
            spriteRenderer.color = originalColor;
        }
    }


}
