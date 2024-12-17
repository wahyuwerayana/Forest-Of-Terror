using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    private CardBuffSO cardInfo;

    public void Setup(CardBuffSO card){
        cardInfo = card;
        cardImage.sprite = card.cardImage;
    }

    public void ChooseCard(){
        float finalBuffValue;
        
        if(cardInfo.rangedValue){
            finalBuffValue = Random.Range(cardInfo.minEffectValue, cardInfo.maxEffectValue);
        } else{
            finalBuffValue = cardInfo.effectValue;
        }

        CardManager.Instance.AddBuff(cardInfo.effectType, finalBuffValue);
    }
}
