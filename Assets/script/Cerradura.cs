using UnityEngine;
using UnityEngine.InputSystem;

public class InteraccionCerradura : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color colorOriginal;

    [Header("Configuración de Brillo/Parpadeo")]
    public Color colorBrillo = Color.yellow; // Color que tomará la cerradura al activarse
    public float velocidadParpadeo = 5f;

    [Header("UI del Minijuego")]
    public GameObject panelPuzzleDeslizar; // La Imagen/Panel de fondo que contiene el puzzle
    public GameObject canvasTextoE;        // El cartel de "Pulsa E para forzar cerradura"

    private bool estaCerca = false;

    void Start()
    {
        // Guardamos el SpriteRenderer de la cerradura y su color original
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) colorOriginal = sr.color;

        if (canvasTextoE != null) canvasTextoE.SetActive(false);
    }

    void Update()
    {
        // 1. Efecto de parpadeo: solo ocurre si el script ha sido activado por la jaula
        if (sr != null)
        {
            float oscilacion = Mathf.Sin(Time.time * velocidadParpadeo);
            sr.color = (oscilacion > 0) ? colorBrillo : colorOriginal;
        }

        // 2. Control de la interacción con la E
        if (estaCerca)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("¡Cerradura: Pulsaste la E correctamente!");

                if (panelPuzzleDeslizar != null)
                {
                    panelPuzzleDeslizar.SetActive(true); // Abrimos el minijuego de deslizar
                    Debug.Log("¡Cerradura: He activado el objeto del puzzle: " + panelPuzzleDeslizar.name + "!");
                    
                    if (canvasTextoE != null) canvasTextoE.SetActive(false);
                    
                    // Al abrir el puzzle, restauramos su color y apagamos este script para que deje de parpadear
                    if (sr != null) sr.color = colorOriginal;
                    this.enabled = false; 
                }
                else
                {
                    Debug.LogError("¡ERROR! La casilla 'Panel Puzzle Deslizar' está VACÍA en el inspector de la cerradura.");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Unity solo lee este Trigger si el script está activado (es decir, si ya terminó la conversación)
        if (other.CompareTag("Player"))
        {
            estaCerca = true;
            if (canvasTextoE != null) canvasTextoE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = false;
            if (canvasTextoE != null) canvasTextoE.SetActive(false);
        }
    }
}