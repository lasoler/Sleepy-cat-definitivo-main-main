using UnityEngine;
using UnityEngine.InputSystem;

public class VentanaSotanoInteraccion : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color colorOriginal;

    [Header("Configuración de Brillo")]
    public Color colorBrillo = Color.cyan; 
    public float velocidad = 5f;

    [Header("UI e Interacción (Mismo Estilo Cerradura)")]
    public GameObject canvasTexto;           // El cartel de "Pulsa E" para la ventana
    public GameObject panelRomperCristal;   // La Imagen/Panel de fondo que contiene el minijuego de los cristales

    private bool estaActiva = false;        // Controla si el puzzle de la cerradura ya fue resuelto
    private bool estaCerca = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) colorOriginal = sr.color;
       
        if (canvasTexto != null) canvasTexto.SetActive(false);
        if (panelRomperCristal != null) panelRomperCristal.SetActive(false);
    }

    void Update()
    {
        // 1. El parpadeo ocurre siempre que la ventana esté activa tras ganar el puzzle anterior
        if (estaActiva && sr != null)
        {
            float oscilacion = Mathf.Sin(Time.time * velocidad);
            sr.color = (oscilacion > 0) ? colorBrillo : colorOriginal;
        }

        // 2. Procesamos la pulsación de la E solo si estamos en la zona de la ventana
        if (estaActiva && estaCerca)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (panelRomperCristal != null)
                {
                    panelRomperCristal.SetActive(true); // Encendemos el minijuego de los cristales con tiempo
                    
                    if (canvasTexto != null) canvasTexto.SetActive(false);
                    if (sr != null) sr.color = colorOriginal; // Devolvemos el color normal
                    
                    estaCerca = false; // Desactivamos interacciones repetidas
                    this.enabled = false; // Apagamos este script de interacción mientras juega
                }
                else
                {
                    Debug.LogError("¡ERROR! No has arrastrado la Imagen/Panel en la casilla 'Panel Romper Cristal' de la Ventana.");
                }
            }
        }
    }

    // Este método lo llama el script del puzzle numérico al ganar
    public void ActivarVentana()
    {
        estaActiva = true;
        Debug.Log("La ventana del sótano ahora está activa y parpadeando.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Algo ha entrado en el Trigger de la ventana: " + other.gameObject.name + " con el Tag: " + other.gameObject.tag);
        // Solo reacciona al jugador si el puzzle numérico ya fue resuelto
        if (other.CompareTag("Player") && estaActiva)
        {
            estaCerca = true;
            if (canvasTexto != null) canvasTexto.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = false;
            if (canvasTexto != null) canvasTexto.SetActive(false);
        }
    }
}