using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;

public class CardManager : MonoBehaviour
{
    public CardInfo currentCard; 
    public TMP_Text title;
    public TMP_Text description;
    public bool isBoss;

    public EventManager choice1;
    public EventManager choice2;
    public EventManager choice3;

    public GameEngine currentGe;

    public Animator cardAnimator;

    public int nbEvent;


    // Start is called before the first frame update
    void Start()
    {
        nbEvent = 3;
        currentGe  = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
    }

    
    public void FillNewCard(CardInfo newValue)
    {
        currentCard = newValue;
    }

    public void DisplayCard()
    {
        StartCoroutine(CoRoutineCreateNewCardDisplay());
    }

    public void finishCard()
    {
        nbEvent -= 1;
        if (!isBoss || (isBoss && nbEvent == 0))
        {
            StartCoroutine(CoRoutineEndCard());
            //Désactivier la carte ?
        }
    }


    IEnumerator CoRoutineCreateNewCardDisplay()
    {
        cardAnimator.SetInteger("State", 1);
        title.text = currentCard.title;
        isBoss = currentCard.isBoss;
        description.text = currentCard.description;
        choice1.CreateInfo(currentCard.allEvent[0]);
        choice2.CreateInfo(currentCard.allEvent[1]);
        choice3.CreateInfo(currentCard.allEvent[2]);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator CoRoutineEndCard()
    {
        cardAnimator.SetInteger("State", 2);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        currentGe.DrawCard();
    }
}
