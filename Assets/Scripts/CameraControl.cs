using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform lookTarget;
    private float smoothSpeed = 10f;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;
    private Vector3 offset = new Vector3(0f, 3f, 0f);

    private void FixedUpdate()
    {
        if (lookTarget == null) return;

        desiredPosition = lookTarget.transform.TransformPoint(0f, 3f, -20f);
        desiredRotation = Quaternion.LookRotation(lookTarget.position + offset - transform.position);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
    }
    public void SetTarget(Transform argLookTarget)
    {
        lookTarget = argLookTarget;
    }
}
