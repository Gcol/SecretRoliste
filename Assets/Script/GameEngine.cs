using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class CardObject
{
    public string name;
    public string description;
    public string effect; // A détailler;
    public string typeEffect;
    public string gainEffect;

    public CardObject(string newName, string newDescription, string newEffect, string newTypeEffect, string newgainEffect)
    {
        name = newName;
        description = newDescription;
        effect = newEffect;
        typeEffect = newTypeEffect;
        gainEffect = newgainEffect;
        //Debug.Log("Creation de '" + newName + "'");
    }
}

public class EventEffect
{
    public string textEvenEffect;
    public string typeEffect;
    public int effectGain;
    public CardObject objectReward;

    public EventEffect(string newTextEvenEffect,  string newEffectGain, string newTypeEffect, List<CardObject> allObject)
    {
        //Debug.Log("Creation de l event '" + newTextEvenEffect+ "' // '" + newEffectGain + "' // '" + newTypeEffect + "'");
        textEvenEffect = newTextEvenEffect;
        typeEffect = newTypeEffect;
        if (typeEffect == "objet" && !int.TryParse(newEffectGain, out effectGain))
        {
            objectReward = allObject.Find((x) => x.name == newEffectGain.Trim());
            if (objectReward == null)
            {
                Debug.LogError("'" + newEffectGain.Trim() + "' n'as pas était trouvé dans la liste des objets");
            }
        }
        else if (typeEffect != "rien")
        {
            effectGain = int.Parse(newEffectGain);
            objectReward = null;

        }

    }
}

public class CardInfo
{

    public string title;
    public bool isBoss;
    public string description;
    public CardEvent[] allEvent = new CardEvent[3];

    public CardInfo(string newTitle, string newDescription, bool newIsBoss = false)
    {
        //Debug.Log("Creation de la carte '" + newTitle + "' // '" + newDescription + "'");
        isBoss = newIsBoss;
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


    public CardEvent(string newType, string newDifficulty, string new_description, EventEffect newWinEffect, EventEffect newLoseEffect)
    {
        //Debug.Log("Creation de la cardEvent '" + new_description + "' // '" + newType + "' // '" + newDifficulty);
        type = newType;
        difficulty = int.Parse(newDifficulty);
        description = new_description;

        winEffect = newWinEffect;
        loseEffect = newLoseEffect;
    }

}

public class GameEngine : MonoBehaviour
{

    public TextAsset cardFile; // Reference of CSV file
    public TextAsset objectFile; // Reference of CSV file

    private string lineSeperater = "\"EOL\""; // It defines line seperate character
    private string fieldSeperator = "\",\""; // It defines field seperate chracter

    public List<CardInfo> allCardForThisGame = new List<CardInfo>();
    public List<CardObject> allObject = new List<CardObject>();

    public PlayerManager currentPM;

    public string loseState;
    public GameObject losePannel;
    public TMP_Text textLose;

    public int nbCard;

    public GameObject cardPrefab;
    public GameObject objectPrefab;
    public GameObject spawnCard;
    public GameObject inventory;

    public CardManager currentCard;
    public List<ObjectManager> objectActivate;
    public List<GameObject> listCard;

    // Start is called before the first frame update
    void Start()
    {
        Restart();
        readData();
        CreateCard();
        DrawCard();
        GameObject.FindWithTag("ObjectPlaceHolder").transform.localScale = new Vector3(0, 0, 25);
    }


    private void readData()
    {
        string[] records = objectFile.text.Split(lineSeperater);

        for (int indexLine = 1; indexLine < records.Length; indexLine++)
        {
            string[] fields = records[indexLine].Split(fieldSeperator);

            if (fields.Length < 4)
            {
                break;
            }
            //Debug.Log(records[indexLine]);
            CardObject newCard = new CardObject(fields[0][3..].Trim(), fields[1], fields[2], fields[3], fields[4].Split('"')[0]);
            allObject.Add(newCard);
        }
        Debug.Log("Create" + allObject.Count + "object");


        records = cardFile.text.Split(lineSeperater);

        for (int indexLine = 1; indexLine < records.Length; indexLine++)
        {
            string[] fields = records[indexLine].Split(fieldSeperator);
            if (fields.Length < 24)
            {
                break;
            }
            CardInfo newCard = new CardInfo(fields[0][3..], fields[1]);


            newCard.allEvent[0] = new CardEvent(fields[2].Split(' ')[0].ToLower(), fields[2].Split(' ')[1], fields[3], new EventEffect(fields[4], fields[5], fields[6], allObject), new EventEffect(fields[7], fields[8], fields[9], allObject));
            newCard.allEvent[1] = new CardEvent(fields[10].Split(' ')[0].ToLower(), fields[10].Split(' ')[1], fields[11], new EventEffect(fields[12], fields[13], fields[14], allObject), new EventEffect(fields[15], fields[16], fields[17], allObject));  ;
            newCard.allEvent[2] = new CardEvent(fields[18].Split(' ')[0].ToLower(), fields[18].Split(' ')[1], fields[19], new EventEffect(fields[20], fields[21], fields[22], allObject), new EventEffect(fields[23], fields[24], fields[25], allObject));

            allCardForThisGame.Add(newCard);
            nbCard++;
        }
        Debug.Log("Create" + nbCard.ToString()+ "card");
    }

    void CreateCard()
    {
        int indexCard = 0;
        foreach(CardInfo newCard in allCardForThisGame)
        {
            GameObject newInstance = Instantiate(cardPrefab, new Vector3(spawnCard.transform.position.x, spawnCard.transform.position.y + 0.05f * indexCard, spawnCard.transform.position.z), spawnCard.transform.rotation);
            newInstance.GetComponent<CardManager>().FillNewCard(newCard);

            indexCard++;
            listCard.Add(newInstance);
        }
    }

    public void Restart()
    {
        loseState = null;
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

    public void DrawCard()
    {
        listCard[0].GetComponent<CardManager>().DisplayCard();
        currentCard = listCard[0].GetComponent<CardManager>();
        listCard.Remove(listCard[0]);

    }


    public CardInfo GetNewCard()
    {
        // Ajouter une condition de fin de carte
        CardInfo newCard = allCardForThisGame[0];

        allCardForThisGame.Remove(allCardForThisGame[0]);
        if (allCardForThisGame.Count == 0)
        {
            newCard.isBoss = true;
        }
        return newCard;

    }

    public bool RollDice(string diceType, int difficulty)
    {
        int currentRoll = Random.Range(1, currentPM.returnStatDice(diceType.ToLower()));
         currentRoll = 100;
        Debug.LogFormat("current dice {0} for max {1} I roll {2} with diff of {3} my win is {4}", diceType, currentPM.returnStatDice(diceType.ToLower()), currentRoll, difficulty, difficulty >= currentRoll);
        StartCoroutine(CoRoutineCleanUsedObject());
        return currentRoll >= difficulty;
    }

    public void ApplyEventEffect(EventEffect newEventEffect)
    {
        if (newEventEffect.typeEffect != null)
        {
            if (newEventEffect.objectReward != null)
            {
                Debug.Log(newEventEffect.objectReward.name);
                GameObject newInstance = Instantiate(objectPrefab, inventory.transform) ;
                objectPrefab.GetComponent<ObjectManager>().ChangeText(newEventEffect.objectReward);
                inventory.GetComponent<InventoryManager>().RecalculatePosition();
            }
            else
            {
                currentPM.ChangeStat(newEventEffect.typeEffect, newEventEffect.effectGain);
            }
        }
    }

    public void AddObjectTest()
    {
        if (inventory.GetComponent<InventoryManager>().gameObject.transform.childCount < 9)
        {
            GameObject newInstance = Instantiate(objectPrefab, inventory.transform);
            objectPrefab.GetComponent<ObjectManager>().ChangeText(allObject[0]);
            inventory.GetComponent<InventoryManager>().RecalculatePosition();

        }
    }

    public void AddUseObject(ObjectManager currentOM)
    {
        objectActivate.Add(currentOM);
        if (currentOM.typeEffect == "étrange")
        {
            //Debug.Log(currentOM.gainEffect);
            currentCard.choice1.UpdateDifficulty(int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "démon")
        {
            currentCard.choice2.UpdateDifficulty(int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "rationnel")
        {
            currentCard.choice3.UpdateDifficulty(int.Parse(currentOM.gainEffect));
        }
    }

    public void DeleteUseObject(ObjectManager currentOM)
    {
        objectActivate.Remove(currentOM);
        if (currentOM.typeEffect == "étrange")
        {
            Debug.Log(currentOM.gainEffect);
            currentCard.choice1.UpdateDifficulty(-int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "démon")
        {
            currentCard.choice2.UpdateDifficulty(-int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "rationnel")
        {
            currentCard.choice3.UpdateDifficulty(-int.Parse(currentOM.gainEffect));
        }
    }


    IEnumerator CoRoutineCleanUsedObject()
    {
        foreach (ObjectManager currentObject in objectActivate)
        {
            currentObject.DeleteObject();
        }
        objectActivate = new List<ObjectManager>();

        yield return new WaitForSeconds(0.05f);
        inventory.GetComponent<InventoryManager>().RecalculatePosition();
    }
}
