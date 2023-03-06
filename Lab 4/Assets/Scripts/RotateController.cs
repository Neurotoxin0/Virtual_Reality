using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    public bool isActivated { get; set; }
    public float rotateRatio { get; set; }

    private Vector3 vec;

    void Start()
    {
        isActivated = false;
        rotateRatio = 1;
        vec = new Vector3 (15, 30, 45);
    }

    void Update()
    {
		  if (isActivated) transform.Rotate(vec * rotateRatio * Time.deltaTime);
    }
}
