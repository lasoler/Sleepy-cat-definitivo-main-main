using UnityEngine;

public class GestorSotano : MonoBehaviour
{
    public GameObject personajeGato;
    public Transform puntoEntrada;

    void Start()
    {
        if (personajeGato != null && puntoEntrada != null)
        {
            Rigidbody2D rb = personajeGato.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;

            personajeGato.transform.position = puntoEntrada.position;

            if (rb != null) rb.simulated = true;
            Debug.Log("Gato posicionado en la entrada del sótano.");
        }
    }
}