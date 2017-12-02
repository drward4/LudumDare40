using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayController : MonoBehaviour
{
    public Rigidbody2D Rigidbody;

    public float PlateSpacing = 0.3f;
    public float StartingBreakForce = 6f;
    public float LowestBreakForce = 1f;
    public float DishUpperLimitCount = 10f;

    public float LowestAngleLimit = 5f;

    public AnimationCurve BreakForceCurve;

    private List<Dish> Dishes;


    public int DishCount { get { return this.Dishes.Count; } }

    private void Awake()
    {
        this.Dishes = new List<Dish>();
    }


    public void ResetTray()
    {
        for (int i = 0; i < this.Dishes.Count; i++)
        {
            this.Dishes[i].DestroyJoint();
        }

        this.Dishes.Clear();

        this.Rigidbody.velocity = Vector2.zero;
        this.Rigidbody.angularVelocity = 0f;
    }


    public void AddDish(Dish dish)
    {
        //float breakForce = Mathf.Clamp(this.StartingBreakForce - this.BreakForceDecrement * this.Dishes.Count, this.LowestBreakForce, this.StartingBreakForce);
        if (this.Dishes.Contains(dish))
        {
            Debug.Log("Possible error, " + dish.name + " already in dishes list");
            return;
        }

        if (dish.GetComponent<HingeJoint2D>() != null)
        {
            Destroy(dish.Joint);
        }

        dish.Joint = dish.gameObject.AddComponent<HingeJoint2D>();
        dish.Joint.limits = new JointAngleLimits2D { min = -this.LowestAngleLimit, max = this.LowestAngleLimit };
        dish.Joint.enableCollision = true;
        dish.Rigidbody.gravityScale = 0f;
        dish.transform.rotation = Quaternion.identity;
        dish.Rigidbody.velocity = this.Rigidbody.velocity;
        dish.Rigidbody.angularVelocity = 0f;
        dish.Joint.breakForce = 1000f; // Helps make sure it doesn't fall off right away?

        if (this.Dishes.Count > 0)
        {
            Dish lastDish = this.Dishes[this.Dishes.Count - 1];
            lastDish.NextDish = dish;
            dish.Joint.connectedBody = lastDish.Rigidbody;
            dish.transform.position = lastDish.transform.position + lastDish.transform.up * this.PlateSpacing;
        }
        else
        {
            dish.transform.position = this.transform.position + Vector3.up * this.PlateSpacing;
            dish.Joint.connectedBody = this.Rigidbody;
        }

        this.Dishes.Add(dish);

        float d = this.BreakForceCurve.Evaluate(Mathf.Clamp(this.Dishes.Count / this.DishUpperLimitCount, 0f, 1f));
        dish.Joint.breakForce = Mathf.Clamp(d * this.StartingBreakForce, this.LowestBreakForce, this.StartingBreakForce);
    }


    public void RemoveDish(Dish dish)
    {
        if (this.Dishes.Contains(dish))
        {
            this.Dishes.Remove(dish);
        }
    }
}
