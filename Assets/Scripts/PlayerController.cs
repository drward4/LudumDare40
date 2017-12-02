using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D Player;
    public float GroundAcceleration = 60f;
    public float AirialAcceleration = 0f;
    public float MaxSpeed = 5f;
    public float JumpPower = 300f;
    public bool IsGrounded;

    private bool HandleJumpPressed = false;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            this.HandleJumpPressed = true;
        }
    }


    void FixedUpdate ()
    {
        float velocityMagnitude = this.Player.velocity.magnitude;
        float horizontalDirection = Input.GetAxis("Horizontal");

        if (this.HandleJumpPressed)
        {
            this.Player.AddForce(new Vector2(0f, this.JumpPower));
            this.HandleJumpPressed = false;
        }

        // TODO separate 2d and 3d max velocity
        if (Mathf.Abs(horizontalDirection) > 0 && velocityMagnitude < this.MaxSpeed)
        {
            if (this.IsGrounded)
            {
                this.Player.AddForce(new Vector2(horizontalDirection * this.GroundAcceleration, 0f));
            }
            else
            {
                this.Player.AddForce(new Vector2(horizontalDirection * this.AirialAcceleration, 0f));
            }
        }               
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.IsGrounded = true;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        this.IsGrounded = false;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        this.IsGrounded = true;
    }
}
