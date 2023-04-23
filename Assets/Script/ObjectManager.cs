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
    public Vector3 previousPosition;

    public string typeEffect;
    public string gainEffect;

    public TMP_Text titre;
    public TMP_Text description;
    public TMP_Text effet;

    public int tempZ;

    public GameEngine currentGE;

    void OnMouseOver()
    {
        if (!isActive)
        {
            renderer.material.color = Color.red;

        }
        if (!isOverlay && gameObject.transform.localPosition.y != 0f)
        {
            previousPosition = gameObject.transform.localPosition;
            Vector3 p = gameObject.transform.localPosition;
            p.z = p.z + tempZ * 0.126f;
            p.y = 0f;
            p.x = -1f;
            gameObject.transform.localPosition = p;

        }

    }

    public void DeleteObject()
    {
        currentGE.inventory.GetComponent<InventoryManager>().DeleteObject(gameObject);
        Destroy(gameObject);
    }



    void OnMouseDown()
    {
        if (!isOverlay)
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
            gameObject.transform.localPosition = previousPosition;
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
        titre.text = newObject.name;
        description.text = newObject.description;
        effet.text = newObject.effect;
        isOverlay = newIsOverlay;
        typeEffect = newObject.typeEffect;
        gainEffect = newObject.gainEffect;
    }
}
