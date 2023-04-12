using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ObjectManager : MonoBehaviour
{
    private Renderer renderer;
    public Color defaultColor;
    public bool isOverlay;
    public bool isActive;
    public Vector2 previousPosition;

    public string typeEffect;
    public string gainEffect;

    public TMP_Text titre;
    public TMP_Text description;
    public TMP_Text effet;

    public GameEngine currentGE;

    void OnMouseOver()
    {
        if (!isActive)
        {
            renderer.material.color = Color.red;

        }
        if (!isOverlay && gameObject.transform.localPosition.y != -0.12f)
        {
            Vector3 p = gameObject.transform.localPosition;
            previousPosition = new Vector2(p.x, p.y);
            p.y = -0.12f;
            p.x = 0.03f;
            gameObject.transform.localPosition = p;

        }

    }

    public void DeleteObject()
    {
        Destroy(gameObject);
    }



    void OnMouseDown()
    {
        if (isOverlay)
        {
            Debug.Log("Faudrait Zoomer");
        }
        else
        {
            if (!isActive)
            {
                renderer.material.color = Color.yellow;
                isActive = true;
                currentGE.AddUseObject(this);
            }
            else
            {
                currentGE.DeleteUseObject(this);
                isActive = false;
            }
        }

    }


    void OnMouseExit()
    {
        if (!isActive)
        { 
            renderer.material.color = defaultColor;
        }
        if (!isOverlay)
        {
            Vector3 p = gameObject.transform.localPosition;
            p.y = previousPosition.y;
            p.x = previousPosition.x;
            gameObject.transform.localPosition = p;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultColor = renderer.material.color;
        currentGE = GameObject.FindWithTag("GameController").GetComponent<GameEngine>();
    }

    public void ChangeText(CardObject newObject, bool newIsOverlay = false)
    {
        if (!newIsOverlay)
        {
            Debug.Log("Ajout de l objet" + newObject.name);
        }
        titre.text = newObject.name;
        description.text = newObject.description;
        effet.text = newObject.effect;
        isOverlay = newIsOverlay;
        typeEffect = newObject.typeEffect;
        gainEffect = newObject.gainEffect;
    }
}
