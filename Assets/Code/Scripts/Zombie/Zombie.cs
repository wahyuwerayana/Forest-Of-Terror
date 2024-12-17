using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] private Animator animator;
    public LayerMask playerMask;
    public Transform rightHand;
    public Transform leftHand;
    public float attackRadius = 1f;
    [SerializeField] private EnemyDamageAttributes damageAttributes;

    public void Attack(){
        bool hitPlayerRightHand = Physics.CheckSphere(rightHand.position, attackRadius, playerMask);
        bool hitPlayerLeftHand = Physics.CheckSphere(leftHand.position, attackRadius, playerMask);

        if(hitPlayerLeftHand || hitPlayerRightHand){
            Collider hitPlayers;
            if(hitPlayerLeftHand){
                hitPlayers = Physics.OverlapSphere(leftHand.position, attackRadius, playerMask)[0];
                if(hitPlayers != null) hitPlayers.GetComponentInParent<Health>().ChangeHealth(-damageAttributes.enemyDamage);
            } else if(hitPlayerRightHand){
                hitPlayers = Physics.OverlapSphere(rightHand.position, attackRadius, playerMask)[0];
                if(hitPlayers != null) hitPlayers.GetComponentInParent<Health>().ChangeHealth(-damageAttributes.enemyDamage);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftHand.position, attackRadius);
        Gizmos.DrawWireSphere(rightHand.position, attackRadius);
    }
}
