using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct RoleJob : IJobParallelForTransform
{
    [ReadOnly] public NativeArray<Vector3> _targetPosition;
    public NativeArray<int> _state;
    [ReadOnly] public float _deltaTime;
    bool _isIdle;
    bool _isWalking;
    public void Execute(int index, TransformAccess transform)
    {
        TranslateState(index);
        RoleAI(index, transform);
        UpdateState(index);
    }
    void TranslateState(int index)
    {
        _isIdle = (_state[index] & (1 << 0)) == (1 << 0) ? true : false;
        _isWalking = (_state[index] & (1 << 1)) == (1 << 1) ? true : false;
    }
    void RoleAI(int index, TransformAccess transform)
    {
        if (transform.position == _targetPosition[index])
        {
            _isWalking = false;
            _isIdle = true;
        }
        else
        {
            _isWalking = true;
            _isIdle = false;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition[index], 5.0f * _deltaTime);
        }
    }
    void UpdateState(int index)
    {
        _state[index] = 0;
        _state[index] |= _isIdle ? 1 << 0 : 0;
        _state[index] |= _isWalking ? 1 << 1 : 0;
    }
}