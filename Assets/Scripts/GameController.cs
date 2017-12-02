using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float LooseDishGravityScale = 0.7f;

    public PlayerController Player;
    public TrayController Tray;
    public GameObject DishesContainer;
    public GameObject DishSpawnPointsContainer;
    public Dish DishPrefab;
    public int StartingDishes;
    public float DishSpawnInterval;

    private static GameController Instance;
    private DynamicPooler<Dish> DishPooler;
    private List<DishSpawnLocation> FilledSpawnLocations;
    private List<DishSpawnLocation> EmptySpawnLocations;
    private float SpawnTimeRemaining = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this.DishPooler = new DynamicPooler<Dish>(this.DishPrefab);
            this.DishPooler.ParentTransform = this.DishesContainer.transform;
            this.FilledSpawnLocations = new List<DishSpawnLocation>();
            this.EmptySpawnLocations = new List<DishSpawnLocation>();
        }
    }


    private void Start()
    {
        this.ResetGame();
    }


    public void ResetGame()
    {
        this.SpawnTimeRemaining = this.DishSpawnInterval;

        this.EmptySpawnLocations.Clear();
        this.FilledSpawnLocations.Clear();
        this.DishSpawnPointsContainer.GetComponentsInChildren<DishSpawnLocation>(true, this.EmptySpawnLocations);

        for (int i = 0; i < this.EmptySpawnLocations.Count; i++)
        {
            if (this.EmptySpawnLocations[i].Dish != null)
            {
                this.EmptySpawnLocations[i].Dish = null;
            }
        }

        this.Player.ResetPlayer();
        this.Tray.ResetTray();
        this.DishPooler.DeactivateAll();

        this.SpawnDishAtRandomLocation();

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
        GivePlayerDish(newDish);
    }


    public static void GivePlayerDish(Dish dish)
    {
        dish.gameObject.layer = (int)GameLayers.Dishes;
        Instance.Player.Tray.AddDish(dish);
    }


    public static void PickUpDishFromLocation(DishSpawnLocation location)
    {
        if (Instance.FilledSpawnLocations.Contains(location) && location.Dish != null)
        {
            GivePlayerDish(location.Dish);
            location.Dish = null;

            Instance.FilledSpawnLocations.Remove(location);
            Instance.EmptySpawnLocations.Add(location);
        }
        else
        {
            Debug.LogError("did something go wrong here?");
        }
    }


    // TODO logic
    public void SpawnDishAtRandomLocation()
    {
        if (this.EmptySpawnLocations.Count == 0)
            return;
           
        Dish newDish = Instance.SpawnDish();
        newDish.Rigidbody.gravityScale = 0f;
        newDish.gameObject.layer = (int)GameLayers.Scenery;

        int index = Random.Range(0, this.EmptySpawnLocations.Count);
        DishSpawnLocation location = this.EmptySpawnLocations[index];        
        this.EmptySpawnLocations.RemoveAt(index);
        this.FilledSpawnLocations.Add(location);

        location.Dish = newDish;
        newDish.transform.position = location.transform.position;
    }


    public static void NotifyDishJointBreak(Dish dish)
    {
        Instance.Tray.RemoveDish(dish);
    }


    public static void DespawnDish(Dish dish)
    {
        Instance.DishPooler.Deactivate(dish);
    }


    private void Update()
    {
        this.SpawnTimeRemaining -= Time.deltaTime;
        if (this.SpawnTimeRemaining <= 0f)
        {
            this.SpawnTimeRemaining += this.DishSpawnInterval;
            this.SpawnDishAtRandomLocation();
        }
    }
}
