using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoriaMinijuego : MonoBehaviour
{
    public string escenaPrincipal = "casa"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            ControladorGlobal.tieneLlave = true;
            Debug.Log("Variable tieneLlave activada: " + ControladorGlobal.tieneLlave);

            ControladorGlobal.puntoAparicion = 1;
            
            // 1. Guardamos el estado en el GameManager de que el juego ya fue superado
            GameManager.Instance.RegistrarVictoriaMinijuego();
            
            // 2. Cargamos la escena seguros de que no romperemos ninguna animación en proceso
            SceneManager.LoadScene(escenaPrincipal);
        }
    }
}