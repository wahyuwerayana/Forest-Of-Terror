using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageAttributes : MonoBehaviour
{
    public const float BASE_CHARACTER_DAMAGE = 20f;
    public static float characterDamage;
    public static float critRate = 0.05f;
    public static float critDamage = 1.0f;

    public class CharacterMaxStats{
        public float maxHealth;
        public float maxHealthRegen;
        public float maxCharacterDamage;
        public float maxCritRate;
        public float maxCritDamage;
        public float maxSpeed;

        public CharacterMaxStats(float health, float healthRegen, float characterDamage, float critRate, float critDamage, float speed){
            maxHealth = health;
            maxHealthRegen = healthRegen;
            maxCharacterDamage = characterDamage;
            maxCritRate = critRate;
            maxCritDamage = critDamage;
            maxSpeed = speed;
        }
    }

    public CharacterMaxStats[] RoundLimits;

    private void Start() {
        characterDamage = BASE_CHARACTER_DAMAGE;
        InitializeCharacterStatsLimits();
    }

    public void InitializeCharacterStatsLimits(){
        RoundLimits = new CharacterMaxStats[4];

        // Round 10, 20, 25, > 25
        RoundLimits[0] = new CharacterMaxStats(200f, 2f, 40f, 0.25f, 1.5f, 1.3f);
        RoundLimits[1] = new CharacterMaxStats(600f, 4f, 80f, 0.5f, 2.0f, 1.3f);
        RoundLimits[2] = new CharacterMaxStats(1200f, 6f, 120f, 0.7f, 2.5f, 1.3f);
        RoundLimits[3] = new CharacterMaxStats(float.MaxValue, float.MaxValue, float.MaxValue, 1f, float.MaxValue, 1.3f);
    }

    public CharacterMaxStats GetMaxStatsForRound(int round){
        if(round > 25) return RoundLimits[3];
        else if(round >= 20) return RoundLimits[2];
        else if(round >= 10) return RoundLimits[1];

        return RoundLimits[0];
    }

    public static float CalculateDamage(float weaponDamage){
        float damage = (weaponDamage + characterDamage) * (1 + critRate * critDamage);
        //PrintAttributes(weaponDamage);
        return damage;
    }

    public static void PrintAttributes(float weaponDamage){
        Debug.Log("WeaponDamage: " + weaponDamage);
        Debug.Log("CharacterDamage: " + characterDamage);
        Debug.Log("CritRate: " + critRate);
        Debug.Log("CritDamage: " + critDamage);
    }
}
