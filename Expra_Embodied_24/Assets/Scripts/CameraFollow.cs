using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public CursorClass cursorClass;

    //private float smoothSpeed = 10f;
    //private float intendedDeltaTime = 1.0f / 60.0f;
    [SerializeField]
    private Vector3 offset;

    private Transform cameraTransform;
    private Transform targetTransform;
    //private Vector3 desiredPosition;
    //private Vector3 smoothedPosition;
    //private float smoothSpeed;
    //private float intendedDeltaTime;

    //private Vector3 desiredPosition;
    //private Vector3 smoothedPosition;

    private void Awake()
    {
        cursorClass = target.GetComponent<CursorClass>();
        cameraTransform = transform;
        targetTransform = target.transform;

        //smoothSpeed = 120.0f;
        //intendedDeltaTime = 1.0f / 60.0f;
    }

    private void LateUpdate()
    {
        //desiredPosition = target.position + offset;
        //desiredPosition = new Vector3(0, targetTransform.position.y, targetTransform.position.z) + offset;
        //smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed * (Time.deltaTime / intendedDeltaTime));
        //cameraTransform.position = smoothedPosition;
        switch (cursorClass.currentLane)
        {
            case "LeftLane":
                cameraTransform.position = new Vector3(-20.0f, targetTransform.position.y + offset.y, targetTransform.position.z + offset.z);
                break;
            case "MiddleLane":
                cameraTransform.position = new Vector3(0.0f, targetTransform.position.y + offset.y, targetTransform.position.z + offset.z);
                break;
            case "RightLane":
                cameraTransform.position = new Vector3(20.0f, targetTransform.position.y + offset.y, targetTransform.position.z + offset.z);
                break;
        }
    }
}
