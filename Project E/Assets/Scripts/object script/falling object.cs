using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falling_object : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float gravityScale;
    [SerializeField] private float waittime;
    [SerializeField] private LayerMask player;
    [SerializeField] private Transform rayCasts;
    [SerializeField] private float raylenght;
    //private Movement PlayerScript;
    private void Start()
    {
        //PlayerScript = GameObject.FindWithTag("Player").GetComponent<Movement>();

    }
   
    private void FixedUpdate()
    {  
        RaycastHit2D hit = Physics2D.BoxCast(rayCasts.position,new Vector2(transform.localScale.x +3,transform.localScale.y+raylenght),0f,Vector2.down,raylenght,player);
       
       
        if (hit.collider != null/*||hit1.collider != null*/)
        {
            StartCoroutine(fall());

            
        }
    }
    private IEnumerator fall()
    {
        yield return new WaitForSecondsRealtime(waittime);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScale; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.CompareTag("Player"))
        //     PlayerScript.Respawn();

    }
    private void OnDrawGizmos()
    {

        Gizmos.DrawCube(new Vector3(rayCasts.position.x,rayCasts.position.y-raylenght,rayCasts.position.z), new Vector2(transform.localScale.x +3, transform.localScale.y + raylenght));
    }
}
