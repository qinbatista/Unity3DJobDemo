using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;
public class RoleManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject rolePrefab;

    [Header("Job System")]
    List<GameObject> roles = new List<GameObject>();
    NativeArray<int> _state;
    TransformAccessArray _roleTransformAccessArray;
    RoleJob _roleJob;
    JobHandle _jobHandle;
    public static Action Event_RoleManagerJobDone;
    int _maxSizeOfRoles = 20;
    void Start()
    {
        _roleTransformAccessArray = new TransformAccessArray(_maxSizeOfRoles);
        for (int i = 0; i < 10; i++)
        {
            GameObject role = Instantiate(rolePrefab);
            role.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            _roleTransformAccessArray.Add(role.transform);
            roles.Add(role);
        }
    }

    void Update()
    {
        _roleJob = new RoleJob
        {
            _targetPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
            _state = _state,
        };
        _jobHandle = _roleJob.Schedule(_roleTransformAccessArray);
        _jobHandle.Complete();
        Event_RoleManagerJobDone?.Invoke();
    }
}
