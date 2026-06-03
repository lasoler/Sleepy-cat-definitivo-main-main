using UnityEngine;

public class AmbienteSotano : MonoBehaviour
{
    [Header("Configuración de Sonidos Autónomos")]
    public AudioClip[] sonidosDelSotano;      // Tus audios de crujidos, gotas, etc.
    public float tiempoMinimo = 6f;           // Tiempo mínimo de espera (en segundos)
    public float tiempoMaximo = 18f;          // Tiempo máximo de espera (en segundos)
    
    [Range(0f, 1f)] 
    public float volumenSotano = 0.2f;        // Volumen de fondo sutil para no tapar pasos

    private AudioSource audioSource;
    private float cronometro = 0f;
    private float tiempoSiguienteSonido = 0f;

    void Start()
    {
        // Añadimos el componente de audio automáticamente al objeto
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Calculamos la primera espera para que empiece la cuenta atrás
        CalcularSiguienteTiempo();
    }

    void Update()
    {
        // El cronómetro avanza frame a frame
        cronometro += Time.deltaTime;

        if (cronometro >= tiempoSiguienteSonido)
        {
            ReproducirSonidoAleatorio();
            cronometro = 0f;                  // Reseteamos el reloj
            CalcularSiguienteTiempo();        // Elegimos un nuevo tiempo al azar
        }
    }

    void ReproducirSonidoAleatorio()
    {
        if (sonidosDelSotano == null || sonidosDelSotano.Length == 0) return;

        // Elegimos un audio de la lista al azar
        int indiceAleatorio = Random.Range(0, sonidosDelSotano.Length);

        // Cambiamos el pitch un poco para que no suene robótico ni siempre igual
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = volumenSotano;

        // Reproduce el sonido
        audioSource.PlayOneShot(sonidosDelSotano[indiceAleatorio]);
        Debug.Log("Sótano: Sonido ambiental reproducido de forma autónoma.");
    }

    void CalcularSiguienteTiempo()
    {
        tiempoSiguienteSonido = Random.Range(tiempoMinimo, tiempoMaximo);
    }
}
