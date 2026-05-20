using UnityEngine;
using UnityEngine.InputSystem;
public class movPersonajeMinijuego : MonoBehaviour
{




 public float velocidad = 0.5f;

 public float impulsoSalto= 10.0f;

 private bool puedoSaltar= true;
  Animator controlAnimacion;

  Rigidbody2D rb;

  GameObject respawn;
 




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
     controlAnimacion = GetComponent<Animator>();
     rb = GetComponent<Rigidbody2D>();
    respawn = GameObject.Find("respawn");
    Respawnear();

    }




    // Update is called once per frame
    void Update()
    {
       
      Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();
     
      this.transform.Translate(moveInput.x * velocidad, moveInput.y * velocidad, 0);
     
      if(moveInput.x < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        } else if (moveInput.x > 0)
        {
           this.GetComponent<SpriteRenderer>().flipX = false;

        }

     if (moveInput.x != 0)
        {
            controlAnimacion.SetBool("activaCamina", true);
        }
        else
        {
            controlAnimacion.SetBool("activaCamina", false);
        }

     //SALTO


         bool salto = InputSystem.actions["Jump"]. WasPressedThisFrame( );
         Debug.Log(salto);
    
    if(puedoSaltar && salto)
         {
            Debug.Log("Salto");
            rb.AddForce(transform.up * impulsoSalto, ForceMode2D.Impulse);
            puedoSaltar = false;
           
         }
     
     
     /*RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * 0.5f);

     if(hit.collider == true)
        {
            puedoSaltar = true;
        }
        else
        {
            puedoSaltar = false;
     
        }*/

 }

       void OnCollisionEnter2D(Collision2D collision)
     {
        puedoSaltar = true;
     }

 public void Respawnear()
    {
        transform.position = respawn.transform.position;



    }


}
