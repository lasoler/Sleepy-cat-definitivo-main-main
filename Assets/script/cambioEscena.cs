using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarAlSotano : MonoBehaviour
{
    public string escenaSotano = "sotano"; // Escribe aquí el nombre exacto de tu escena del sótano

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el gato (Player) toca la puerta o entrada al sótano
        if (other.CompareTag("Player"))
        {
            // Cambiamos a la escena del sótano inmediatamente
            SceneManager.LoadScene(escenaSotano);
        }
    }
}