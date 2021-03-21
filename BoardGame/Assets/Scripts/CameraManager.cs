using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform holder;
    [SerializeField] Vector3 positionOffset = Vector3.zero;
    [Range(0f, 10f)]
    [SerializeField] 
    float smoothPosition = 1f;

    Camera cam;

    [ShowNativeProperty]
    public Transform Target { get; set; }



    private void OnValidate()
    {
        if (Target == null)
        {
            Target = transform;
        }

        cam = holder.GetComponentInChildren<Camera>();
        
        cam.transform.localPosition = positionOffset;
        
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        if (Target != null)
        {
            holder.transform.position = Vector3.Lerp(holder.transform.position, Target.position, smoothPosition * Time.deltaTime);
        }
    }


    

}
