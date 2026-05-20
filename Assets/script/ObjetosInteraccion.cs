using UnityEngine;
using UnityEngine.InputSystem;

public class interaccionObjetos : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color colorOriginal;
   
    public Color colorBrillo = Color.yellow;
    public float velocidadBrillo = 5f; 
    public GameObject canvasTexto; 
   
    [Header("Configuración de la Imagen con Fade (LLAVE)")]
    public GameObject objetoConFade;
    private lanzarMensaje scriptFade;

    private bool estaCerca = false;
    private bool yaDioLlave = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        colorOriginal = sr.color;
       
        if (canvasTexto != null) canvasTexto.SetActive(false);
       
        if (objetoConFade != null)
        {
            scriptFade = objetoConFade.GetComponent<lanzarMensaje>();
        }
    }

    void Update()
    {
        
        if (!GameManager.Instance.haVistoMensajeCaja || yaDioLlave) 
        {
            return; 
        }

        if (estaCerca)
        {
            
            float oscilacion = Mathf.Sin(Time.time * velocidadBrillo);
            sr.color = (oscilacion > 0) ? colorBrillo : colorOriginal;

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                EntregarLlave();
            }
        }
    }

    private void EntregarLlave()
    {
        
        yaDioLlave = true; 
        sr.color = colorOriginal;

        if (canvasTexto != null) canvasTexto.SetActive(false);

       
        GameManager.Instance.RecogerLlave();

        if (scriptFade != null)
        {
            scriptFade.LanzarMensaje();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Player") && GameManager.Instance.haVistoMensajeCaja && !yaDioLlave)
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
            sr.color = colorOriginal; 
            if (canvasTexto != null) canvasTexto.SetActive(false);
        }
    }
}