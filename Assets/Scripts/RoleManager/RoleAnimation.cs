using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAnimation : MonoBehaviour
{
    Animator _animator;
    //animation hash id
    int _isIdleKey;
    int _isWalkingKey;
    float _walkingSpeed;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _isIdleKey = Animator.StringToHash("isIdle");
        _isWalkingKey = Animator.StringToHash("isWalking");
        _walkingSpeed = Animator.StringToHash("walkingSpeed");
    }
    public void SetWalkAnimation(bool isWalking)=>_animator.SetBool(_isWalkingKey, isWalking);
    public void SetIdleAnimation(bool isIdle)=>_animator.SetBool(_isIdleKey, isIdle);
}
