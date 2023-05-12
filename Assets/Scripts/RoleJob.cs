using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct RoleJob : IJobParallelForTransform
{
    [ReadOnly] public Vector3 _targetPosition;
    public NativeArray<int> _state;
    public void Execute(int index, TransformAccess transform)
    {
        if (Mathf.Abs(_targetPosition.x - transform.position.x) > 10f) _state[index] = 1;
        else _state[index] = 1;
        if (Mathf.Abs(_targetPosition.x - transform.position.x) >= 30 && Mathf.Abs(_targetPosition.x - transform.position.x) < 35)
        {
            if (_targetPosition.x > transform.position.x)
            {
                transform.position = new Vector3(transform.position.x + 55, 0, 0);
                Debug.Log($"left move to right = {transform.position}");
            }
            else
            {
                transform.position = new Vector3(transform.position.x - 55, 0, 0);
                Debug.Log($"right move to left = {transform.position}");
            }
        }
    }
}