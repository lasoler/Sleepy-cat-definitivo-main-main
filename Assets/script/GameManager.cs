using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Necesario para el control de escenas

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progreso de Llaves")]
    public int llavesActuales = 0;
    public int llavesTotalesNecesarias = 4;
    public bool haVistoMensajeCaja = false;
    [HideInInspector] public bool minijuegoSuperado = false; 

    [Header("Componentes de la Interfaz (UI)")]
    // Arrastra aquí las 4 imágenes de las llaves de la esquina
    public GameObject[] iconosLlaves; 
    public float velocidadFade = 2f; 

    [Header("UI - Llave Final del Minijuego")]
    // Arrastra aquí el objeto de la nueva llave dorada/final en el Canvas
    public GameObject llaveFinalMinijuego; 

    // Referencia interna para el mensaje central que se esté mostrando actualmente
    private Coroutine corrutinaMensajeActual;

    private void Awake()
    {
        // Sistema Singleton para que el GameManager persista entre pantallas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Nos suscribimos al evento de Unity para detectar cambios de escena de forma segura
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Nos desuscribimos al destruirse el objeto para evitar fugas de memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Inicializamos los componentes de la UI al arrancar el juego
        PrepararInterfazInicial();
    }

    // Este método se ejecuta automáticamente CADA VEZ que una escena termina de cargarse
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Si el jugador acaba de ganar el minijuego y estamos de vuelta en la casa...
        if (minijuegoSuperado)
        {
            ActualizarInterfazVictoria();
        }
    }

    private void PrepararInterfazInicial()
    {
        // Ocultamos y preparamos las 4 llaves pequeñas de la esquina
        foreach (GameObject llave in iconosLlaves)
        {
            if (llave != null)
            {
                CanvasGroup cg = llave.GetComponent<CanvasGroup>();
                if (cg == null) cg = llave.AddComponent<CanvasGroup>();
                
                cg.alpha = 0f;
                llave.SetActive(false);
            }
        }

        // Preparamos también la llave final para que empiece invisible
        if (llaveFinalMinijuego != null)
        {
            CanvasGroup cgFinal = llaveFinalMinijuego.GetComponent<CanvasGroup>();
            if (cgFinal == null) cgFinal = llaveFinalMinijuego.AddComponent<CanvasGroup>();
            
            cgFinal.alpha = 0f;
            llaveFinalMinijuego.SetActive(false);
        }
    }

    // --- LÓGICA DE LAS LLAVES NORMALES (ESQUINA) ---

    public void RecogerLlave()
    {
        if (llavesActuales < llavesTotalesNecesarias)
        {
            GameObject llaveAActivar = iconosLlaves[llavesActuales];
            llavesActuales++;

            // Hacer aparecer la llave pequeña con Fade In
            StartCoroutine(FadeInObjetoUI(llaveAActivar));
        }
    }

    // --- LÓGICA POST-MINIJUEGO (LLAVE FINAL) ---

    public void RegistrarVictoriaMinijuego()
    {
        // Guardamos el estado. La animación se ejecutará en el OnSceneLoaded al cargar la casa
        minijuegoSuperado = true; 
    }

    private void ActualizarInterfazVictoria()
    {
        // 1. Apagamos por completo las 4 llaves pequeñas para limpiar la esquina
        foreach (GameObject llave in iconosLlaves)
        {
            if (llave != null)
            {
                llave.SetActive(false);
            }
        }

        // 2. Activamos la nueva llave definitiva con su propio Fade suave
        if (llaveFinalMinijuego != null)
        {
            StartCoroutine(FadeInObjetoUI(llaveFinalMinijuego));
        }
    }

    // Método genérico para hacer que cualquier elemento UI aparezca de 0 a 1 de Alpha
    private IEnumerator FadeInObjetoUI(GameObject obj)
    {
        obj.SetActive(true);
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null) cg = obj.AddComponent<CanvasGroup>();
        
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime * velocidadFade;
            yield return null;
        }
        cg.alpha = 1f;
    }

    // --- LÓGICA DE MENSAJES TEMPORALES (CENTRO) ---

    public void MostrarImagenMensaje(GameObject imagenAMostrar, float duracion)
    {
        if (imagenAMostrar == null) return;

        // Si ya hay otro cartel mostrándose, lo cancelamos para que no se solapen
        if (corrutinaMensajeActual != null)
        {
            StopCoroutine(corrutinaMensajeActual);
        }

        corrutinaMensajeActual = StartCoroutine(SecuenciaFadeMensaje(imagenAMostrar, duracion));
    }

    private IEnumerator SecuenciaFadeMensaje(GameObject objeto, float duracion)
    {
        CanvasGroup cg = objeto.GetComponent<CanvasGroup>();
        if (cg == null) cg = objeto.AddComponent<CanvasGroup>();

        objeto.SetActive(true);
        cg.alpha = 0f;

        // 1. FADE IN
        while (cg.alpha < 1f)
        {
            cg.alpha += Time.deltaTime * velocidadFade;
            yield return null;
        }
        cg.alpha = 1f;

        // 2. ESPERA
        yield return new WaitForSeconds(duracion);

        // 3. FADE OUT
        while (cg.alpha > 0f)
        {
            cg.alpha -= Time.deltaTime * velocidadFade;
            yield return null;
        }
        cg.alpha = 0f;
        objeto.SetActive(false);

        corrutinaMensajeActual = null;
    }
}