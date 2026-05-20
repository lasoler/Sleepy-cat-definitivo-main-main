using UnityEngine;

public class MostrarImagen : MonoBehaviour
{
    public GameObject imagen;
    public float tiempoEspera = 2f;  
    public float segundos = 10f;    

    void Start()
    {
        
        if (ControladorGlobal.mensajeInicialMostrado == true)
        {
            imagen.SetActive(false);
            return; 
        }
       
        imagen.SetActive(false);
     
    
        Invoke("MostrarConFade", tiempoEspera);

        ControladorGlobal.mensajeInicialMostrado = true;
    }

    void MostrarConFade()
    {
        if (imagen != null)
        {
           
            GameManager.Instance.MostrarImagenMensaje(imagen, segundos);
        }
    }
}