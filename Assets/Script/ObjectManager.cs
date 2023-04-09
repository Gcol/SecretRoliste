using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Renderer renderer;

    void OnMouseOver()
    {
        renderer.material.color = Color.red;

    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white;

    }
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
