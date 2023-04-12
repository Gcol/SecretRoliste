using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class EventManager : MonoBehaviour
{
    public CardEvent currentCardEvent;

    public TMP_Text entete;
    public TMP_Text description;

    public int difficultyModifier;
    public Color defaultColor;

    public GameEngine currentGE;

    public bool eventOn;
    public CardObject linkObject;
    public GameObject currentPlaceHolder;


    void OnMouseOver()
    {
        if (eventOn)
        {
            GetComponent<Renderer>().material.color = Color.red;
            if (linkObject != null && currentPlaceHolder.GetComponent<ObjectManager>().titre.text != linkObject.name)
            {
                currentPlaceHolder.SetActive(true);
                currentPlaceHolder.transform.localScale = new Vector3(200,200,25);
                currentPlaceHolder.GetComponent<ObjectManager>().ChangeText(linkObject, true);
            }
        }

    }

    void OnMouseExit()
    {
        if (eventOn)
        {
            GetComponent<Renderer>().material.color = defaultColor;
            if (linkObject != null)
            {
                currentPlaceHolder.transform.localScale = new Vector3(0, 0, 25);
                currentPlaceHolder.SetActive(false);
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        currentGE = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
        eventOn = true;
        currentPlaceHolder = GameObject.FindWithTag("ObjectPlaceHolder");

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseDown()
    {
        if (eventOn)
        {
            if (currentGE.RollDice(currentCardEvent.type, currentCardEvent.difficulty + difficultyModifier))
            {
                Debug.Log("Win");
                currentGE.ApplyEventEffect(currentCardEvent.winEffect);
            }
            else
            {
                Debug.Log("Lose");
                currentGE.ApplyEventEffect(currentCardEvent.loseEffect);

            }


            GetComponent<Renderer>().material.color = Color.grey;
            transform.parent.GetComponent<CardManager>().finishCard();
        }
    }

    string TranslateEventEffectInString(EventEffect currentEventEffect, char checkMark)
    {
        string currentDescription = " "+ checkMark + " " + currentEventEffect.textEvenEffect ;
        if (currentEventEffect.typeEffect != "rien")
        {
            currentDescription += '\n' + "    ";
            if (currentEventEffect.objectReward == null)
            {
                if (currentEventEffect.effectGain > 0)
                {
                    currentDescription += "+";
                }
                currentDescription += currentEventEffect.effectGain.ToString() + " " + currentEventEffect.typeEffect;
            }
            else
            {
                currentDescription += "+" + currentEventEffect.objectReward.name;
            }
        }
        if (currentEventEffect.objectReward != null)
        {
            linkObject = currentEventEffect.objectReward;
        }
        return currentDescription;
    }

    public void CreateInfo(CardEvent eventInfo)
    {
        currentCardEvent = eventInfo;

        entete.text = currentCardEvent.type + " " + currentCardEvent.difficulty.ToString();

        description.text = currentCardEvent.description + '\n' + TranslateEventEffectInString(currentCardEvent.winEffect, '√') + '\n' + TranslateEventEffectInString(currentCardEvent.loseEffect, 'X');

    }

    public void UpdateDifficulty(int newDifficultyModifier)
    {
        difficultyModifier += newDifficultyModifier;

        entete.text = currentCardEvent.type + " " + (currentCardEvent.difficulty + difficultyModifier).ToString();
        if (difficultyModifier < 0)
        {
            entete.color = Color.green;
        }
        else if (difficultyModifier > 0)
        {
            entete.color = Color.red;
        }
        else
        {
            entete.color = Color.white;
        }
    }
}
