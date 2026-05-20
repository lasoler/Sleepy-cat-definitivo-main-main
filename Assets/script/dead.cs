using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class dead : MonoBehaviour

{
    private GameObject personajeMinijuego;
    private movPersonajeMinijuego movPersonajeMinijuego;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        personajeMinijuego = GameObject.Find("personajeMinijuego");
        movPersonajeMinijuego = personajeMinijuego.GetComponent<movPersonajeMinijuego>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.name == "personajeMinijuego")
        {
             movPersonajeMinijuego.Respawnear();
            
        }
        
    }
}
