using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    void Update()
    {
        Vector3 t1 = transform.position;
        t1.y = Camera.main.transform.position.y;
        Vector3 delta = t1 - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(delta, Vector3.up);
    }
}
