using UnityEngine;
using UnityEngine.SceneManagement; 

public class CambioEscena : MonoBehaviour
{
    [SerializeField] private string nombreEscena; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            ControladorGlobal.puntoAparicion = 2;
            SceneManager.LoadScene(nombreEscena);
        }
    }
}