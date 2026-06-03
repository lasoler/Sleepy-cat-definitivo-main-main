using UnityEngine;

public class BotonSalirFinal : MonoBehaviour
{
    private void Start()
    {
        // Buscamos al gato por su Tag "Player" en cuanto carga la escena final
        GameObject gato = GameObject.FindGameObjectWithTag("Player");
        
        // Si lo encuentra, lo destruimos para que no moleste en la pantalla final
        if (gato != null)
        {
            Destroy(gato);
            Debug.Log("Gato destruido para la escena final.");
        }
    }

    // Función para el botón
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando el juego...");
        Application.Quit();
    }
}