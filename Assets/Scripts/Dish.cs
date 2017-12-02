using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public Dish NextDish;
    public Rigidbody2D Rigidbody;
    public HingeJoint2D Joint;

    public bool HasJoint()
    {
        return this.GetComponent<HingeJoint2D>() != null;
    }

    public void DestroyJoint()
    {
        if (this.HasJoint())
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
        if (collision.collider.gameObject.layer == 9 && !this.HasJoint())
        {
            Debug.Log("break " + this.name);
            this.BreakDish();
        }
    }
}
