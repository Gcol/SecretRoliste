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

    public GameObject diceEtrange;
    public GameObject diceDemon;
    public GameObject diceRational;

    public bool changeAction;

    // Start is called before the first frame update
    void Start()
    {
        nbEvent = 3;
        changeAction = false;
        currentGe  = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
    }

    
    public void FillNewCard(CardInfo newValue, GameObject newDiceEtrange, GameObject newDiceDemon, GameObject newDiceRational)
    {
        currentCard = newValue;
        diceEtrange = newDiceEtrange;
        diceDemon = newDiceDemon;
        diceRational = newDiceRational;
    }

    public void Reset()
    {
        nbEvent = 3;
        changeAction = false;
        choice1.Reset();
        choice2.Reset();
        choice3.Reset();

    }

    public void DisplayCard()
    {
        StartCoroutine(CoRoutineCreateNewCardDisplay());
    }

    public void finishCard(bool currentWin)
    {
        nbEvent -= 1;
        if (changeAction && nbEvent == 2 && !currentWin)
        {
            choice1.changeAction = false;
            choice2.changeAction = false;
            choice3.changeAction = false;
            // Debug.Log("Test");
        }
        else if (!isBoss || (isBoss && nbEvent == 0))
        {
            choice1.eventOn = false;
            choice2.eventOn = false;
            choice3.eventOn = false;
            choice3.ExitEvent();
            cardAnimator.SetInteger("State", 3);
            currentGe.DrawCard();
            //StartCoroutine(CoRoutineEndCard());
            //Désactivier la carte ?
        }
    }

    public void updateChangeAction()
    {
        changeAction = !changeAction;
        choice1.changeAction = changeAction;
        choice2.changeAction = changeAction;
        choice3.changeAction = changeAction;
    }

    IEnumerator CoRoutineCreateNewCardDisplay()
    {
        cardAnimator.SetInteger("State", 2);
        title.text = currentCard.title;
        isBoss = currentCard.isBoss;
        description.text = currentCard.description;
        choice1.CreateInfo(currentCard.allEvent[0], diceEtrange);
        choice2.CreateInfo(currentCard.allEvent[1], diceDemon);
        choice3.CreateInfo(currentCard.allEvent[2], diceRational);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator CoRoutineEndCard()
    {
        cardAnimator.SetInteger("State", 3);
        yield return new WaitForSeconds(1f);
        currentGe.DrawCard();
        Destroy(gameObject);
    }
}
