using UnityEngine;

public class PositionOffset : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private bool autoOffset = true;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool x, y, z;

    private void Start()
    {
        if (Target && autoOffset)
        {
            offset = transform.position - Target.position;
            if (!x) offset.x = 0;
            if (!y) offset.y = 0;
            if (!z) offset.z = 0;
        }
    }

    private void Update()
    {
        if (x) transform.position = transform.position.x(Target.position.x + offset.x);
        if (y) transform.position = transform.position.y(Target.position.y + offset.y);
        if (z) transform.position = transform.position.z(Target.position.z + offset.z);
    }
}
