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

    public GameObject currentDice;
    public Color defaultDiceColor;

    public bool ignoreEffect;
    public bool inverseEffect;

    public int reductionGlobal;
    public int reductionRaison;

    public bool ignoreNega;

    public int nbLancer;

    public bool changeAction;

    void OnMouseOver()
    {
        if (eventOn)
        {
            GetComponent<Renderer>().material.color = Color.red;
            currentDice.GetComponent<Renderer>().material.color = Color.yellow;
            if (linkObject != null && currentPlaceHolder.GetComponent<ObjectManager>().titre.text != linkObject.name)
            {
                currentPlaceHolder.GetComponent<ObjectManager>().ChangeText(linkObject, true);
            }
            if (linkObject != null && currentPlaceHolder.transform.localScale.x == 0)
            {
                currentPlaceHolder.transform.localScale = new Vector3(150, 150, 25);
            }
        }

    }

    void OnMouseExit()
    {
        if (eventOn)
        {
            ExitEvent();
        }

    }

    public void ExitEvent()
    {
        GetComponent<Renderer>().material.color = defaultColor;
        currentDice.GetComponent<Renderer>().material.color = defaultDiceColor;
        if (linkObject != null)
        {
            currentPlaceHolder.transform.localScale = new Vector3(0, 0, 25);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(GetComponent<Renderer>().material.color);
        defaultColor = new Color(0.5f, 0.5f, 0.5f);
        currentGE = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
        currentPlaceHolder = GameObject.FindWithTag("ObjectPlaceHolder");
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        nbLancer = 1;
        eventOn = true;
        ignoreNega = false;
        ignoreEffect = false;
        inverseEffect = false;
        changeAction = false;
        reductionRaison = 0;
        reductionGlobal = 0;
        difficultyModifier = 0;
        GetComponent<Renderer>().material.color = defaultColor;

    }

    void OnMouseDown()
    {
        if (eventOn)
        {
            eventOn = false;
            ExitEvent();
            bool win = false;
            // voir pour locker la carte Event
            for (int lancer = 0; lancer < nbLancer; lancer ++)
            {
                if (currentGE.RollDice(currentDice, currentCardEvent.type, currentCardEvent.difficulty + difficultyModifier))
                {
                    win = true;
                    //Debug.Log("Win");
                    break;
                }
                else
                {
                    //Debug.Log("Lose");
                }
            }

            if (win == true)
            {
                currentGE.ApplyEventEffect(currentCardEvent.winEffect);
            }
            else if (changeAction != true && ignoreEffect != true && !(ignoreNega == true && (currentCardEvent.loseEffect.objectReward != null || currentCardEvent.loseEffect.typeEffect != null)))
            {
                if (reductionRaison > 0 && currentCardEvent.loseEffect.typeEffect == "raison")
                {
                    currentGE.ApplyEventEffect(currentCardEvent.loseEffect, reductionRaison - reductionGlobal);
                }
                else
                {
                    currentGE.ApplyEventEffect(currentCardEvent.loseEffect, reductionGlobal);
                }
            }

            GetComponent<Renderer>().material.color = Color.black;
            transform.parent.GetComponent<CardManager>().finishCard(win);
        }
    }


    string TranslateEventEffectInString(EventEffect currentEventEffect, char checkMark)
    {
        string currentDescription = " "+ checkMark + " " + currentEventEffect.textEvenEffect ;
        if (currentEventEffect.typeEffect != "rien" && ignoreEffect == false)
        {
            currentDescription += '\n' + "    ";
            if (currentEventEffect.objectReward == null)
            {
                if (currentEventEffect.effectGain > 0)
                {
                    currentDescription += "+" + currentEventEffect.effectGain.ToString() + " " + currentEventEffect.typeEffect;
                }
                else
                {
                    if (reductionRaison > 0 && currentEventEffect.typeEffect == "raison")
                    {

                        currentDescription += (currentEventEffect.effectGain - reductionRaison - reductionGlobal).ToString()+ " " + currentEventEffect.typeEffect;
                    }
                    else
                    {
                        currentDescription += (currentEventEffect.effectGain - reductionGlobal).ToString() + " " + currentEventEffect.typeEffect;
                    }
                }
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

    public void CreateInfo(CardEvent eventInfo, GameObject newDice)
    {
        currentDice = newDice;
        defaultDiceColor = newDice.GetComponent<Renderer>().material.color;
        currentCardEvent = eventInfo;

        entete.text = currentCardEvent.type + " " + currentCardEvent.difficulty.ToString();

        description.text = currentCardEvent.description + '\n' + TranslateEventEffectInString(currentCardEvent.winEffect, '√') + '\n' + TranslateEventEffectInString(currentCardEvent.loseEffect, 'X');

    }

    public void IgnoreEvent()
    {
        ignoreEffect = !ignoreEffect;
        description.text = currentCardEvent.description + '\n' + TranslateEventEffectInString(currentCardEvent.winEffect, '√') + '\n' + TranslateEventEffectInString(currentCardEvent.loseEffect, 'X');
    }

    public void InverserEvent()
    {
        inverseEffect = !inverseEffect;
        EventEffect tempEventEffect = currentCardEvent.winEffect;
        currentCardEvent.winEffect = currentCardEvent.loseEffect;
        currentCardEvent.loseEffect = tempEventEffect;
        
        description.text = currentCardEvent.description + '\n' + TranslateEventEffectInString(currentCardEvent.winEffect, '√') + '\n' + TranslateEventEffectInString(currentCardEvent.loseEffect, 'X');

    }

    public void UpdateRelance(int modifierLancer)
    {
        nbLancer += modifierLancer;
    }

    public void UpdateIgnoreNega()
    {
        ignoreNega = !ignoreNega;
    }

    public void UpdateReductionGlobal(int modifierReductionGlobal)
    {
        reductionGlobal += modifierReductionGlobal;
        description.text = currentCardEvent.description + '\n' + TranslateEventEffectInString(currentCardEvent.winEffect, '√') + '\n' + TranslateEventEffectInString(currentCardEvent.loseEffect, 'X');
    }

    public void UpdateReductionRaison(int modifierReductionRaison)
    {
        reductionRaison += modifierReductionRaison;
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
