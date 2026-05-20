using UnityEngine;
using UnityEngine.EventSystems;

// Usamos IPointerClickHandler para detectar el clic en la UI de forma limpia
public class TrozoCristalUI : MonoBehaviour, IPointerClickHandler
{
    private Rigidbody2D rb;
    private bool yaSeCayo = false;
    private RomperCristal gestorCristal;

    void Start()
    {
        // Buscamos o añadimos el Rigidbody2D en el trozo de cristal
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        // Al principio el cristal está fijo en la ventana
        rb.bodyType = RigidbodyType2D.Kinematic; 

        // Buscamos el script padre que controla todo el minijuego
        gestorCristal = GetComponentInParent<RomperCristal>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si ya le hicimos clic, no hacemos nada
        if (yaSeCayo) return;

        yaSeCayo = true;

        // 1. Convertimos el cristal en físico para que la gravedad de Unity lo haga caer
        rb.bodyType = RigidbodyType2D.Dynamic;

        // 2. Le damos un pequeño empujón aleatorio y un toque de rotación para que caiga de forma natural
        float empujeX = Random.Range(-50f, 50f);
        float fuerzaCaida = Random.Range(-10f, -30f); // Empuje leve hacia abajo antes de que actúe la gravedad
        rb.linearVelocity = new Vector2(empujeX, fuerzaCaida);
        rb.angularVelocity = Random.Range(-100f, 100f);

        // 3. Le quitamos el "Raycast Target" para que el jugador no pueda volver a clicarlo en el aire
        if (TryGetComponent<UnityEngine.UI.Image>(out var img))
        {
            img.raycastTarget = false;
        }

        // 4. Avisamos al script principal de que quitamos un cristal
        if (gestorCristal != null)
        {
            gestorCristal.QuitarTrozoDeLaCuenta();
        }

        // 5. Destruimos el objeto tras 2 segundos para no llenar la memoria de trozos caídos
        Destroy(gameObject, 2f);
    }
}