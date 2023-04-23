using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager
{
    public int health;
    public int reason;

    public int hpModifier;
    public int reasonModifier;

    public Character currentCharacter;
    public GameEngine currentGE;

    public List<Renderer> raisonBar;
    public List<Renderer> santéBar;

    public Color defaultBarColor;

    //a voir pour le gérer avec un script a part
    public Color previsouTempHpColor;
    public Color previsouTempRaisonColor;

    public Renderer currentTmpHpRender;
    public Renderer currentHpRenderd;

    public Renderer currentTmpReasonRender;
    public Renderer currentReasonRenderd;

    public StatManager(Character currentPlayer, List<Renderer> newRaisonBar, List<Renderer> newSantéBar)
    {
        raisonBar = newRaisonBar;
        santéBar = newSantéBar;
        currentCharacter = currentPlayer;
        defaultBarColor = newRaisonBar[0].material.color;
        hpModifier = 0;
        reasonModifier = 0;
        Restart();
    }

    public void gainHealth(int addHealth, bool addReasonIfFull = true)
    {
        //Debug.Log("Change de " + addHealth.ToString() + "avec comme base " + health.ToString());
        health += addHealth;
        if (health > currentCharacter.nbHealth)
        {
            health = currentCharacter.nbHealth;
            if (addReasonIfFull)
            {
                gainReason(health - currentCharacter.nbHealth, false);
            }
        }
    }

    public void gainReason(int addReason, bool addHealthIfFull = true)
    {
        reason += addReason;
        if (reason > currentCharacter.nbReason)
        {
            reason = currentCharacter.nbReason;
            if (addHealthIfFull)
            {
                gainHealth(reason - currentCharacter.nbReason, false);
            }
        }
        //Debug.Log("Max raison = " + currentCharacter.nbReason.ToString() + " current " + reason.ToString());
    }

    public void Restart()
    {
        health = currentCharacter.nbHealth;
        reason = currentCharacter.nbReason;
        updateBar();
    }

    public void updateTempReason(int modifier)
    {
        // regler le cas ou c'est pas necesaire car full partout)
        if (currentTmpReasonRender != null)
        {
            currentTmpReasonRender.material.color = previsouTempRaisonColor;
        }

        if (reasonModifier + reason > currentCharacter.nbReason && reasonModifier + reason - modifier <= currentCharacter.nbReason)
        {

            updateTempHp(reasonModifier + reason - currentCharacter.nbReason);
        }

        reasonModifier += modifier;

        if (reasonModifier + reason < 0)
        {
            currentTmpReasonRender = raisonBar[0];
        }
        else if (reasonModifier + reason > currentCharacter.nbReason)
        {
            updateTempHp(reasonModifier + reason - currentCharacter.nbReason);
            if (reason != currentCharacter.nbReason)
            {
                currentTmpReasonRender = raisonBar[currentCharacter.nbReason - 1];
            }
            else
            {
                currentTmpReasonRender = null;
            }
        }
        else
        {
            currentTmpReasonRender = raisonBar[reason + reasonModifier - 1];
        }

        if (reasonModifier != 0 && currentTmpReasonRender)
        {
            previsouTempRaisonColor = currentTmpReasonRender.material.color;
            currentTmpReasonRender.material.color = reasonModifier > 0 ? Color.green : Color.red;
        }
    }

    public void updateTempHp(int modifier)
    {
        if (currentTmpHpRender != null)
        {
            currentTmpHpRender.material.color = previsouTempHpColor;
        }


        if (modifier < 0 && hpModifier + health > currentCharacter.nbHealth && hpModifier + health - modifier <= currentCharacter.nbHealth)
        {

            updateTempReason(hpModifier + health - currentCharacter.nbHealth);
        }

        hpModifier += modifier;
        if (hpModifier + health > currentCharacter.nbHealth)
        {
            updateTempReason(hpModifier + health - currentCharacter.nbHealth);
            if (health != currentCharacter.nbHealth)
            {
                currentTmpHpRender = santéBar[currentCharacter.nbHealth - 1];
            }
            else
            {

                currentTmpHpRender = null;
            }
        }
        else if (hpModifier + health < 0)
        {
            currentTmpHpRender = santéBar[0];

        }
        else
        {
            currentTmpHpRender = santéBar[health + hpModifier - 1];
        }


        if (hpModifier != 0 && currentTmpHpRender)
        {
            previsouTempHpColor = currentTmpHpRender.material.color;
            currentTmpHpRender.material.color = hpModifier > 0 ? Color.green : Color.red;
        }
    }

    public void updateBar()
    {
        if (currentTmpHpRender != null)
        {
            hpModifier = 0;
            currentTmpHpRender.material.color = defaultBarColor;
        }

        if (currentTmpReasonRender != null)
        {
            reasonModifier = 0;
            currentTmpReasonRender.material.color = defaultBarColor;

        }

        //Debug.Log("Current vie " + health.ToString());
        if (currentHpRenderd != santéBar[health > 0 ? health - 1 : 0])
        {
            if (currentHpRenderd != null)
            {
                currentHpRenderd.material.color = defaultBarColor;
            }
            santéBar[health > 0 ? health - 1 : 0].material.color = Color.yellow;
            currentHpRenderd = santéBar[health > 0 ? health - 1 : 0];
        }

        //Debug.Log("Current reason " + reason.ToString());
        if (currentReasonRenderd != raisonBar[reason > 0 ? reason - 1 : 0])
        {
            if (currentReasonRenderd != null)
            {
                currentReasonRenderd.material.color = defaultBarColor;

            }
            raisonBar[reason > 0 ? reason - 1 : 0].material.color = Color.yellow;
            currentReasonRenderd = raisonBar[reason > 0 ? reason - 1 : 0];
        }
    }
}

public class PlayerManager : MonoBehaviour
{
    public Character currentCharacter;
    public StatManager currentStat;
    public GameEngine currentGE;
    public bool showLore;
    private Animator playerAnimator;

    public List<Renderer> raisonBar;
    public List<Renderer> santéBar;

    void OnMouseDown()
    {
        showLore = !showLore;
        playerAnimator.SetBool("Lore", showLore);
    }

        // Start is called before the first frame update
    public void Awake()
    {
        showLore = false;
        currentStat = new StatManager(currentCharacter, raisonBar, santéBar);
        currentGE = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    public void Restart()
    {
        currentStat.Restart();
    }

    public void ChangeStat(string type, int amount)
    {
        //Debug.LogFormat("Change start for {0} with an amount of {1}", type, amount);
        if (type.Contains("santé")){
            //Debug.LogFormat("Change santé by {0}", amount);
            currentStat.gainHealth(amount);
        }
        if (type.Contains("raison"))
        {
            //Debug.LogFormat("Change raison by {0}", amount);
            currentStat.gainReason(amount);
        }

        currentStat.updateBar();
        //Debug.LogFormat("Current stat Hp : {0} Re: {1}", currentStat.health, currentStat.reason);
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
        Debug.LogError("Value" + statWanted + "Is not a correct dice Type");
        return 0;
    }

}
