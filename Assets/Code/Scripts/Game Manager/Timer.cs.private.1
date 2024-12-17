using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Health playerHealth;

    public static Timer Instance;

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
    }

    public void StartNextWave(){
        StopAllCoroutines();
        StartCoroutine(StartWaveDelay());
    }

    private IEnumerator StartCountdown(float countdownTime = 90f){
        float currentCountdownValue = countdownTime;

        while(currentCountdownValue > 0f){
            currentCountdownValue -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentCountdownValue / 60);
            int seconds = Mathf.FloorToInt(currentCountdownValue % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }

        if(countdownTime == 90f){
            timerText.text = "00:00";
            StartCoroutine(ReducePlayerHealth());
        }
    }

    private IEnumerator StartWaveDelay(){
        yield return StartCoroutine(StartCountdown(5f));
        StartCoroutine(StartCountdown());
        EnemySpawningSystem.Instance.SpawnAllEnemies();
    }

    private IEnumerator ReducePlayerHealth(){
        playerHealth.ChangeHealth(-1);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ReducePlayerHealth());
    }
}
