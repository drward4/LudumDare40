using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public Dish NextDish;
    public Rigidbody2D Rigidbody;

    private bool WasTossed;
    public void BeginToss(Vector2 velocity)
    {
        this.Rigidbody.velocity = velocity * Random.Range(0.6f, 0.9f) + Vector2.up * Random.Range(5f, 8f);
        this.Rigidbody.gravityScale = 1f;
        this.WasTossed = true;
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
        GameController.AddScore(-10);
        GameController.DespawnDish(this);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == (int)GameLayers.Ground)
        {
            this.BreakDish();
        }
    }


    private void CheckTrigger(Collider2D collision)    
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == (int)GameLayers.Bin)
        {
            if (collision.GetComponent<Collider2D>().gameObject.transform.parent.GetComponent<Bin>().CanScore)
            {
                GameController.DespawnDish(this);
                GameController.AddScore(20);
            }
        }
        else if (collision.GetComponent<Collider2D>().gameObject.layer == (int)GameLayers.BinArea)
        {
            if (collision.GetComponent<Collider2D>().gameObject.GetComponent<Bin>().CanScore)
            {
                this.gameObject.layer = (int)GameLayers.Scenery;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.CheckTrigger(collision);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        this.CheckTrigger(collision);
    }
}
