using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class EventManager : MonoBehaviour
{
    public CardEvent currentCardEvent;

    public TMP_Text entete;
    public TMP_Text description;

    public Color defaultColor;

    public GameEngine currentGE;


    void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.red;

    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = defaultColor;

    }
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        currentGE = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseDown()
    {
        if (currentGE.RollDice(currentCardEvent.type, currentCardEvent.difficulty))
        {
            Debug.Log("Win");
            currentGE.ApplyEventEffect(currentCardEvent.winEffect);
        }
        else
        {
            Debug.Log("Lose");
            currentGE.ApplyEventEffect(currentCardEvent.loseEffect);

        }

    }

    string TranslateEventEffectInString(EventEffect currentEventEffect)
    {
        string currentDescription = " - " + currentEventEffect.textEvenEffect ;
        if (currentEventEffect.typeEffect != null)
        {
            currentDescription += '\n' + "    ";
            if (currentEventEffect.typeEffect != "object")
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
        return currentDescription;
    }

    public void CreateInfo(CardEvent eventInfo)
    {
        currentCardEvent = eventInfo;

        entete.text = currentCardEvent.type + " " + currentCardEvent.difficulty.ToString();



        description.text = currentCardEvent.description + '\n' + TranslateEventEffectInString(currentCardEvent.winEffect) + '\n' + TranslateEventEffectInString(currentCardEvent.loseEffect);

    }
}
