using UnityEngine;

public class MapBlockObjectsSpawner : MonoBehaviour
{
    [SerializeField] private bool isStartMap;
    [SerializeField] [Range(0, 10)] private int startDelaySpawn = 5;
    [SerializeField] [Range(0.1f, 0.9f)] private float startSpawnRatio = 0.35f;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float mapLength = 125f;
    [SerializeField] private float startZ = 3.75f;
    [SerializeField] private float objInterval = 7.5f;

    private float TrackWidth => GameManager.Instance.trackWidth;
    private float SpawnRatio => Mathf.Clamp01(startSpawnRatio + GameManager.spawnRatio);

    private void Start()
    {
        SetObjects();
    }

    private void SetObjects()
    {
        int c = objects.Length;
        float objZ = transform.position.z + startZ;
        float maxZ = objZ + mapLength;
        if (isStartMap) objZ += objInterval * startDelaySpawn;
        while (objZ < maxZ)
        {
            if (Random.value <= SpawnRatio)
            {
                int o = Random.Range(0, c);
                float x = TrackWidth * Random.Range(-1, 2);
                Vector3 pos = new Vector3(x, objects[o].transform.position.y, objZ);
                Instantiate(objects[o], pos, Quaternion.identity, transform);
            }
            objZ += objInterval;
        }
    }
}
