using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PositionLimiter : MonoBehaviour
{
    private Vector3 position;
    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        if (transform.position != position)
        {
            float posY = Mathf.Clamp(transform.position.y, 0, 2);
            transform.position = new Vector3(position.x, posY, position.z);
        }
    }
}
