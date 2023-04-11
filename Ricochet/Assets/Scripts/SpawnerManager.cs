using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Min(0)]
    public int amountOfEnemies;
    [Min(0)]
    public float timeBetweenSpawans;
    public List<GameObject> spawners;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
