using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
            Destroy(other.transform.parent.gameObject);
    }
}
