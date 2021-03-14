using UnityEngine;

[CreateAssetMenu()]
public class GrowUpProfile : ScriptableObject
{
    [Range(0.001f, 0.2f)] public float playerGrowRatio = 0.04f;
    [Range(0.001f, 0.2f)] public float spawnGrowRatio = 0.001f;
}
