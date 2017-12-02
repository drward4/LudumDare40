﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayController : MonoBehaviour
{
    public Rigidbody2D Rigidbody;

    public float PlateSpacing = 0.3f;
    public float StartingBreakForce = 6f;
    public float LowestBreakForce = 1f;

    public AnimationCurve BreakForceCurve;

    private List<Dish> Dishes;

    private void Awake()
    {
        this.Dishes = new List<Dish>();
    }


    public void AddDish(Dish dish)
    {
        //float breakForce = Mathf.Clamp(this.StartingBreakForce - this.BreakForceDecrement * this.Dishes.Count, this.LowestBreakForce, this.StartingBreakForce);

        if (this.Dishes.Count > 0)
        {
            Dish lastDish = this.Dishes[this.Dishes.Count - 1];
            lastDish.NextDish = dish;
            dish.transform.position = lastDish.transform.position + Vector3.up * this.PlateSpacing;
            dish.Joint.connectedBody = lastDish.Rigidbody;
        }
        else
        {
            dish.transform.position = this.transform.position + Vector3.up * this.PlateSpacing;
            dish.Joint.connectedBody = this.Rigidbody;
        }

        this.Dishes.Add(dish);

        float d = this.BreakForceCurve.Evaluate(Mathf.Clamp(this.Dishes.Count / 10f, 0f, 1f));
        dish.Joint.breakForce = Mathf.Clamp(d * this.StartingBreakForce, this.LowestBreakForce, this.StartingBreakForce);
    }
}
