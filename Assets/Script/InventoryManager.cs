using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> listObject;
    public Vector3 basePosition;

    public void Start()
    {
        listObject = new List<GameObject>();
        basePosition = new Vector3(-0.943f, -0.28f, 1.439f);
    }

    public void AddObject(GameObject currentObj)
    {
        listObject.Add(currentObj);
    }

    public void DeleteObject(GameObject currentObj)
    {
        listObject.Remove(currentObj);
    }

    public void RecalculatePosition(bool newCard=true)
    {
        //comment faire si on dépâsse les 10 
        // Debug.Log(gameObject.name + " has " + gameObject.transform.childCount + " children");


        Vector3 step = new Vector3(0.092f,-0.17f, -0.6f);


        //basePosition = new Vector3(listObject.Count);

        //Debug.Log("firstStep is" + firstStep.ToString());

        int minusIndex = 1;

        if (newCard == true)
        {
            int indexLastObject = listObject.Count - 1;
            listObject[indexLastObject].transform.localPosition = new Vector3(listObject[indexLastObject].transform.localPosition.x, listObject[indexLastObject].transform.localPosition.y, listObject[indexLastObject].transform.localPosition.z - step.y * listObject.Count / 2);
            basePosition.z = 1.439f - step.y * (listObject.Count / 2 - 1);
            listObject[indexLastObject].transform.GetComponent<ObjectManager>().tempZ = 0;
            minusIndex++;
        }
        for (int indexObject = listObject.Count - minusIndex; indexObject >= 0; indexObject--)
        {
            int currentSeptMult = (listObject.Count - indexObject);
            listObject[indexObject].transform.GetComponent<ObjectManager>().tempZ = currentSeptMult;
            Vector3 p = new Vector3(basePosition.x + step.x * currentSeptMult, basePosition.y + step.y * currentSeptMult, basePosition.z + step.z * currentSeptMult);
            listObject[indexObject].transform.localPosition = p;
            listObject[indexObject].name = "ObjectNr" + indexObject.ToString();
        }
    }

    public void Reset()
    {
        foreach (GameObject currentObj in listObject)
        {
            Destroy(currentObj);
        }
        listObject = new List<GameObject>();

    }
}
