using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecalculatePosition()
    {
        //comment faire si on dépâsse les 10 
        // Debug.Log(gameObject.name + " has " + gameObject.transform.childCount + " children");


        float stepCard = 0.8f;

        float firstStep = -((float)gameObject.transform.childCount / 2 * stepCard) + 0.3f;

        //Debug.Log("firstStep is" + firstStep.ToString());

        for (int indexObject = 0; indexObject < gameObject.transform.childCount; indexObject++)
        {
            Vector3 p = new Vector3(-0.03f * indexObject, 0.12f * indexObject, firstStep + stepCard * indexObject);
            gameObject.transform.GetChild(indexObject).transform.localPosition = p;
        }
        
    }
}
