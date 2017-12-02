using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D PlayerBody;
    public TrayController Tray;

    public float GroundAcceleration = 60f;
    public float AirialAcceleration = 0f;
    public float MaxSpeed = 5f;
    public float JumpPower = 300f;
    public bool IsGrounded;

    private bool HandleJumpPressed = false;
    private Vector3 OriginalPosition;
    private Vector3 TrayOriginalPosition;

    private void Awake()
    {
        this.OriginalPosition = this.transform.position;
        this.TrayOriginalPosition = this.Tray.transform.position;
    }


    public void ResetPlayer()
    {
        this.transform.position = this.OriginalPosition;
        this.Tray.transform.position = this.TrayOriginalPosition;
        this.PlayerBody.velocity = Vector2.zero;
        this.PlayerBody.angularVelocity = 0f;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            this.HandleJumpPressed = true;
        }
    }


    void FixedUpdate ()
    {
        float velocityMagnitude = this.PlayerBody.velocity.magnitude;
        float horizontalDirection = Input.GetAxis("Horizontal");

        if (this.HandleJumpPressed)
        {
            this.PlayerBody.AddForce(new Vector2(0f, this.JumpPower));
            this.HandleJumpPressed = false;
        }

        // TODO separate 2d and 3d max velocity
        if (Mathf.Abs(horizontalDirection) > 0 && velocityMagnitude < this.MaxSpeed)
        {
            if (this.IsGrounded)
            {
                this.PlayerBody.AddForce(new Vector2(horizontalDirection * this.GroundAcceleration, 0f));
            }
            else
            {
                this.PlayerBody.AddForce(new Vector2(horizontalDirection * this.AirialAcceleration, 0f));
            }
        }               
	}


    private bool IsGroundCollision(Collision2D collision)
    {
        bool contacts = true;
        if (collision.contacts.Length > 0)
        {
            contacts = (collision.contacts[0].normal == Vector2.up);
        }

        return collision.collider.gameObject.layer == 9 &&  contacts;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.IsGroundCollision(collision))
        {
            this.IsGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
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
}
