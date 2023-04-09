using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class CardObject
{
    public string name;
    public string description;
    public string effect; // A détailler;
}

public class EventEffect
{
    public string textEvenEffect;
    public string typeEffect;
    public int effectGain;
    public CardObject objectReward;

    public EventEffect(string newTextEvenEffect)
    {
        textEvenEffect = newTextEvenEffect;
    }

    public EventEffect(string newTextEvenEffect, string newTypeEffect, int newEffectGain)
    {
        textEvenEffect = newTextEvenEffect;
        typeEffect = newTypeEffect;
        effectGain = newEffectGain;
    }

    public EventEffect(string newTextEvenEffect, CardObject newObjectReward)
    {
        textEvenEffect = newTextEvenEffect;
        typeEffect = "object";
        objectReward = newObjectReward;
    }
}

public class CardInfo
{

    public string title;
    public string description;
    public CardEvent[] allEvent = new CardEvent[3];
    
    public CardInfo(string newTitle, string newDescription)
    {
        title = newTitle;
        description = newDescription;
    }

}

public class CardEvent
{
    public string type;
    public int difficulty;
    public string description;
    public EventEffect winEffect;
    public EventEffect loseEffect;


    public CardEvent(string newType, int newDifficulty, string new_description, EventEffect newWinEffect, EventEffect newLoseEffect)
    {
        type = newType;
        difficulty = newDifficulty;
        description = new_description;

        winEffect = newWinEffect;
        loseEffect = newLoseEffect;
    }

}

public class GameEngine : MonoBehaviour
{
    public List<CardInfo> allCardForThisGame = new List<CardInfo>();
    public List<CardObject> allObject = new List<CardObject>();

    public bool CardActive;
    public PlayerManager currentPM;

    public string loseState;
    public GameObject losePannel;
    public TMP_Text textLose;

    // Start is called before the first frame update
    void Start()
    {
        Restart();
        CardInfo firstCard = new CardInfo("Vendeur d'éponge lunaire", " \"En provenance directe de la troisième lune terrestre. \r\nUn vendeur s’annonce à vous, il promet de détenir l’éponge qui saura régler tous vos problèmes.\"\r\n");

        firstCard.allEvent[0] = new CardEvent("étrange", 8, "Déclamer de la poèsie en braille.", new EventEffect("Impressionant"), new EventEffect("Vos rimes croisées ne fonctionnent pas.", "santé", -3));
        firstCard.allEvent[1] = new CardEvent("Démon", 8, "Casser une noix avec les dents.", new EventEffect("En plus la noix n'est pas pourrie !"), new EventEffect("Noix 1 - Machoire 0", "santé & raison", -2));
        firstCard.allEvent[2] = new CardEvent("Rationnel", 8, "Faire une présentation projetée de vos réussites", new EventEffect("Vos animations et bruitage de canard éblouissent Shoggoth"), new EventEffect("Faire pipirdans la cuvette ne compte pas comme une réussite", "raison", -5));

        allCardForThisGame.Add(firstCard);
    }

    public void Restart()
    {
        loseState = null;
        CardActive = false;
        losePannel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (loseState != null)
        {
            losePannel.SetActive(true);
            textLose.text = "Tu as perdu à cause de " + loseState + "\n Bonne chance pour ta prochaine aventure !";
            loseState = null;
        }
    }

    public bool drawCard()
    {
        if (!CardActive)
        {
            CardActive = true;
            return true;
        }
        return false;
    }


    public CardInfo GetNewCard()
    {
        // Ajouter une condition de fin de carte
        CardInfo newCard = allCardForThisGame[0];

        //allCardForThisGame.Remove(allCardForThisGame[0]);
        return newCard;

    }

    public bool RollDice(string diceType, int difficulty)
    {
        int currentRoll = Random.Range(1, currentPM.returnStatDice(diceType.ToLower()));
        Debug.LogFormat("current dice {0} for max {1} I roll {2} with diff of {3} my win is {4}", diceType, currentPM.returnStatDice(diceType.ToLower()), currentRoll, difficulty, difficulty >= currentRoll);
        return currentRoll >= difficulty;
    }

    public void ApplyEventEffect(EventEffect newEventEffect)
    {
        if (newEventEffect.typeEffect != null)
        {
            if (newEventEffect.typeEffect == "object")
            {
                Debug.Log("Tu as gagné un object");
            }
            else
            {
                currentPM.ChangeStat(newEventEffect.typeEffect, newEventEffect.effectGain);
            }
        }
    }


}
