using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;

public class CardManager : MonoBehaviour
{
    public CardInfo currentCard; 
    public TMP_Text title;
    public TMP_Text description;

    public EventManager choice1;
    public EventManager choice2;
    public EventManager choice3;

    public GameEngine currentGe;

    public Animator cardAnimator;



    void OnMouseDown()
    {
        if (currentGe.drawCard())
        {
            currentCard = currentGe.GetNewCard();
            StartCoroutine(CoRoutineCreateNewCardDisplay());
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        currentGe  = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator CoRoutineCreateNewCardDisplay()
    {
        cardAnimator.Play("EventSelection");
        title.text = currentCard.title;
        description.text = currentCard.description;
        choice1.CreateInfo(currentCard.allEvent[0]);
        choice2.CreateInfo(currentCard.allEvent[1]);
        choice3.CreateInfo(currentCard.allEvent[2]);
        yield return new WaitForSeconds(1f);
    }
}
