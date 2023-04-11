using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform enemyToSpawn;
    [Min(0)]
    public float additionalEdgeOffset;

}
