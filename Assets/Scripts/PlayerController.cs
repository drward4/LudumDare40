using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D PlayerBody;
    public TrayController Tray;

    public float GroundAcceleration = 60f;
    public float AirialAcceleration = 0f;
    public float MaxSpeed = 5f;
    public float JumpPower = 300f;
    public bool IsGrounded;

    private int ContactCount;
    private bool HandleJumpPressed = false;
    private Vector3 OriginalPosition;
    private Vector3 TrayRelativePosition;
    
    private void Awake()
    {
        this.OriginalPosition = this.transform.position;
        this.TrayRelativePosition = this.Tray.transform.position - this.transform.position;
    }


    public void ResetPlayer()
    {
        this.transform.position = this.OriginalPosition;
        this.Tray.transform.position = this.transform.position + this.TrayRelativePosition;
        this.PlayerBody.velocity = Vector2.zero;
        this.PlayerBody.angularVelocity = 0f;
        this.ContactCount = 0;
    }


    private void Update()
    {
        if (this.IsGrounded)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Jump"))
            {
                this.HandleJumpPressed = true;
            }
        }
    }


    void FixedUpdate ()
    {
        float velocityMagnitude = this.PlayerBody.velocity.magnitude;
        float horizontalDirection = Input.GetAxis("Horizontal");

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            float mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - this.transform.position.x;
            horizontalDirection = Mathf.Clamp(mouseDirection / 2f, -1f, 1f);
        }

        if (this.HandleJumpPressed)
        {
            this.PlayerBody.AddForce(new Vector2(0f, this.JumpPower));
            this.HandleJumpPressed = false;
        }


        if (Mathf.Abs(horizontalDirection) > 0 && velocityMagnitude < this.MaxSpeed)
        {
            if (this.IsGrounded)
            {
                this.PlayerBody.AddForce(new Vector2(horizontalDirection * this.GroundAcceleration, 0f));
            }
            else if (this.ContactCount == 0) // Make sure we can't stick ourself to walls
            {
                this.PlayerBody.AddForce(new Vector2(horizontalDirection * this.AirialAcceleration, 0f));
            }
        }
	}


    private bool IsGroundCollision(Collision2D collision)
    {
        return collision.collider.gameObject.layer == (int)GameLayers.Ground;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == (int)GameLayers.Ground)
        {
            this.ContactCount = collision.contacts.Length;
        }

        if (this.IsGroundCollision(collision))
        {
            this.IsGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == (int)GameLayers.Ground)
        {
            this.ContactCount = collision.contacts.Length;
        }

        if (this.IsGroundCollision(collision))
        {
            this.IsGrounded = false;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (this.IsGroundCollision(collision))
        {
            this.IsGrounded = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == (int)GameLayers.BinArea)
        {
            Bin bin = collision.GetComponent<Collider2D>().gameObject.GetComponent<Bin>();
            if (bin.IsAvailable)
            {
                bin.WashCycle();
                this.Tray.DropAllDishes();
            }
        }
    }
}
