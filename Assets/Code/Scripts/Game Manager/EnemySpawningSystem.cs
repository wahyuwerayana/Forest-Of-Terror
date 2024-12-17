using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawningSystem : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float range = 10f;

    [SerializeField] private List<GameObject> normalZombiesList;
    [SerializeField] private List<GameObject> tankZombiesList;
    [SerializeField] private List<GameObject> crazyZombiesList;

    public const float NORMAL_ZOMBIE_BASE_HEALTH = 100f;
    public const float TANK_ZOMBIE_BASE_HEALTH = 200f;
    public const float CRAZY_ZOMBIE_BASE_HEALTH = 50f;

    public const float NORMAL_ZOMBIE_BASE_ATTACK = 10f;
    public const float TANK_ZOMBIE_BASE_ATTACK = 6F;
    public const float CRAZY_ZOMBIE_BASE_ATTACK = 10f;

    public const int NORMAL_ZOMBIE_BASE_SPAWN_NUMBER = 6;
    public const int TANK_ZOMBIE_BASE_SPAWN_NUMBER = 4;
    public const int CRAZY_ZOMBIE_BASE_SPAWN_NUMBER = 2;

    private const int TANK_ZOMBIE_APPEARS_AT_ROUND = 5;
    private const int CRAZY_ZOMBIE_APPEARS_AT_ROUND = 10;

    [HideInInspector] public int roundNumber = 1;
    [HideInInspector] public int totalEnemies = 0;
    [HideInInspector] public int enemyKillsCount = 0;

    public TMP_Text roundText;
    public TMP_Text totalEnemiesText;

    public static EnemySpawningSystem Instance;

    private Vector3 resultEnemyPosition;

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
    }

    private void Start() {
        Timer.Instance.StartNextWave();
        roundText.text = roundNumber.ToString();
    }

    private IEnumerator SpawnEnemy(GameObject enemyPrefab, float baseHealth, float baseDamage){
        yield return StartCoroutine(RandomPointInNavmeshFixed(playerTransform.position, range));
        GameObject enemy = Instantiate(enemyPrefab, resultEnemyPosition, Quaternion.identity);
        SetEnemyAttributes(enemy, baseHealth, baseDamage);
    }

    private void SetEnemyAttributes(GameObject enemy, float baseHealth, float baseDamage){
        //Set Enemy Health
        Health enemyHealth = enemy.GetComponent<Health>();
        float enemyHealthNumber = GetEnemyAttributesValue(roundNumber, baseHealth);
        enemyHealth.maxHealth = enemyHealthNumber;
        enemyHealth.startingHealth = enemyHealthNumber;

        //Set Enemy Damage
        EnemyDamageAttributes enemyDamage = enemy.GetComponent<EnemyDamageAttributes>();
        float enemyDamageNumber = GetEnemyAttributesValue(roundNumber, baseDamage);
        enemyDamage.enemyDamage = enemyDamageNumber;
    }

    private IEnumerator RandomPointInNavmeshFixed(Vector3 center, float range){
        Vector3 randomPoint;
        NavMeshHit navMeshHit;

        do
        {
            randomPoint = center + Random.insideUnitSphere * range;
            yield return null;
        } while (!NavMesh.SamplePosition(randomPoint, out navMeshHit, 1.0f, NavMesh.AllAreas));

        resultEnemyPosition = navMeshHit.position;
    }

    public void SpawnAllEnemies(){
        int normalZombiesCount = GetEnemyNumber(roundNumber, NORMAL_ZOMBIE_BASE_SPAWN_NUMBER);
        int tankZombiesCount = (roundNumber >= TANK_ZOMBIE_APPEARS_AT_ROUND) ? GetEnemyNumber(roundNumber, TANK_ZOMBIE_BASE_SPAWN_NUMBER) : 0;
        int crazyZombiesCount = (roundNumber >= CRAZY_ZOMBIE_APPEARS_AT_ROUND) ? GetEnemyNumber(roundNumber, CRAZY_ZOMBIE_BASE_SPAWN_NUMBER) : 0;
        enemyKillsCount = 0;
        totalEnemies = normalZombiesCount + tankZombiesCount + crazyZombiesCount;
        totalEnemiesText.text = "Enemies Killed: 0 / " + totalEnemies.ToString();

        for(int i = 0; i < normalZombiesCount; i++){
            StartCoroutine(SpawnEnemy(normalZombiesList[Random.Range(0, normalZombiesList.Count)],
                NORMAL_ZOMBIE_BASE_HEALTH,
                NORMAL_ZOMBIE_BASE_ATTACK));
        }

        for(int i = 0; i < tankZombiesCount; i++){
            StartCoroutine(SpawnEnemy(tankZombiesList[Random.Range(0, tankZombiesList.Count)], 
                TANK_ZOMBIE_BASE_HEALTH,
                TANK_ZOMBIE_BASE_ATTACK));
        }

        for(int i = 0; i < crazyZombiesCount; i++){
            StartCoroutine(SpawnEnemy(crazyZombiesList[Random.Range(0, crazyZombiesList.Count)], 
                CRAZY_ZOMBIE_BASE_HEALTH,
                CRAZY_ZOMBIE_BASE_ATTACK));
        }
    }

    private int GetEnemyNumber(int roundNumber, int baseSpawnNumber){
        return (int)(baseSpawnNumber * Mathf.Pow(1 + 0.05f, roundNumber - 1));
    }

    private float GetEnemyAttributesValue(int roundNumber, float baseValue){
        return (float)(baseValue * Mathf.Pow(1 + 0.05f, roundNumber));
    }
}
