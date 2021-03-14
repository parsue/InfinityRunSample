using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject mapBlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
            Instantiate(mapBlock, other.transform.position, Quaternion.identity);
    }
}
