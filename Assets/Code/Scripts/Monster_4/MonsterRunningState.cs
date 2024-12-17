using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRunningState : StateMachineBehaviour
{
    [Range(1f, 5f)] public float attackCloseRange; // jarak musuh melakukan attack
    [Range(5f, 15f)] public float attackFarRange; // jarak musuh melakukan attack
    public List<int> closeAttackChance;
    public List<string> closeAttackTriggerName;
    public List<int> farAttackChance;
    public List<string> farAttackTriggerName;

    private Transform _player;
    private int _randNum;
    private string _triggerName;
    private NavMeshAgent _agent;
    private bool _isAttacking = false;
    public bool closeRangeOnly = true;
    public bool boolBasedTrigger = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
       if(!_agent) _agent = animator.GetComponent<NavMeshAgent>();
       _isAttacking = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_player == null)
            return;

        if(closeRangeOnly){
            if(((_player.transform.position - animator.transform.position).sqrMagnitude <= attackCloseRange * attackCloseRange) && !_isAttacking){
                GetCloseAttackTrigger(animator);
            }
        } else{
            if(((_player.transform.position - animator.transform.position).sqrMagnitude <= attackCloseRange * attackCloseRange) && !_isAttacking){
                GetCloseAttackTrigger(animator);
            } else if(((_player.transform.position - animator.transform.position).sqrMagnitude > attackCloseRange * attackCloseRange) && 
                ((_player.transform.position - animator.transform.position).sqrMagnitude <= attackFarRange * attackFarRange) &&
                !_isAttacking){
                GetFarAttackTrigger(animator);
            }
        }

       _agent.SetDestination(_player.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(_triggerName != "" && _triggerName != null && !boolBasedTrigger) animator.ResetTrigger(_triggerName);
       _agent.ResetPath();
    }

    private void GetCloseAttackTrigger(Animator animator){
        _isAttacking = true;

        _randNum = Random.Range(1, 101);
        int counter = 0;

        for(int i = 0; i < closeAttackChance.Count; i++){
            if(i == 0){
                if(_randNum <= closeAttackChance[i]){
                    _triggerName = closeAttackTriggerName[i];
                    break;
                }
            } else if(i == closeAttackChance.Count - 1){
                _triggerName = closeAttackTriggerName[i];
                break;
            } else{
                if(_randNum > counter && _randNum <= (closeAttackChance[i+1] + counter)){
                    _triggerName = closeAttackTriggerName[i];
                    break;
                }
            }
            counter += closeAttackChance[i];
        }

        if(!boolBasedTrigger)
            animator.SetTrigger(_triggerName);
        else
            animator.SetBool(_triggerName, true);
    }

    private void GetFarAttackTrigger(Animator animator){
        _isAttacking = true;

        _randNum = Random.Range(1, 101);
        int counter = 0;

        for(int i = 0; i < farAttackChance.Count; i++){
            if(i == 0){
                if(_randNum <= farAttackChance[i]){
                    _triggerName = farAttackTriggerName[i];
                    break;
                }
            } else if(i == farAttackChance.Count - 1){
                _triggerName = farAttackTriggerName[i];
                break;
            } else{
                if(_randNum > counter && _randNum <= (farAttackChance[i+1] + counter)){
                    _triggerName = farAttackTriggerName[i];
                    break;
                }
            }
            counter += farAttackChance[i];
        }

        if(!boolBasedTrigger)
            animator.SetTrigger(_triggerName);
        else
            animator.SetBool(_triggerName, true);
    }
}
