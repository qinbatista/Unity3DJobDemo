using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RoleController : MonoBehaviour
{
    // Start is called before the first frame update
    int _roleIndex = 0;
    bool _isIdle = true;
    bool _isWalking = false;
    public int RoleIndex { get => _roleIndex; set => _roleIndex = value; }
    void Start()
    {

    }
    void OnEnable()
    {
        RoleManager.Event_RoleManagerJobDone += DoAction;
    }
    void OnDisable()
    {
        RoleManager.Event_RoleManagerJobDone -= DoAction;
    }
    void DoAction()
    {
        TranslateState();
        SetStateMachine();
        UpdateState();
    }
    void TranslateState()
    {
        _isIdle = (RoleManager.Instance.state[RoleIndex] & (1 << 0)) == (1 << 0) ? true : false;
        _isWalking = (RoleManager.Instance.state[RoleIndex] & (1 << 1)) == (1 << 1) ? true : false;
    }
    void SetStateMachine()
    {
        if (_isIdle)
        {
            RoleManager.Instance.targetPosition[RoleIndex] = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            _isWalking = true;
            _isIdle = false;
        }
    }
    void UpdateState()
    {
        RoleManager.Instance.state[RoleIndex] = 0;
        RoleManager.Instance.state[RoleIndex] |= _isIdle ? 1 << 0 : 0;
        RoleManager.Instance.state[RoleIndex] |= _isWalking ? 1 << 1 : 0;
    }
}
