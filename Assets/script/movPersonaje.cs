using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 

public class movpersonaje : MonoBehaviour
{
    public float velocidad = 0.5f;
    public Vector3 inicioPersonaje = new Vector3(1, 2, 0); 
    
    Animator animator;
    SpriteRenderer sr;

    [Header("Configuración de Sonido de Pasos (Loop)")]
    public AudioClip sonidoDePasoLoop;       // Tu sonido de paso recortado
    private AudioSource audioSourcePasos;

    [Header("Configuración de Maullidos Temporales")]
    public AudioClip[] sonidosPeriodicos;     // Tus maullidos
    public float tiempoMinimo = 10f;          // Tiempo mínimo en segundos
    public float tiempoMaximo = 25f;          // Tiempo máximo en segundos
    private AudioSource audioSourcePeriodico; 
    private float cronometroPeriodico = 0f;
    private float tiempoSiguienteSonido = 0f;

    void Awake()
    {
        GameObject[] objetosPersonaje = GameObject.FindGameObjectsWithTag("Player");

        if (objetosPersonaje.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        
        // --- CONFIGURACIÓN DE AUDIO ---
        
        // 1. Canal de Pasos: Configurado con LOOP nativo
        audioSourcePasos = gameObject.AddComponent<AudioSource>();
        audioSourcePasos.playOnAwake = false;
        audioSourcePasos.loop = true; 
        audioSourcePasos.volume = 1f; // Volumen de las pisadas

        // 2. Canal de Maullidos: Independiente y SIN loop
        audioSourcePeriodico = gameObject.AddComponent<AudioSource>();
        audioSourcePeriodico.playOnAwake = false;
        audioSourcePeriodico.loop = false;
        
        // Calculamos el primer tiempo para el maullido
        CalcularSiguienteTiempo();

        PosicionarGato();
    }

    private void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
    private void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

    void AlCargarEscena(Scene scene, LoadSceneMode mode)
    {
        PosicionarGato();
    }

    void PosicionarGato()
    {
        if (ControladorGlobal.puntoAparicion == 1)
        {
            GameObject spawn = GameObject.FindWithTag("spawnCaja");
            if (spawn != null) inicioPersonaje = spawn.transform.position;
        }
        else if (ControladorGlobal.puntoAparicion == 2)
        {
            GameObject spawn = GameObject.FindWithTag("spawnSotano");
            if (spawn != null) inicioPersonaje = spawn.transform.position;
        }
        else if (ControladorGlobal.puntoAparicion == 3)
        {
            GameObject spawn = GameObject.FindWithTag("spawnEntradaSotano");
            if (spawn != null) inicioPersonaje = spawn.transform.position;
        }

        this.transform.position = new Vector3(inicioPersonaje.x, inicioPersonaje.y, 0);
    }

    void Update()
    {
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
        this.transform.Translate(moveInput.x * velocidad, moveInput.y * velocidad, 0);

        if (moveInput.x < 0) sr.flipX = true;
        else if (moveInput.x > 0) sr.flipX = false;

        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        
        bool estaMoviendose = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", estaMoviendose);

        // --- 1. CONTROL DE PASOS (CON LOOP NATIVO CORREGIDO) ---
        if (estaMoviendose)
        {
            // Protegemos la lectura verificando primero que audioSourcePasos exista
            if (audioSourcePasos != null && !audioSourcePasos.isPlaying && sonidoDePasoLoop != null)
            {
                audioSourcePasos.clip = sonidoDePasoLoop;
                audioSourcePasos.Play();
            }
        }
        else
        {
            // Protegemos también la parada para evitar errores nulos
            if (audioSourcePasos != null && audioSourcePasos.isPlaying)
            {
                audioSourcePasos.Stop();
            }
        }

        // --- 2. CONTROL DE MAULLIDOS TEMPORALES ---
        cronometroPeriodico += Time.deltaTime;

        if (cronometroPeriodico >= tiempoSiguienteSonido)
        {
            ReproducirSonidoPeriodico();
            cronometroPeriodico = 0f;
            CalcularSiguienteTiempo(); 
        }
    }

    void ReproducirSonidoPeriodico()
    {
        if (sonidosPeriodicos == null || sonidosPeriodicos.Length == 0) return;
        
        int indiceAleatorio = Random.Range(0, sonidosPeriodicos.Length);
        
        audioSourcePeriodico.pitch = Random.Range(0.95f, 1.05f);
        // Volumen de los maullidos (un poco más alto que los pasos)
        audioSourcePeriodico.volume = Random.Range(0.3f, 0.5f); 
        
        audioSourcePeriodico.PlayOneShot(sonidosPeriodicos[indiceAleatorio]);
        Debug.Log("El gato ha maullado de forma temporal.");
    }

    void CalcularSiguienteTiempo()
    {
        tiempoSiguienteSonido = Random.Range(tiempoMinimo, tiempoMaximo);
    }
}
