using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Food", order = 1)]
public class SO_Food : ScriptableObject
{
    public new string name;
    public Sprite spriteImage;
    public bool jokeItem;
    public bool dessert;
    public bool produce;
    public bool healthy;
    public bool junk;
    public bool raw;
}
