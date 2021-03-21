using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] Vector3 lookOffset = Vector3.zero;
    [SerializeField] Vector3 positionOffset = Vector3.zero;
    
    [Range(0f, 1f)]
    [SerializeField] 
    float smoothPosition = 1f;
    [Range(0f, 1f)]
    [SerializeField]
    float smoothLook = 1f;

    Camera cam;

    private void OnValidate()
    {
        cam = GetComponentInChildren<Camera>();

        cam.transform.localPosition = positionOffset;
        cam.transform.LookAt(target.position + lookOffset);
    }

    void Start()
    {
        
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        if (target != null)
        {
            // Position
            transform.position = Vector3.Lerp(transform.position, target.position, smoothPosition);

            // Look
            var targetLook = Quaternion.LookRotation(target.position + lookOffset - cam.transform.position);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetLook, smoothLook);
        }
    }

}
