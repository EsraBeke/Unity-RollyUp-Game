using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSoftness = 0.125f;
    [SerializeField] float offSettZ;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, target.position.z + offSettZ), followSoftness);
    }
}
