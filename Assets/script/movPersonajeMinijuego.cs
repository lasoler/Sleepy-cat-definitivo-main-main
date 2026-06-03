using UnityEngine;
using UnityEngine.InputSystem;

public class movPersonajeMinijuego : MonoBehaviour
{
    public float velocidad = 0.5f;
    public float impulsoSalto = 10.0f;
    private bool puedoSaltar = true;
    
    Animator controlAnimacion;
    Rigidbody2D rb;
    GameObject respawn;

    // --- NUEVAS VARIABLES PARA EL AUDIO ---
    [Header("Configuración de Sonido")]
    public AudioClip sonidoSalto;           // Arrastra aquí tu audio breve de salto
    [Range(0f, 1f)] public float volumenSalto = 0.5f; // Regulador de volumen
    private AudioSource audioSourceSalto;   // Canal de audio interno

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controlAnimacion = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        respawn = GameObject.Find("respawn");
        
        // --- INICIALIZAMOS EL AUDIO ---
        audioSourceSalto = gameObject.AddComponent<AudioSource>();
        audioSourceSalto.playOnAwake = false;
        audioSourceSalto.loop = false;

        Respawnear();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
        this.transform.Translate(moveInput.x * velocidad, moveInput.y * velocidad, 0);
        
        if (moveInput.x < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        } 
        else if (moveInput.x > 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (moveInput.x != 0)
        {
            controlAnimacion.SetBool("activaCamina", true);
        }
        else
        {
            controlAnimacion.SetBool("activaCamina", false);
        }

        // SALTO
        bool salto = InputSystem.actions["Jump"].WasPressedThisFrame();
        Debug.Log(salto);
      
        if (puedoSaltar && salto)
        {
            Debug.Log("Salto");
            rb.AddForce(transform.up * impulsoSalto, ForceMode2D.Impulse);
            
            // --- REPRODUCIR EL EFECTO DE SALTO ---
            if (sonidoSalto != null && audioSourceSalto != null)
            {
                // Variamos un pelín el tono para que no suene idéntico y robótico cada vez que saltas
                audioSourceSalto.pitch = Random.Range(0.95f, 1.05f);
                audioSourceSalto.PlayOneShot(sonidoSalto, volumenSalto);
            }

            puedoSaltar = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        puedoSaltar = true;
    }

    public void Respawnear()
    {
        if (respawn != null)
        {
            transform.position = respawn.transform.position;
        }
    }
}