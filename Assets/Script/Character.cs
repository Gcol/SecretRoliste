using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string name;
    public string description;

    public int nbHealth;
    public int nbReason;


    public int statEtrange;
    public int statDemon;
    public int statRational;
}
