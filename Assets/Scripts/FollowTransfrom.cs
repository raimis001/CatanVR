using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransfrom : MonoBehaviour
{
    public Transform follow;
    public bool allowPosition = true;
    public bool allowRotation = true;
    public bool lateUpdate;

    private void Update()
    {
        if (lateUpdate)
            return;

        UpdateAll();
    }
    private void LateUpdate()
    {
        if (!lateUpdate)
            return;

        UpdateAll();
    }

    void UpdateAll()
    {
        if (!follow)
            return;

        if (allowPosition)
            transform.position = follow.position;
        if (allowRotation)
            transform.rotation = follow.rotation;
    }
}
