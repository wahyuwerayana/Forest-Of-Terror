using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageAttributes : MonoBehaviour
{
    public const float BASE_CHARACTER_DAMAGE = 20f;
    public static float characterDamage;
    public static float critRate = 0.15f;
    public static float critDamage = 1.5f;

    private void Start() {
        characterDamage = BASE_CHARACTER_DAMAGE;
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
