using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public Dish NextDish;
    public Rigidbody2D Rigidbody;
    //public HingeJoint2D Joint;

    //public bool IsOnTray()
    //{
    //    return this.GetComponent<HingeJoint2D>() != null;
    //}

    //public void DestroyJoint()
    //{
    //    if (this.IsOnTray())
    //    {
    //        Destroy(this.Joint);
    //    }

    //    if (this.NextDish != null)
    //    {
    //        this.NextDish.DestroyJoint();
    //        this.NextDish = null;
    //    }

    //    this.Rigidbody.gravityScale = GameController.LooseDishGravityScale;
    //    GameController.NotifyDishJointBreak(this);
    //}

    //private void OnJointBreak2D(Joint2D joint)
    //{
    //    this.DestroyJoint();
    //}


    private bool WasTossed;
    public void BeginToss(Vector2 velocity)
    {
        this.Rigidbody.velocity = velocity * Random.Range(0.6f, 0.9f) + Vector2.up * Random.Range(5f, 8f);
        this.Rigidbody.gravityScale = 1f;
        this.WasTossed = true;
    //    this.gameObject.layer = (int)GameLayers.Dishes;
    }


    private void Update()
    {
        if (this.WasTossed && this.Rigidbody.velocity.y < 0f)
        {
            this.gameObject.layer = (int)GameLayers.Dishes;
            this.WasTossed = false;
        }
    }


    public void BreakDish()
    {
        GameController.DespawnDish(this);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == (int)GameLayers.Ground)
        {
            this.BreakDish();
        }
        else if (collision.collider.gameObject.layer == (int)GameLayers.Player)
        {
            //GameController.GivePlayerDish(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == (int)GameLayers.Bin)
        {
            GameController.DespawnDish(this);
            GameController.AddScore(10);
        }
        else if (collision.GetComponent<Collider2D>().gameObject.layer == (int)GameLayers.BinArea)
        {
            this.gameObject.layer = (int)GameLayers.Scenery;
        }
    }
}
