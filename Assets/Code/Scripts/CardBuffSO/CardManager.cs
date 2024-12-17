using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardSelectionUI;
    [SerializeField] private Card firstBuffCard;
    [SerializeField] private Card secondBuffCard;
    [SerializeField] private Card thirdBuffCard;
    [SerializeField] private List<CardBuffSO> allAvailableBuff;

    [Header("Player Attribute")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private GameObject WeaponsParent;
    [SerializeField] private PlayerDamageAttributes playerDamageAttributesScript;

    public static CardManager Instance;
    [NonSerialized] public Weapon currentActiveWeapon;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    public void RandomizeCards(){
        CardBuffSO cardOne = allAvailableBuff[Random.Range(0, allAvailableBuff.Count)];
        CardBuffSO cardTwo = allAvailableBuff[Random.Range(0, allAvailableBuff.Count)];
        CardBuffSO cardThree = allAvailableBuff[Random.Range(0, allAvailableBuff.Count)];

        firstBuffCard.Setup(cardOne);
        secondBuffCard.Setup(cardTwo);
        thirdBuffCard.Setup(cardThree);

        SetCardSelectionUI(true);
    }

    public void SetCardSelectionUI(bool condition){
        cardSelectionUI.SetActive(condition);

        if(condition){
            GetCurrentWeapon();
            currentActiveWeapon.showCrosshair = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        } else{
            currentActiveWeapon.showCrosshair = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void AddBuff(CardEffect cardEffect, float buffValue){
        PlayerDamageAttributes.CharacterMaxStats roundMaxStats = playerDamageAttributesScript.GetMaxStatsForRound(EnemySpawningSystem.Instance.roundNumber);

        switch(cardEffect){
            case CardEffect.HealthIncrease:
                playerHealth.maxHealth += buffValue;
                playerHealth.maxHealth = Mathf.Clamp(playerHealth.maxHealth, 0f, roundMaxStats.maxHealth);
                playerHealth.ChangeHealth(buffValue);
                break;
            case CardEffect.DamageIncrease:
                PlayerDamageAttributes.characterDamage += buffValue * PlayerDamageAttributes.BASE_CHARACTER_DAMAGE;
                PlayerDamageAttributes.characterDamage = Mathf.Clamp(PlayerDamageAttributes.characterDamage, 0f, roundMaxStats.maxCharacterDamage);
                break;
            case CardEffect.MovementSpeedIncrease:
                playerMovementScript.ChangeMovementSpeed(buffValue);
                break;
            case CardEffect.CritRateIncrease:
                PlayerDamageAttributes.critRate += buffValue;
                PlayerDamageAttributes.critRate = Mathf.Clamp(PlayerDamageAttributes.critRate, 0f, roundMaxStats.maxCritRate);
                break;
            case CardEffect.CritDamageIncrease:
                PlayerDamageAttributes.critDamage += buffValue;
                PlayerDamageAttributes.critDamage = Mathf.Clamp(PlayerDamageAttributes.critDamage, 0f, roundMaxStats.maxCritDamage);
                break;
            case CardEffect.HealthRegeneration:
                playerHealth.healthRegenValue += buffValue;
                playerHealth.healthRegenValue = Mathf.Clamp(playerHealth.healthRegenValue, 0f, roundMaxStats.maxHealthRegen);
                break;
        }

        SetCardSelectionUI(false);
    }

    public void GetCurrentWeapon(){
        foreach(Transform child in WeaponsParent.transform){
            if(child.gameObject.activeSelf == true){
                currentActiveWeapon = child.GetComponent<Weapon>();
            }
        }
    }
}
