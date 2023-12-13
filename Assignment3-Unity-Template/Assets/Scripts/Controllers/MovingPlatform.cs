using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // radius from point variable needed
    [SerializeField]
    Transform center;

    public float rotationRadius;
    public float speed;

    private float angle = 0f;

    private Vector3 pos = new Vector3();

    void Update()
    {
        // Rotate around parent's transform
        pos.x = center.position.x + rotationRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.y = center.position.y + rotationRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

        transform.position = new Vector3(pos.x, pos.y);
        angle = angle + (speed * Time.deltaTime);

        if (angle >= 360)
        {
            angle = 0;
        }
    }
}
