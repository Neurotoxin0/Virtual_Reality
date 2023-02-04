using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Range(1f, 15f)]public float SpeedRatio = 10;    // give a range, for debug
    [Range(1f, 15f)]public float JumpRatio = 10;
    public TextMeshProUGUI CollectiblesCountText;
    public TextMeshProUGUI BilliardCountText;
    public TextMeshProUGUI DragStateText;
    public GameObject GameStateTextObject;

    [SerializeField]private Rigidbody rb;
    private float movementX, movementY, movementZ;
    private int collectibles_count, billiard_count;
    private bool jump_state, drag_state;

    private int collectibles = 12, billiards = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        collectibles_count = 0;
        billiard_count = 0;
        jump_state = false;
        drag_state = false;
        GameStateTextObject.SetActive(false);
    
        SetCollectiblesCountText();
        SetBilliardCountText();
        SetDragStateText();
    }
    
    void Update()
    {
        UpdateGameState();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, movementY, movementZ);
        rb.AddForce(movement * SpeedRatio);
        Jump();
    }


    void UpdateGameState()
    {
        // win
        if (collectibles_count >= collectibles && billiard_count >= billiards) 
        {
            TextMeshProUGUI Message = GameStateTextObject.GetComponent<TextMeshProUGUI>();
            Message.text = "You Win!";
            GameStateTextObject.SetActive(true);
            GameObject.Find("Player").SendMessage("SetTimerState"); 
        }

        // lose
        if (transform.position.y <= 0)
        {
            TextMeshProUGUI Message = GameStateTextObject.GetComponent<TextMeshProUGUI>();
            Message.text = "Game Over";
            GameStateTextObject.SetActive(true);
            //Destroy(gameObject, 1);    // should not destroy player -> avoid unaccessible cases
            GameObject.Find("Player").SendMessage("SetTimerState"); 
        }
    }

    // Update Score
    void SetBilliardCount() 
    { 
        billiard_count++;
        SetBilliardCountText();
    }

    void SetCollectiblesCount()
    {
        collectibles_count ++;
        SetCollectiblesCountText();
    }

    // Set Text
    void SetCollectiblesCountText() { CollectiblesCountText.text = "Collectibles Count: " + collectibles_count.ToString(); }
    void SetBilliardCountText() { BilliardCountText.text = "Billiard Ball Count: " + billiard_count.ToString(); }
    void SetDragStateText() { DragStateText.text = "Drag Mode: " + drag_state.ToString(); }
    

    // Input props
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementZ = movementVector.y;
    }

    void OnJump()
    {
        if (!jump_state && transform.position.y == 0.5) jump_state = true;  // can only jump when on the ground
    }
    void Jump()
    {
        if (jump_state) 
        {
            movementY = JumpRatio * Time.deltaTime * 100;
            jump_state = false;
        }
        else movementY = 0.0f;
    }

    void OnDrag()
    {
        if (!drag_state)    { rb.drag = 3; drag_state = true; }
        else                { rb.drag = 0; drag_state = false; }
        SetDragStateText();
    }


    // Collision Handler
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("PickUp")) 
        { 
            SetCollectiblesCount();
            //obj.gameObject.SetActive(false);
            Destroy(obj.gameObject);
        }
    }
}
