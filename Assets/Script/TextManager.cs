using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class TextManager : MonoBehaviour
{
    public Color overColor;
    Color defaultColor;

    public string typeButton;


    void OnMouseOver()
    {
        gameObject.transform.GetComponent<TMP_Text>().color = overColor;
    }

    void OnMouseExit()
    {
        gameObject.transform.GetComponent<TMP_Text>().color = defaultColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = gameObject.transform.GetComponent<TMP_Text>().color;
    }

    void OnMouseDown()
    {
        if (typeButton == "Restart")
        {
            GameObject.FindWithTag("GameController").GetComponent<GameEngine>().Restart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
