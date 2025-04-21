using UnityEngine;
using System.Collections.Generic;


public class MainPlayerController : MonoBehaviour
{
    private GameObject moveRange;
    private SpriteRenderer moveRangeSR;
    private EdgeCollider2D moveRangeCollider;
    //private Animator animator;
    private Camera mainCam;
    private bool othersLocked = false;
    private bool isSelected = false;
    private PlayerController reporter;
    private Vector3 originalPosition;
    private GameObject characterSelected;
    private Color originalColor;
    private float moveSpeed = 8.0f;
    void Awake() 
    {
    }
    void Start()
    {

        moveRange = GameObject.Find("MoveRange");
        moveRangeSR = moveRange.GetComponent<SpriteRenderer>();
        moveRangeCollider = moveRange.GetComponent<EdgeCollider2D>();
        mainCam = Camera.main;
        
    }
    void Update()
    {
        HandleSelection();
        HandleMovement();
    }
    void HandleSelection() {
        
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);

            //No character selected yet
            if (hit != null && hit.gameObject.tag == "character" && !isSelected) {
                isSelected = true;
                characterSelected = GameObject.Find(hit.gameObject.name);

                SpriteRenderer sr = characterSelected.GetComponent<SpriteRenderer>();
                originalColor = sr.color;
                sr.color = Color.yellow;
                
                originalPosition = characterSelected.transform.position;

                //Lock position of all other characters
                if (!othersLocked) {lockOtherCharacterMovement();}

                //Move Range stuff
                moveRange.transform.position = characterSelected.transform.position;
                //TODO moveRange.transform.scale. Scale with character stats.
                moveRangeSR.enabled = true;
                moveRangeCollider.enabled = true;

                //Attack Range stuff
                //TODO attackRange.transform.scale. Scale with character stats.
                SpriteRenderer attackRangeSR = characterSelected.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
                attackRangeSR.enabled = true;
            }
            
            //Same character is selected again
            else if (hit != null && hit.gameObject.tag == "character" && isSelected && hit.gameObject.name == characterSelected.name){
                //Do nothing
            }

            //Existing character selection but clicked a different character
            else if (hit != null && hit.gameObject.tag == "character" && isSelected && hit.gameObject.name != characterSelected.name){
                isSelected = true;
                
                //Disable existing character select Attack Range and unlock others
                SpriteRenderer attackRangeSR = characterSelected.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
                attackRangeSR.enabled = false;
                if (othersLocked) {
                    unlockOtherCharacterMovement();
                }

                //Reassign character selected
                characterSelected = GameObject.Find(hit.gameObject.name);

                if (!othersLocked) {lockOtherCharacterMovement();}

                //Move Range stuff
                moveRange.transform.position = characterSelected.transform.position;
                //TODO moveRange.transform.scale. Scale with character stats.
                moveRangeSR.enabled = true;
                moveRangeCollider.enabled = true;

                //Attack Range stuff
                //TODO attackRange.transform.scale. Scale with character stats.
                attackRangeSR = characterSelected.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
                attackRangeSR.enabled = true;

            }

            /*
            //Existing character selection and clicked enemy or interactable
            else if (hit != null && hit.gameObject == enemyGameObj && isSelected){
                - show combat UI
                - start combat
            }
            */
            
            //Existing character selection, clicked anything else
            else {
                //If character was selected then should deselect character etc.
                if (isSelected) {
                    if (othersLocked) {
                        unlockOtherCharacterMovement();
                    }
                    
                    SpriteRenderer sr = characterSelected.GetComponent<SpriteRenderer>();
                    sr.color = Color.white;

                    Animator animator = characterSelected.GetComponent<Animator>();
                    animator.SetBool("isWalking", false);
                    
                    //Move Range stuff
                    moveRangeSR.enabled = false;
                    moveRangeCollider.enabled = false;

                    //Attack Range stuff
                    SpriteRenderer attackRangeSR = characterSelected.transform.Find("AttackRange").GetComponent<SpriteRenderer>();
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
                characterSelected.transform.position += direction.normalized * moveSpeed * Time.deltaTime;
                characterSelected.GetComponent<Animator>().SetBool("isWalking", true);
            }
            else
            {
                characterSelected.GetComponent<Animator>().SetBool("isWalking", false);
            }
        }
    }
    void lockOtherCharacterMovement() {
        foreach (Transform obj in this.transform) {
            if (obj.name != characterSelected.name) {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        othersLocked = true;

    }
    void unlockOtherCharacterMovement() {
        foreach (Transform obj in transform) {
            if (obj.name != characterSelected.name) {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
        }
        othersLocked = false;
    }
    public void OnChildMouseEnter(GameObject child, Color originalColor) {
        if (!isSelected) {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = Color.yellow;
        }

    }
    public void OnChildMouseExit(GameObject child, Color originalColor) {
        if (!isSelected) {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = originalColor;
        }
    }
}
