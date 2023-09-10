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
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 50f,player);
        if (hit.collider != null)
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
}
