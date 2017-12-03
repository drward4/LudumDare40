using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishSpawnLocation : MonoBehaviour
{
    public Dish Dish;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().gameObject.layer == (int)GameLayers.Player && this.Dish != null)
        {
            GameController.PickUpDishFromLocation(this);
        }
    }
}
