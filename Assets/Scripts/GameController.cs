using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController Player;
    public Dish DishPrefab;

    public int StartingDishes;

    private static GameController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Start()
    {
        for (int i = 0; i < Instance.StartingDishes; i++)
        {
            SpawnDish();
        }
    }


    public static void SpawnDish()
    {
        Dish newDish = Instantiate<Dish>(Instance.DishPrefab);
        Instance.Player.Tray.AddDish(newDish);
    }

}
