using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public Dish NextDish;
    public Rigidbody2D Rigidbody;
    public HingeJoint2D Joint;

    public bool IsOnTray()
    {
        return this.GetComponent<HingeJoint2D>() != null;
    }

    public void DestroyJoint()
    {
        if (this.IsOnTray())
        {
            Destroy(this.Joint);
        }

        if (this.NextDish != null)
        {
            this.NextDish.DestroyJoint();
            this.NextDish = null;
        }

        this.Rigidbody.gravityScale = GameController.LooseDishGravityScale;
        GameController.NotifyDishJointBreak(this);
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        this.DestroyJoint();
    }


    public void BreakDish()
    {
        GameController.DespawnDish(this);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.IsOnTray())
        {
            if (collision.collider.gameObject.layer == (int)GameLayers.Obstacle) 
            {
                this.DestroyJoint();
            }
        }
        else
        {
            if (collision.collider.gameObject.layer == (int)GameLayers.Ground)
            {
                this.BreakDish();
            }
            else if (collision.collider.gameObject.layer == (int)GameLayers.Player) 
            {
                GameController.GivePlayerDish(this);
            }
        }
    }
}
