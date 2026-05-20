using UnityEngine;

public class MensajePuerta : MonoBehaviour
{
    public float tiempoVisible = 3f;

    
    public void LanzarMensaje()
    {
        
        GameManager.Instance.MostrarImagenMensaje(this.gameObject, tiempoVisible);
    }
}