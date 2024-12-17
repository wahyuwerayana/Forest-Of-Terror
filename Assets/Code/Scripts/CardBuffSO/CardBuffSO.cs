using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Buff", menuName = "Card Buff")]
public class CardBuffSO : ScriptableObject
{
    public Sprite cardImage;
    public CardEffect effectType;
    public bool rangedValue;

    [Header("Value")]
    public float effectValue;
    public float minEffectValue;
    public float maxEffectValue;
}

public enum CardEffect{
    DamageIncrease,
    CritDamageIncrease,
    CritRateIncrease,
    HealthIncrease,
    MovementSpeedIncrease,
    HealthRegeneration
}
