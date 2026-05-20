using UnityEngine;

public class lanzarMensaje : MonoBehaviour
{
    public float tiempoVisible = 5f; 

    
    public void LanzarMensaje()
    {
       
        GameManager.Instance.MostrarImagenMensaje(this.gameObject, tiempoVisible);
    }
}