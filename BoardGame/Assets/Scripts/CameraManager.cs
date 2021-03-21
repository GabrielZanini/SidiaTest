using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform holder;

    [SerializeField] Vector3 lookOffset = Vector3.zero;
    [SerializeField] Vector3 positionOffset = Vector3.zero;
    
    [Range(0f, 1f)]
    [SerializeField] 
    float smoothPosition = 1f;

    Camera cam;


    private void OnValidate()
    {
        cam = holder.GetComponentInChildren<Camera>();
        
        cam.transform.localPosition = positionOffset;
        if (!Application.isPlaying)
        {
            cam.transform.LookAt(target.position + positionOffset);
        }
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        if (target != null)
        {
            holder.transform.position = Vector3.Lerp(holder.transform.position, target.position, smoothPosition);
        }
    }

}
