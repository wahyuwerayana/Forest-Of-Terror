using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    NavMeshAgent _agent;
    Transform _player;
    [SerializeField] [Range(1f, 50f)] float _chaseSpeed;
    [SerializeField] [Range(1f, 50f)] float _enemyChaseRange;
    [SerializeField] [Range(0f, 50f)] float _enemyAttackRange;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(_agent == null) _agent = animator.GetComponent<NavMeshAgent>();
       if(_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
       _agent.speed = _chaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_player == null) return;

        _agent.SetDestination(_player.position);
       
        float _distance = Vector3.Distance(_player.position, animator.transform.position);
        //if(_distance > _enemyChaseRange) animator.SetBool("isChasing", false);
        if(_distance < _enemyAttackRange) animator.SetBool("isAttacking", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _agent.ResetPath();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
