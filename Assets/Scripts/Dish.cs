using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public Dish NextDish;
    public Rigidbody2D Rigidbody;
    public HingeJoint2D Joint;

    public void DestroyJoint()
    {
        if (this.GetComponent<HingeJoint2D>() != null)
        {
            Destroy(this.Joint);
        }

        // TODO destroy joints recursively
        this.NextDish = null;

        this.Rigidbody.gravityScale = GameController.LooseDishGravityScale;
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        this.Rigidbody.gravityScale = GameController.LooseDishGravityScale;
        Debug.Log("A joint has just been broken!, force: " + joint.breakForce);
    }
}
