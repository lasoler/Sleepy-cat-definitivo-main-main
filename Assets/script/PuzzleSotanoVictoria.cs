using UnityEngine;

public class PuzzleSotanoVictoria : MonoBehaviour
{
    [Header("Referencias de la Escena")]
    public GameObject panelMinijuego;      // El panel de UI del puzzle que queremos cerrar
    public GameObject ventanaSotano;       // El objeto de la ventana en el escenario del sótano

    public void ProcesarVictoria()
    {
        Debug.Log("¡Puzzle de deslizar completado!");

        // 1. Cerramos el panel del minijuego
        if (panelMinijuego != null)
        {
            panelMinijuego.SetActive(false);
        }

        // 2. Activamos el parpadeo e interacción de la ventana
        if (ventanaSotano != null)
        {
            // Buscamos el script de la ventana (que crearemos abajo) y lo activamos
            VentanaSotanoInteraccion scriptVentana = ventanaSotano.GetComponent<VentanaSotanoInteraccion>();
            if (scriptVentana != null)
            {
                scriptVentana.ActivarVentana();
            }
        }
    }
}