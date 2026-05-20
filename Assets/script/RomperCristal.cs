using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // ¡IMPORTANTE! Necesario para controlar vídeos
using TMPro;

public class RomperCristal : MonoBehaviour
{
    [Header("Configuración del Tiempo")]
    public float tiempoLimite = 15f; 
    public TextMeshProUGUI textoTemporizador; 

    [Header("Sonidos")]
    public AudioClip sonidoCristalRomper;
    private AudioSource audioSource;

    [Header("Secuencia de Fin de Juego (Vídeo)")]
    public VideoPlayer videoPlayerFinal; // Arrastra aquí el objeto con el Video Player
    public string escenaSiguiente = "volver_a_intentar"; // Nombre de la escena final

    private int trozosRestantes;
    private bool juegoTerminado = false;

    void OnEnable()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        juegoTerminado = false;

        trozosRestantes = GetComponentsInChildren<TrozoCristalUI>().Length;
        Debug.Log("Minijuego Ventana iniciado. Trozos a quitar: " + trozosRestantes);

        // Nos aseguramos de que el vídeo esté apagado y preparado al empezar
        if (videoPlayerFinal != null)
        {
            videoPlayerFinal.gameObject.SetActive(false);
            // Nos suscribimos al evento de Unity que avisa cuando un vídeo termina
            videoPlayerFinal.loopPointReached += AlTerminarElVideo;
        }
    }

    void OnDisable()
    {
        // Buena práctica: nos desuscribimos del evento al apagar el script para evitar errores
        if (videoPlayerFinal != null)
        {
            videoPlayerFinal.loopPointReached -= AlTerminarElVideo;
        }
    }

    void Update()
    {
        if (juegoTerminado) return;

        if (tiempoLimite > 0)
        {
            tiempoLimite -= Time.deltaTime;
            
            if (textoTemporizador != null)
            {
                textoTemporizador.text = "Tiempo: " + tiempoLimite.ToString("F1") + "s";
                if (tiempoLimite <= 4f) textoTemporizador.color = Color.red;
            }
        }
        else
        {
            GameOverTiempo();
        }
    }

    public void QuitarTrozoDeLaCuenta()
    {
        if (juegoTerminado) return;

        if (sonidoCristalRomper != null)
        {
            audioSource.PlayOneShot(sonidoCristalRomper);
        }

        trozosRestantes--;
        Debug.Log("Cristal quitado. Quedan: " + trozosRestantes);

        if (trozosRestantes <= 0)
        {
            ActivarSecuenciaVideo();
        }
    }

    void ActivarSecuenciaVideo()
    {
        juegoTerminado = true;
        Debug.Log("¡Ventana rota! Reproduciendo vídeo final...");

        // Guardamos datos globales
        ControladorGlobal.tieneLlave = true;
        if (GameManager.Instance != null) GameManager.Instance.RegistrarVictoriaMinijuego();

        // 1. Apagamos los componentes visuales del minijuego para que no tapen el vídeo
        if (TryGetComponent<UnityEngine.UI.Image>(out var fondo)) fondo.enabled = false;
        if (textoTemporizador != null) textoTemporizador.gameObject.SetActive(false);

        // 2. Encendemos el objeto del vídeo y lo reproducimos
        if (videoPlayerFinal != null)
        {
            videoPlayerFinal.gameObject.SetActive(true);
            videoPlayerFinal.Play();
        }
        else
        {
            // Si olvidaste poner el vídeo, salta directo a la escena para que el juego no se quede colgado
            Debug.LogError("No se ha asignado el VideoPlayer. Saltando escena directamente.");
            SceneManager.LoadScene(escenaSiguiente);
        }
    }

    // Esta función se ejecuta AUTOMÁTICAMENTE cuando el vídeo llega al final
    void AlTerminarElVideo(VideoPlayer source)
    {
        Debug.Log("El vídeo ha terminado. Cambiando de escena...");
        SceneManager.LoadScene(escenaSiguiente);
    }

    void GameOverTiempo()
    {
        juegoTerminado = true;
        if (textoTemporizador != null) textoTemporizador.text = "¡TIEMPO AGOTADO!";
        Debug.Log("No lograste romper la ventana a tiempo.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}