using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ConversacionJaula : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color colorOriginal;

    [Header("Configuración de Brillo/Parpadeo")]
    public Color colorBrillo = Color.yellow; // Color que tomará la jaula al parpadear
    public float velocidadParpadeo = 5f;

    [Header("UI de Diálogos")]
    public GameObject contenedorDialogo; // El objeto 'GrupoDialogo' del Canvas
    public GameObject[] imagenesDialogo;  // Arrastra aquí las imágenes del diálogo EN ORDEN

    [Header("Referencias de Bloqueo")]
    public MonoBehaviour scriptCerradura; // Arrastra aquí el script de la Cerradura para activarlo al final
    public GameObject canvasTextoE;      // El cartel de "Pulsa E para hablar"

    private int indiceActual = 0;
    private bool estaCerca = false;
    private bool conversacionIniciada = false;
    private bool conversacionTerminada = false;
    private CanvasGroup cgContenedor;

    void Start()
    {
        // Obtenemos el SpriteRenderer de la jaula y guardamos su color original
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) colorOriginal = sr.color;

        if (canvasTextoE != null) canvasTextoE.SetActive(false);
        
        if (contenedorDialogo != null)
        {
            cgContenedor = contenedorDialogo.GetComponent<CanvasGroup>();
            if (cgContenedor == null) cgContenedor = contenedorDialogo.AddComponent<CanvasGroup>();
            cgContenedor.alpha = 0f;
            contenedorDialogo.SetActive(false);
        }

        // Nos aseguramos de que todas las imágenes estén apagadas al inicio
        foreach (GameObject img in imagenesDialogo)
        {
            if (img != null) img.SetActive(false);
        }
    }

    void Update()
    {
        if (!estaCerca || conversacionTerminada) return;

        // Si el jugador está cerca y NO ha empezado la conversación, la jaula parpadea
        if (!conversacionIniciada && sr != null)
        {
            float oscilacion = Mathf.Sin(Time.time * velocidadParpadeo);
            sr.color = (oscilacion > 0) ? colorBrillo : colorOriginal;
        }

        // Detectamos la pulsación de la E
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!conversacionIniciada)
            {
                IniciarConversacion();
            }
            else
            {
                AvanzarDialogo();
            }
        }
    }

    void IniciarConversacion()
    {
        conversacionIniciada = true;
        
        // Al empezar a hablar, restauramos el color original de la jaula y quitamos el "Pulsa E"
        if (sr != null) sr.color = colorOriginal;
        if (canvasTextoE != null) canvasTextoE.SetActive(false); 
        
        contenedorDialogo.SetActive(true);
        StartCoroutine(FadeCanvas(cgContenedor, 0f, 1f, 0.2f)); // Aparición suave del panel
        
        indiceActual = 0;
        imagenesDialogo[indiceActual].SetActive(true); // Mostramos la primera frase
    }

    void AvanzarDialogo()
    {
        imagenesDialogo[indiceActual].SetActive(false);
        indiceActual++;

        if (indiceActual < imagenesDialogo.Length)
        {
            imagenesDialogo[indiceActual].SetActive(true);
        }
        else
        {
            TerminarConversacion();
        }
    }

    void TerminarConversacion()
    {
        conversacionTerminada = true;
        if (sr != null) sr.color = colorOriginal; // Aseguramos que se quede con su color normal
        StartCoroutine(OcultarYDesbloquear());
    }

    IEnumerator OcultarYDesbloquear()
    {
        yield return StartCoroutine(FadeCanvas(cgContenedor, 1f, 0f, 0.2f));
        contenedorDialogo.SetActive(false);

        // ¡ACTIVAMOS LA CERRADURA!
        if (scriptCerradura != null)
        {
            scriptCerradura.enabled = true; 
            Debug.Log("Conversación terminada. La cerradura del puzzle ahora está disponible.");
        }
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float inicio, float fin, float tiempo)
    {
        float transcurrido = 0f;
        while (transcurrido < tiempo)
        {
            transcurrido += Time.deltaTime;
            cg.alpha = Mathf.Lerp(inicio, fin, transcurrido / tiempo);
            yield return null;
        }
        cg.alpha = fin;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !conversacionTerminada)
        {
            estaCerca = true;
            // Solo muestra el "Pulsa E" si no estás ya hablando con el personaje
            if (canvasTextoE != null && !conversacionIniciada) canvasTextoE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaCerca = false;
            if (canvasTextoE != null) canvasTextoE.SetActive(false);
            if (sr != null) sr.color = colorOriginal; // Devolvemos su color si el jugador se aleja
            
            // Si el jugador se va a mitad de la conversación, reiniciamos el estado por seguridad
            if (conversacionIniciada && !conversacionTerminada)
            {
                StopAllCoroutines();
                if (cgContenedor != null) cgContenedor.alpha = 0f;
                if (contenedorDialogo != null) contenedorDialogo.SetActive(false);
                foreach (GameObject img in imagenesDialogo) if (img != null) img.SetActive(false);
                conversacionIniciada = false;
            }
        }
    }
}