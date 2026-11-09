using UnityEngine;

public class PivotRotator : MonoBehaviour
{
    public float rotationSpeed = 90f;

    public Transform[] elements = new Transform[7];

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        foreach (Transform element in elements)
        {
            if (element != null)
            {
                element.RotateAround(
                    transform.position,
                    Vector3.up,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
