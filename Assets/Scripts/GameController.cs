using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController Player;
    public TrayController Tray;
    public GameObject DishesContainer;
    public Dish DishPrefab;

    public int StartingDishes;
    public static float LooseDishGravityScale = 0.7f;

    private static GameController Instance;
    private DynamicPooler<Dish> DishPooler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Instance.DishPooler = new DynamicPooler<Dish>(this.DishPrefab);
            Instance.DishPooler.ParentTransform = this.DishesContainer.transform;
        }
    }


    private void Start()
    {
        this.ResetGame();
    }


    public void ResetGame()
    {
        this.Player.ResetPlayer();
        this.Tray.ResetTray();
        this.DishPooler.DeactivateAll();

        for (int i = 0; i < Instance.StartingDishes; i++)
        {
            SpawnDishOnTray();
        }
    }


    public Dish SpawnDish()
    {
        Dish newDish = this.DishPooler.ActivateNext(); //  Instantiate<Dish>(Instance.DishPrefab);
        newDish.transform.rotation = Quaternion.identity;
        newDish.Rigidbody.velocity = Vector2.zero;
        newDish.Rigidbody.angularVelocity = 0f;

        return newDish;
    }

    public void SpawnDishOnTray()
    {
        Dish newDish = Instance.SpawnDish();
        Instance.Player.Tray.AddDish(newDish);
    }


    // TODO logic
    public void SpawnDishAtRandomLocation()
    {
        Dish newDish = Instance.SpawnDish();
        newDish.transform.position = Vector3.zero;
    }


    public static void NotifyDishJointBreak(Dish dish)
    {
        Instance.Tray.RemoveDish(dish);
    }


    public static void DespawnDish(Dish dish)
    {
        Instance.DishPooler.Deactivate(dish);
    }
}
