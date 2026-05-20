using UnityEngine;
using System.Collections;

public class gestorInventario : MonoBehaviour
{
    public CanvasGroup iconoLlaveCG; 
    public float velocidadFade = 2f;

    void Start()
    {
        if (ControladorGlobal.tieneLlave)
        {
            StartCoroutine(AparecerIcono());
        }
        else
        {
            iconoLlaveCG.alpha = 0;
        }
    }

    IEnumerator AparecerIcono()
    {
        while (iconoLlaveCG.alpha < 1)
        {
            iconoLlaveCG.alpha += Time.deltaTime * velocidadFade;
            yield return null; // Esperamos al siguiente frame
        }
    }
}