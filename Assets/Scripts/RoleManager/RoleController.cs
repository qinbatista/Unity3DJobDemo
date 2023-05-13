using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RoleController : MonoBehaviour
{
    int _roleIndex = 0;
    bool _isIdle = true;
    bool _isWalking = false;
    RoleAnimation _roleAnimation;
    [SerializeField] RoleStateEmu _lastState;
    public int RoleIndex { get => _roleIndex; set => _roleIndex = value; }
    void Awake()
    {
        _roleAnimation = GetComponent<RoleAnimation>();
        _lastState = RoleStateEmu.Idle;
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
        GetStateMachine();
        UpdateState();
        SetStateMachine();
    }
    void GetStateMachine()
    {
        _isIdle = (RoleManager.Instance.state[RoleIndex] & (1 << 0)) == (1 << 0) ? true : false;
        _isWalking = (RoleManager.Instance.state[RoleIndex] & (1 << 1)) == (1 << 1) ? true : false;
        // Debug.Log("[Get]_isIdle: " + _isIdle + " _isWalking: " + _isWalking + " RoleManager.Instance.state[RoleIndex]=" + RoleManager.Instance.state[RoleIndex]);
    }
    void UpdateState()
    {
        switch (_lastState)
        {
            case RoleStateEmu.Idle:
                if (_isWalking)
                {
                    _lastState = RoleStateEmu.Walking;
                    RoleManager.Instance.targetPosition[RoleIndex] = new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
                    _roleAnimation.SetWalkAnimation(true);
                }
                break;
            case RoleStateEmu.Walking:
                if (_isIdle)
                {
                    _lastState = RoleStateEmu.Idle;
                    _roleAnimation.SetWalkAnimation(false);
                }
                break;
        }
    }
    void SetStateMachine()
    {
        RoleManager.Instance.state[RoleIndex] = 0;
        RoleManager.Instance.state[RoleIndex] |= _isIdle ? 1 << 0 : 0;
        RoleManager.Instance.state[RoleIndex] |= _isWalking ? 1 << 1 : 0;
        // Debug.Log("[Set]_isIdle: " + _isIdle + " _isWalking: " + _isWalking + " RoleManager.Instance.state[RoleIndex]=" + RoleManager.Instance.state[RoleIndex]);
    }
}
