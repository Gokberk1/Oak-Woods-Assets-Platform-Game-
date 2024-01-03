using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OakWoods2DPlatformGame.Controller;

public class FS_Walk : StateMachineBehaviour
{
    [SerializeField] float _speed = 3f;
    float _attackRange;
    float _detectRange;
    Transform _playerTransform;
    Rigidbody2D _rb;
    SkeletonController _skeleton;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //_rb = animator.GetComponentInParent<Rigidbody2D>();

        //_skeleton = animator.GetComponentInParent<SkeletonController>();
        //_attackRange = animator.GetComponentInParent<SkeletonController>()._attackRange;
        //_detectRange = animator.GetComponentInParent<SkeletonController>()._detectRange;

        _rb = animator.GetComponent<Rigidbody2D>();

        _skeleton = animator.GetComponent<SkeletonController>();
        _attackRange = animator.GetComponent<SkeletonController>()._attackRange;
        _detectRange = animator.GetComponent<SkeletonController>()._detectRange;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

        if (Vector2.Distance(_playerTransform.position, _rb.position) >= _detectRange)
        {
            animator.SetTrigger("Idle");
        }

        if (Vector2.Distance(_playerTransform.position, _rb.position) <= _attackRange)
        {
            animator.SetTrigger("Attack");
            _skeleton.LookAtPlayer();
        }
        else
        {
            _skeleton.LookAtPlayer();

            Vector2 target = new Vector2(_playerTransform.position.x, _rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(_rb.position, target, _speed * Time.fixedDeltaTime);
            _rb.MovePosition(newPos);
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Idle");
    }

}
