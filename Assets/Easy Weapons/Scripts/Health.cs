/// <summary>
/// Health.cs
/// Author: MutantGopher
/// This is a sample health script.  If you use a different script for health,
/// make sure that it is called "Health".  If it is not, you may need to edit code
/// referencing the Health component from other scripts
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public enum EnemyType{
	NormalZombie,
	TankZombie,
	CrazyZombie
}

public class Health : MonoBehaviour
{
	public bool canDie = true;					// Whether or not this health can die
	
	public float startingHealth = 100.0f;		// The amount of health to start with
	public float maxHealth = 100.0f;			// The maximum amount of health
	private float currentHealth;				// The current ammount of health
	[NonSerialized] public float healthRegenValue = 2f;
	public float destroyDelay = 5f;
	
	public bool hasAnimator = false;
	private Animator _animator;

	public bool replaceWhenDead = false;		// Whether or not a dead replacement should be instantiated.  (Useful for breaking/shattering/exploding effects)
	public GameObject deadReplacement;			// The prefab to instantiate when this GameObject dies
	public bool makeExplosion = false;			// Whether or not an explosion prefab should be instantiated
	public GameObject explosion;				// The explosion prefab to be instantiated

	public bool isPlayer = false;				// Whether or not this health is the player
	public GameObject deathCam;					// The camera to activate when the player dies
	[SerializeField] private GameObject deathPanel;

	private NavMeshAgent _agent;                    // Reference to the NavMeshAgent component
	public bool needRigidBody = false;
	private Rigidbody _rb;                            // Reference to the Rigidbody component
	private CapsuleCollider _collider;                          // Reference to the Collider component

	private bool dead = false;					// Used to make sure the Die() function isn't called twice
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Image healthSliderFill;
	[SerializeField] private TMP_Text healthText;

    [SerializeField] private EnemyType enemyType;

	// Use this for initialization
	void Start()
	{
		// Initialize the currentHealth variable to the value specified by the user in startingHealth
		currentHealth = startingHealth;
		if(hasAnimator) 
			_animator = GetComponent<Animator>();
		
		if(!isPlayer){
			_agent = GetComponent<NavMeshAgent>();
			if(needRigidBody) _rb = GetComponent<Rigidbody>();
			_collider = GetComponent<CapsuleCollider>();
		}

		if(isPlayer){
			ChangeHealthSlider(healthSlider, healthText, maxHealth, currentHealth);
		}
	}

	public void ChangeHealth(float amount)
	{
		float tempHealth = currentHealth;

		// Change the health by the amount specified in the amount variable
		currentHealth += amount;

		// If the health runs out, then Die.
		if (currentHealth <= 0 && !dead && canDie){
			Die();
			if(!isPlayer) EnemyDie();
		}

		// Make sure that the health never exceeds the maximum health
		else if (currentHealth > maxHealth)
			currentHealth = maxHealth;

		if(isPlayer){
			ChangePlayerHealthAttribute(tempHealth);
		}
	}

	public void Die()
	{
		// This GameObject is officially dead.  This is used to make sure the Die() function isn't called again
		dead = true;

		// Make death effects
		if (replaceWhenDead)
			Instantiate(deadReplacement, transform.position, transform.rotation);
		if (makeExplosion)
			Instantiate(explosion, transform.position, transform.rotation);
		if (hasAnimator) 
            _animator.SetBool("die", true);

		if (isPlayer){
			StopAllCoroutines();
			Timer.Instance.StopAllCoroutines();
			if(deathCam != null) deathCam.SetActive(true);
			deathPanel.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
		}
			

		// Remove this GameObject from the scene
		Destroy(gameObject, destroyDelay);
	}

	public void MakeEnemyFall(){
		_agent.enabled = false;
		if(needRigidBody){
			_rb.isKinematic = false;
			_rb.mass = 999; // Make the enemy hard to push from bullets (when the enemy died)
		} 
		_collider.direction = 2; // change into z-direction
	}

	public void TurnOffCollider(){
		if(needRigidBody) Destroy(_rb);
		_collider.enabled = false;
	}

	private void ChangeHealthSlider(Slider healthSlider, TMP_Text healthText, float maxHealth, float currentHealth){
		float healthValue = currentHealth / maxHealth;
		healthSlider.DOValue(healthValue, 0.5f).Play();
		healthText.text = ((int)currentHealth).ToString();
	}

	private void ChangePlayerHealthAttribute(float tempHealth){
		ChangeHealthSlider(healthSlider, healthText, maxHealth, currentHealth);

		// Regenerate Health
		if(tempHealth > currentHealth && healthRegenValue > 0f){
			//Debug.Log("Kena Damage Sebesar: " + (tempHealth - currentHealth).ToString());
			StopAllCoroutines();
			StartCoroutine(StartRegenerateHealth());
		}
	}

	private void EnemyDie(){
		EnemySpawningSystem.Instance.enemyKillsCount += 1;
		EnemySpawningSystem.Instance.totalEnemiesText.text = "Enemies Killed: " + EnemySpawningSystem.Instance.enemyKillsCount.ToString() + " / " + EnemySpawningSystem.Instance.totalEnemies.ToString();
		CoinsManager.Instance.ChangeCoinsValue(CoinsManager.Instance.GetCoinsValue(enemyType));

		if(EnemySpawningSystem.Instance.enemyKillsCount >= EnemySpawningSystem.Instance.totalEnemies){
			EnemySpawningSystem.Instance.roundNumber += 1;
			EnemySpawningSystem.Instance.roundText.text = EnemySpawningSystem.Instance.roundNumber.ToString();
			StartCoroutine(StartNextWave());
		}
	}

	private IEnumerator StartNextWave(){
		yield return new WaitForSeconds(2f);
		CardManager.Instance.RandomizeCards();
		Timer.Instance.StartNextWave();
	}

	private IEnumerator RegenerateHealth(){
		yield return new WaitForSeconds(1f);
		ChangeHealth(healthRegenValue);
		if(currentHealth < maxHealth) StartCoroutine(RegenerateHealth());
	}

	private IEnumerator StartRegenerateHealth(){
		// Wait for 3 seconds, then start the health regeneration
		yield return new WaitForSeconds(3f);
		StartCoroutine(RegenerateHealth());
	}

	public void ActivateRegeneration(){
		StartCoroutine(StartRegenerateHealth());
	}
}
