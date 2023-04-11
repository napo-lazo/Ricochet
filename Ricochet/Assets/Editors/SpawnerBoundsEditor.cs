using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnerBoundsEditor : Editor
{
    private void OnSceneGUI()
    {
        Spawner spawner = (Spawner)target;
        BoxCollider spawnerCollider = spawner.GetComponent<BoxCollider>();
        Transform enemy = spawner.enemyToSpawn;
        MeshRenderer enemyMesh = enemy.GetComponent<MeshRenderer>();

        float enemyOffset = Mathf.Max(enemyMesh.bounds.size.x, enemyMesh.bounds.size.z);
        float finalOffset = enemyOffset + spawner.additionalEdgeOffset;
        Vector3 innerArea = new Vector3(spawnerCollider.size.x - finalOffset, spawnerCollider.size.y, spawnerCollider.size.z - finalOffset);

        if (enemy == null || innerArea.x <= enemyOffset || innerArea.z <= enemyOffset)
        {
            Handles.color = Color.red;
            Handles.DrawWireCube(spawner.transform.position, spawnerCollider.size);
        }
        else
        {
            Handles.color = Color.blue;
            Handles.DrawWireCube(spawner.transform.position, spawnerCollider.size);

            Handles.color = Color.magenta;
            Handles.DrawWireCube(spawner.transform.position, innerArea);
        }
    }
}
