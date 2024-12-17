using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolState : StateMachineBehaviour
{
    NavMeshAgent _agent;
    Transform _player;
    [SerializeField] [Range(1f, 50f)] float _range;
    [SerializeField] [Range(1f, 50f)] float _enemyChaseRange;
    [SerializeField] [Range(1f, 50f)] float _patrolSpeed;
    public Transform centrePoint;
    bool _hasMoved;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_agent == null) _agent = animator.GetComponent<NavMeshAgent>();
        if(centrePoint == null) centrePoint = animator.GetComponent<Transform>();
        _hasMoved = false;
        if(_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent.speed = _patrolSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_agent.remainingDistance <= _agent.stoppingDistance){
            if(_hasMoved){ // kalo zombienya udah bergerak + dah sampai di tujuan
                animator.SetBool("isPatrolling", false);
                return;
            }

            Vector3 point;
            if(RandomPoint(centrePoint.position, _range, out point)){ 
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                _agent.SetDestination(point);
                _hasMoved = true;
            }
            
        }

        float _distance = Vector3.Distance(_player.position, animator.transform.position);
        if(_distance < _enemyChaseRange) animator.SetBool("isChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
       
    // }

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

    bool RandomPoint(Vector3 center, float range, out Vector3 result){ // nyari posisi random untuk bergerak
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)){
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
