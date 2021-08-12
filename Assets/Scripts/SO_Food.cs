using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Food", order = 1)]
public class SO_Food : ScriptableObject
{
    public FoodType type;
    public Sprite spriteImage;
    public bool jokeItem;
    public bool dessert;
    public bool vegetarian;
    public bool healthy;
    public bool junk;
    public bool raw;
}
