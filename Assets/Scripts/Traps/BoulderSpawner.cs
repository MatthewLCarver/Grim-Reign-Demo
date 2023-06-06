using UnityEngine;


public class BoulderSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boulder;
    public Vector3 boulderSpawnPosition;
    
    /// <summary>
    /// Spawns the boulder 
    /// </summary>
    public void Spawn()
    {
        Instantiate(boulder, 
            transform.position + boulderSpawnPosition,
            Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }
}
