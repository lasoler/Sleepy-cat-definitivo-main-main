using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video; 
using UnityEngine.UI; 

public class MenuInicio : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string nombreEscenaCasa = "casa";

    [Header("Música de Fondo")]
    [Tooltip("Arrastra aquí el objeto entero que tiene la música (el GameObject)")]
    public GameObject objetoMusicaMenu; // Cambiado a GameObject para apagarlo de raíz

    [Header("Configuración del Video Intro")]
    public VideoPlayer reproductorVideo;

    private RawImage pantallaImagen;

    private void Start()
    {
        // Al empezar, nos aseguramos de que la música esté encendida
        if (objetoMusicaMenu != null)
        {
            objetoMusicaMenu.SetActive(true);
        }

        if (reproductorVideo != null)
        {
            reproductorVideo.gameObject.SetActive(true);
            
            pantallaImagen = reproductorVideo.GetComponent<RawImage>();
            if (pantallaImagen != null)
            {
                pantallaImagen.enabled = false;
            }
            
            reproductorVideo.Prepare();
        }
    }

    // 1. BOTÓN: EMPEZAR JUEGO
    public void EmpezarJuego()
    {
        // ¡GOLPE DE ESTADO! Apagamos el objeto de la música por completo.
        // Si el objeto está apagado, es físicamente imposible que Unity reproduzca su audio.
        if (objetoMusicaMenu != null)
        {
            objetoMusicaMenu.SetActive(false); 
        }

        StartCoroutine(SecuenciaVideoIntro());
    }

    private IEnumerator SecuenciaVideoIntro()
    {
        if (reproductorVideo != null)
        {
            if (!reproductorVideo.isPrepared)
            {
                reproductorVideo.Prepare();
                while (!reproductorVideo.isPrepared)
                {
                    yield return null; 
                }
            }

            if (pantallaImagen != null)
            {
                pantallaImagen.enabled = true;
            }
            
            reproductorVideo.Play();                     

            yield return new WaitForSeconds(0.1f);

            while (reproductorVideo.isPlaying)
            {
                yield return null; 
            }
        }

        SceneManager.LoadScene(nombreEscenaCasa);
    }

    public void AbrirAjustes() { Debug.Log("Ajustes..."); }
    public void SalirDelJuego() { Application.Quit(); }
}