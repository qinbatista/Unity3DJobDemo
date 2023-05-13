using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;
public class RoleManager : Singleton<RoleManager>
{
    // Start is called before the first frame update
    [SerializeField] GameObject rolePrefab;

    [Header("Job System")]
    public static Action Event_RoleManagerJobDone;
    List<RoleController> _roleList = new List<RoleController>();
    public NativeArray<int> state;// 0000001, Idle, 0000010, Move, 0000100, Attack, 0001000, Dead
    public NativeArray<Vector3> targetPosition;//roles' target position, users will keep moving to this position
    TransformAccessArray _roleTransformAccessArray;//roles' transform
    RoleJob _roleJob;
    JobHandle _jobHandle;
    [SerializeField] int _maxSizeOfRoles = 20;
    void Start()
    {
        //initial all Native Arrays for job system
        _roleTransformAccessArray = new TransformAccessArray(_maxSizeOfRoles);
        state = new NativeArray<int>(_maxSizeOfRoles, Allocator.Persistent);
        targetPosition = new NativeArray<Vector3>(_maxSizeOfRoles, Allocator.Persistent);

        //loop to create roles
        for (int i = 0; i < _maxSizeOfRoles; i++)
        {
#if UNITY_EDITOR
            //group them under the RoleManager GameObject for easy management in Unity Editor
            GameObject role = Instantiate(rolePrefab, this.transform);
#else
            //set them in first level of hierarchy for better performance
            GameObject role = Instantiate(rolePrefab);
#endif
            //set random position
            role.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            _roleTransformAccessArray.Add(role.transform);
            //set target position
            targetPosition[i] = role.transform.position;
            //set the index of each role
            RoleController roleController = role.GetComponent<RoleController>();
            roleController.RoleIndex = i;
            _roleList.Add(roleController);
            //initial state
            state[i] = 1;//0000001, Idle
        }
    }

    void Update()
    {
        //start job loop
        _roleJob = new RoleJob
        {
            _targetPosition = targetPosition,
            _state = state,
            _deltaTime = Time.deltaTime
        };
        _jobHandle = _roleJob.Schedule(_roleTransformAccessArray);
        _jobHandle.Complete();
        //job done, send event
        Event_RoleManagerJobDone?.Invoke();
    }
    private void OnDisable()
    {
        state.Dispose();
        _roleTransformAccessArray.Dispose();
        targetPosition.Dispose();
    }
}
