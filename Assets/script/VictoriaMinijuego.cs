using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaMinijuego : MonoBehaviour
{
    public string escenaPrincipal = "casa"; 

    [Header("Configuración de Sonido")]
    public AudioClip sonidoLlave; // <-- Arrastra aquí tu audio de victoria o de coger la llave

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            // 1. REPRODUCIR EL SONIDO (Sobrevive al cambio de escena)
            if (sonidoLlave != null)
            {
                // Reproduce el sonido en la posición actual de la llave
                AudioSource.PlayClipAtPoint(sonidoLlave, transform.position);
            }

            ControladorGlobal.tieneLlave = true;
            Debug.Log("Variable tieneLlave activada: " + ControladorGlobal.tieneLlave);

            ControladorGlobal.puntoAparicion = 1;
            
            // 2. Guardamos el estado en el GameManager de que el juego ya fue superado
            GameManager.Instance.RegistrarVictoriaMinijuego();
            
            // 3. Cargamos la escena seguros de que no romperemos ninguna animación en proceso
            SceneManager.LoadScene(escenaPrincipal);
        }
    }
}