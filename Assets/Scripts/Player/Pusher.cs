using UnityEngine;
using System.Collections;
public class Pusher : MonoBehaviour
{
    public float force = 1f;
    public bool isForceField;
    [Header("NO NEED IF FORCE FIELD")]
    public float time = 0.1f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player"))
        {
            Vector3 dir = collision.transform.position - transform.position;
            dir = dir.normalized;

            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            rb.AddForce(dir * force);
            if (!isForceField)
            {
                StartCoroutine(StopExerting(rb));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Contains("Player") && isForceField)
        {
            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
    }
    IEnumerator StopExerting(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(time);
        rb.velocity = Vector2.zero;
    }
}
