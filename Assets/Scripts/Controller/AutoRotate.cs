using System.Collections;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private bool randomStartAngle = true;
    [SerializeField] [Range(0, 10)] private float rotateSpeed = 1f;
    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;

    private void OnEnable()
    {
        StartCoroutine(Procedure());
    }

    private IEnumerator Procedure()
    {
        if (randomStartAngle)
        {
            if (x) transform.Rotate(transform.right, Random.value * 360);
            if (y) transform.Rotate(transform.up, Random.value * 360);
            if (z) transform.Rotate(transform.forward, Random.value * 360);
        }

        while (enabled)
        {
            if (x) transform.Rotate(transform.right, rotateSpeed * rotateSpeed);
            if (y) transform.Rotate(transform.up, rotateSpeed * rotateSpeed);
            if (z) transform.Rotate(transform.forward, rotateSpeed * rotateSpeed);
            yield return null;
        }
    }
}
