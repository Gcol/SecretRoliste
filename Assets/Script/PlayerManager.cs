using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager
{
    public int health;
    public int reason;

    public Character currentCharacter;
    public GameEngine currentGE;

    public StatManager(Character currentPlayer)
    {
        currentCharacter = currentPlayer;
        Restart();
    }

    public void gainHealth(int addHealth, bool addReasonIfFull = true)
    {
        health += addHealth;
        if (health > currentCharacter.nbHealth)
        {
            gainReason(health - currentCharacter.nbHealth, false);
            health = currentCharacter.nbHealth;
        }
    }

    public void gainReason(int addReason, bool addHealthIfFull = true)
    {
        reason += addReason;
        if (reason > currentCharacter.nbReason)
        {
            gainHealth(reason - currentCharacter.nbReason, false);
            reason = currentCharacter.nbReason;
        }
    }

    public void Restart()
    {
        health = currentCharacter.nbHealth;
        reason = currentCharacter.nbReason;
    }
}

public class PlayerManager : MonoBehaviour
{
    public Character currentCharacter;
    public StatManager currentStat;
    public GameEngine currentGE;

        // Start is called before the first frame update
    public  void Start()
    {
        currentStat = new StatManager(currentCharacter);
    }

    public void Restart()
    {
        currentStat.Restart();
    }

    public void ChangeStat(string type, int amount)
    {
        Debug.LogFormat("Change start for {0} with an amount of {1}", type, amount);
        if (type.Contains("santé")){
            Debug.LogFormat("Change santé by {0}", amount);
            currentStat.gainHealth(amount);
        }
       if (type.Contains("raison"))
        {
            Debug.LogFormat("Change raison by {0}", amount);
            currentStat.gainReason(amount);
       }

       Debug.LogFormat("Current stat Hp : {0} Re: {1}", currentStat.health, currentStat.reason);
        if (currentStat.reason < 1)
        {
        currentGE.loseState = "manque de raison";


        }
        else if (currentStat.health < 1)
        {
        currentGE.loseState = "manque de santé";

        }
    }

    public int returnStatDice(string statWanted)
    {
        if (statWanted == "étrange"){
            return currentCharacter.statEtrange;
        }
        if (statWanted == "démon")
        {
            return currentCharacter.statDemon;
        }
        if (statWanted == "rationnel")
        {
            return currentCharacter.statRational;
        }
        Debug.Log("Value" + statWanted + "Is not a correct dice Type");
        return 0;
    }


}
