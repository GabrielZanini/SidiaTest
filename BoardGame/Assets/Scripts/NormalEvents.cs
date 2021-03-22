using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class NormalEvents : MonoBehaviour
{
    [SerializeField] bool _Validate;
    [SerializeField] bool _Awake;
    [SerializeField] bool _Start;
    [SerializeField] bool _Update;
    [SerializeField] bool _FixedUpdate;
    [SerializeField] bool _Enable;
    [SerializeField] bool _Disable;
    [SerializeField] bool _Destroy;

    [ShowIf("_Validate")][SerializeField] UnityEvent On_Validate;
    [ShowIf("_Awake")][SerializeField] UnityEvent On_Awake;
    [ShowIf("_Start")][SerializeField] UnityEvent On_Start;
    [ShowIf("_Update")][SerializeField] UnityEvent On_Update;
    [ShowIf("_FixedUpdate")][SerializeField] UnityEvent On_FixedUpdate;
    [ShowIf("_Enable")][SerializeField] UnityEvent On_Enable;
    [ShowIf("_Disable")][SerializeField] UnityEvent On_Disable;
    [ShowIf("_Destroy")] [SerializeField] UnityEvent On_Destroy;

    private void OnValidate()
    {
        if (On_Validate != null) 
        {
            On_Validate.Invoke();
        }        
    }

    private void Awake()
    {
        On_Awake.Invoke();
    }

    void Start()
    {
        On_Start.Invoke();
    }

    void Update()
    {
        On_Update.Invoke();
    }

    private void FixedUpdate()
    {
        On_FixedUpdate.Invoke();
    }

    private void OnEnable()
    {
        On_Enable.Invoke();
    }

    private void OnDisable()
    {
        On_Disable.Invoke();
    }

    private void OnDestroy()
    {
        On_Destroy.Invoke();
    }
}
