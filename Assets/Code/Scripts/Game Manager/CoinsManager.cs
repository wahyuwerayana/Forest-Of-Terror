using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    [NonSerialized] public int coinsValue = 0;
    [SerializeField] private TMP_Text coinsText;

    public static CoinsManager Instance;

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
    }

    private void Start() {
        ChangeCoinsValue(200);
    }

    public void ChangeCoinsValue(int value){
        coinsValue += value;
        UpdateCoinsValue();
    }

    public void UpdateCoinsValue(){
        coinsText.text = coinsValue.ToString();
    }

    public int GetCoinsValue(EnemyType enemyType){
        int coinValue = 0;

        switch(enemyType){
            case EnemyType.NormalZombie:
                coinValue = 2;
                break;
            case EnemyType.TankZombie:
                coinValue = 3;
                break;
            case EnemyType.CrazyZombie:
                coinValue = 4;
                break;
        }

        return coinValue;
    }
}
