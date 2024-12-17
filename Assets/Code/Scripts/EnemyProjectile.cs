using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            Health playerHealth = other.GetComponentInParent<Health>();
            playerHealth.ChangeHealth(-damage);
        }
    }
}
