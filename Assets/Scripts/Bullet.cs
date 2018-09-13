using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public string damageObjectTag = "Enemy";
    public float speed = 2f;
    [HideInInspector]
    public float damage;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains(damageObjectTag))
        {
            collision.GetComponent<Damageable>().ReceiveDamage(damage);
            Destroy(gameObject);
            
        }
    }
    void Update () {
        rb.velocity = Vector2.right * speed;
	}
}
