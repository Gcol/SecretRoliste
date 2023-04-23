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
    public GameObject currentCardGO;
    public List<ObjectManager> objectActivate;
    public List<GameObject> listCard;

    public int indexCard;

    public GameObject diceEtrange;
    public GameObject diceDemon;
    public GameObject diceRational;

    public Animator victoryAnim;

    public bool winLock;
    public bool resetDone;

    public int testNb = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentCardGO = null;
        resetDone = true;
        readData();
        //CreateCard();
        Restart();
        winLock = true;

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
        //Debug.Log("Create" + allObject.Count + "object");
            

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
        //Debug.Log("Create" + nbCard.ToString()+ "card");
    }

    void CreateCard()
    {
        GameObject diceEtrange = GameObject.FindWithTag("StrangeDice");
        GameObject diceDémon = GameObject.FindWithTag("DémonDice");
        GameObject diceRational = GameObject.FindWithTag("RationalDice");

        foreach (CardInfo newCard in allCardForThisGame)
        {
            GameObject newInstance = Instantiate(cardPrefab, spawnCard.transform);
            newInstance.GetComponent<CardManager>().FillNewCard(newCard, diceEtrange, diceDémon, diceRational);
            listCard.Add(newInstance);
        }
       

    }



    public void Restart()
    {
        if (resetDone)
        {
            if (currentCardGO != null)
            {
                currentCardGO.transform.GetComponent<Animator>().SetInteger("State", 1);
                currentCard.Reset();
            }
            resetDone = false;
            indexCard = 0;
            currentCardGO = null;
            currentCard = null;
            Debug.Log("Restart");
            loseState = null;
            losePannel.SetActive(false);
            objectActivate = new List<ObjectManager>();
            StartCoroutine(CoRoutineSpawnCard());
            inventory.GetComponent<InventoryManager>().Reset();
            currentPM.Restart();
            victoryAnim.SetBool("Show", false);

            resetDone = true;


        }
    }

    IEnumerator CoRoutineSpawnCard()
    {
        bool cardDraw = false;
        int indexCard = 0;
        foreach (GameObject newCard in listCard)
        {
            if (newCard.transform.GetComponent<Animator>().GetInteger("State") != 1)
            {
                newCard.transform.GetComponent<Animator>().SetInteger("State", 1);
                newCard.transform.GetComponent<CardManager>().Reset();
                yield return new WaitForSeconds(0.5f);
            }
            if (!cardDraw)
            {
                cardDraw = true;
                DrawCard();
            }
        }
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
        if (indexCard < listCard.Count - 1 )
        {
            listCard[indexCard].GetComponent<CardManager>().DisplayCard();
            currentCardGO = listCard[indexCard];
            currentCard = listCard[indexCard].GetComponent<CardManager>();
            indexCard++;
        }
        else
        {
            victoryAnim.SetBool("Show", true);
        }

    }


    public CardInfo GetNewCard()
    {
        // Ajouter une condition de fin de carte
        CardInfo newCard = allCardForThisGame[0];

        allCardForThisGame.Remove(allCardForThisGame[0]);
        /*
        if (allCardForThisGame.Count == 0)
        {
            newCard.isBoss = true;
        }*/
        return newCard;

    }

    public void DrawObject()
    {
        GameObject newInstance = Instantiate(objectPrefab, inventory.transform);
        newInstance.GetComponent<ObjectManager>().ChangeText(allObject[testNb]);
        inventory.GetComponent<InventoryManager>().AddObject(newInstance);
        inventory.GetComponent<InventoryManager>().RecalculatePosition();
        testNb++;
    }

    public bool RollDice(GameObject currentDice, string diceType, int difficulty)
    {
        int currentRoll = Random.Range(1, currentPM.returnStatDice(diceType.ToLower()));
        /*if (winLock)
        {
            currentRoll = 100;

        }
        else
        {
            currentRoll = 0;
        }*/
        currentDice.GetComponent<Animator>().SetTrigger("Roll");
        currentDice.transform.GetChild(0).GetComponent<TMP_Text>().text = currentRoll.ToString();
        //Debug.LogFormat("current dice {0} for max {1} I roll {2} with diff of {3} my win is {4}", diceType, currentPM.returnStatDice(diceType.ToLower()), currentRoll, difficulty, difficulty >= currentRoll);
        cleanObject();
        return currentRoll >= difficulty;
    }

    public void ApplyEventEffect(EventEffect newEventEffect, int reductionMalus = 0)
    {
        if (newEventEffect.typeEffect != null)
        {
            if (newEventEffect.objectReward != null)
            {
                GameObject newInstance = Instantiate(objectPrefab, inventory.transform) ;
                newInstance.GetComponent<ObjectManager>().ChangeText(newEventEffect.objectReward);
                inventory.GetComponent<InventoryManager>().AddObject(newInstance);
                inventory.GetComponent<InventoryManager>().RecalculatePosition();
            }
            else
            {
                currentPM.ChangeStat(newEventEffect.typeEffect, newEventEffect.effectGain - reductionMalus);
            }
        }
    }

    public void AddUseObject(ObjectManager currentOM)
    {
        objectActivate.Add(currentOM);

        //Debug.Log("Utilisation de " + currentOM.typeEffect);
        if (currentOM.typeEffect == "étrange")
        {
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
        else if (currentOM.typeEffect == "raison")
        {
            currentPM.currentStat.updateTempReason(int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "santé" )
        {
            currentPM.currentStat.updateTempHp(int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "raison+étrange")
        {
            currentPM.currentStat.updateTempReason(int.Parse(currentOM.gainEffect.Split(",")[0]));
            currentCard.choice1.UpdateDifficulty(int.Parse(currentOM.gainEffect.Split(",")[1]));
        }
        else if (currentOM.typeEffect == "raison+démon")
        {
            currentPM.currentStat.updateTempReason(int.Parse(currentOM.gainEffect.Split(",")[0]));
            currentCard.choice2.UpdateDifficulty(int.Parse(currentOM.gainEffect.Split(",")[1]));
        }
        else if (currentOM.typeEffect == "santé+raison")
        {
            currentPM.currentStat.updateTempHp(int.Parse(currentOM.gainEffect.Split(",")[0]));
            currentPM.currentStat.updateTempReason(int.Parse(currentOM.gainEffect.Split(",")[1]));
        }
        else if (currentOM.typeEffect == "ignore")
        {
            currentCard.choice1.IgnoreEvent();
            currentCard.choice2.IgnoreEvent();
            currentCard.choice3.IgnoreEvent();
        }
        else if (currentOM.typeEffect == "inverser")
        {
            currentCard.choice1.InverserEvent();
            currentCard.choice2.InverserEvent();
            currentCard.choice3.InverserEvent();
        }
        else if (currentOM.typeEffect == "relance")
        {
            currentCard.choice1.UpdateRelance(int.Parse(currentOM.gainEffect));
            currentCard.choice2.UpdateRelance(int.Parse(currentOM.gainEffect));
            currentCard.choice3.UpdateRelance(int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "ignoreNéga")
        {
            currentCard.choice1.UpdateIgnoreNega();
            currentCard.choice2.UpdateIgnoreNega();
            currentCard.choice3.UpdateIgnoreNega();
        }
        else if (currentOM.typeEffect == "changeAction")
        {
            currentCard.updateChangeAction();
        }
        else if (currentOM.typeEffect == "réductionRaison")
        {
            currentCard.choice1.UpdateReductionRaison(int.Parse(currentOM.gainEffect));
            currentCard.choice2.UpdateReductionRaison(int.Parse(currentOM.gainEffect));
            currentCard.choice3.UpdateReductionRaison(int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "réductionSantéRaison")
        {
            currentCard.choice1.UpdateReductionGlobal(int.Parse(currentOM.gainEffect));
            currentCard.choice2.UpdateReductionGlobal(int.Parse(currentOM.gainEffect));
            currentCard.choice3.UpdateReductionGlobal(int.Parse(currentOM.gainEffect));
        }
        else {

            Debug.LogError("'" + currentOM.typeEffect.Trim() + "' n'as pas était trouvé dans la liste des effets existant");
        }
    }

    public void DeleteUseObject(ObjectManager currentOM)
    {
        // pense a appliquer les effets de modification de vie
        objectActivate.Remove(currentOM);
        if (currentOM.typeEffect == "étrange")
        {
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
        else if (currentOM.typeEffect == "raison")
        {
            currentPM.currentStat.updateTempReason(-int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "santé")
        {
            currentPM.currentStat.updateTempHp(-int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "raison+étrange")
        {
            currentPM.currentStat.updateTempReason(-int.Parse(currentOM.gainEffect.Split(",")[0]));
            currentCard.choice1.UpdateDifficulty(-int.Parse(currentOM.gainEffect.Split(",")[1]));
        }
        else if (currentOM.typeEffect == "raison+démon")
        {
            currentPM.currentStat.updateTempReason(-int.Parse(currentOM.gainEffect.Split(",")[0]));
            currentCard.choice2.UpdateDifficulty(-int.Parse(currentOM.gainEffect.Split(",")[1]));
        }
        else if (currentOM.typeEffect == "santé+raison")
        {
            currentPM.currentStat.updateTempHp(-int.Parse(currentOM.gainEffect.Split(",")[0]));
            currentPM.currentStat.updateTempReason(-int.Parse(currentOM.gainEffect.Split(",")[1]));
        }
        else if (currentOM.typeEffect == "ignore")
        {
            currentCard.choice1.IgnoreEvent();
            currentCard.choice2.IgnoreEvent();
            currentCard.choice3.IgnoreEvent();
        }
        else if (currentOM.typeEffect == "inverser")
        {
            currentCard.choice1.InverserEvent();
            currentCard.choice2.InverserEvent();
            currentCard.choice3.InverserEvent();
        }
        else if (currentOM.typeEffect == "relance")
        {
            currentCard.choice1.UpdateRelance(-int.Parse(currentOM.gainEffect));
            currentCard.choice2.UpdateRelance(-int.Parse(currentOM.gainEffect));
            currentCard.choice3.UpdateRelance(-int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "ignoreNéga")
        {
            currentCard.choice1.UpdateIgnoreNega();
            currentCard.choice2.UpdateIgnoreNega();
            currentCard.choice3.UpdateIgnoreNega();
        }
        else if (currentOM.typeEffect == "changeAction")
        {
            //Salut
        }
        else if (currentOM.typeEffect == "réductionRaison")
        {
            currentCard.choice1.UpdateReductionRaison(-int.Parse(currentOM.gainEffect));
            currentCard.choice2.UpdateReductionRaison(-int.Parse(currentOM.gainEffect));
            currentCard.choice3.UpdateReductionRaison(-int.Parse(currentOM.gainEffect));
        }
        else if (currentOM.typeEffect == "réductionSantéRaison")
        {
            currentCard.choice1.UpdateReductionGlobal(-int.Parse(currentOM.gainEffect));
            currentCard.choice2.UpdateReductionGlobal(-int.Parse(currentOM.gainEffect));
            currentCard.choice3.UpdateReductionGlobal(-int.Parse(currentOM.gainEffect));
        }
        else
        {
            Debug.LogError("'" + currentOM.typeEffect.Trim() + "' n'as pas était trouvé dans la liste des effets existant");
        }
    }


    void cleanObject()
    {
        foreach (ObjectManager currentObject in objectActivate)
        {
            currentObject.DeleteObject();
        }
        objectActivate = new List<ObjectManager>();

        inventory.GetComponent<InventoryManager>().RecalculatePosition(false);
    }
}
