using UnityEngine;
using Unity.Cinemachine; // Si usas una versión antigua de Unity, usa: using Cinemachine;

public class CinemachineAutoFollow : MonoBehaviour
{
    void Start()
    {
        // Buscamos al objeto que tenga el Tag "Player"
        GameObject jugador = GameObject.FindWithTag("Player");

        if (jugador != null)
        {
            // Conseguimos el componente de la cámara virtual
            CinemachineCamera vcam = GetComponent<CinemachineCamera>();
            
            if (vcam != null)
            {
                // Le asignamos el transform del jugador automáticamente
                vcam.Follow = jugador.transform;
                vcam.LookAt = jugador.transform;
                Debug.Log("Cinemachine ha encontrado y enganchado al jugador con éxito.");
            }
        }
        else
        {
            Debug.LogError("¡Cinemachine no pudo encontrar al jugador! Asegúrate de que tu personaje tenga el Tag 'Player'.");
        }
    }
}