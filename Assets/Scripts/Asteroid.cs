using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Damageable {
    public float damage = 5f;
    public float speed = 10f;
    public float knockbackForce = 10f;
    Rigidbody2D rb;
    GameController gameController;
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = standardHealth;
    }
    private void Update()
    {
        if(currentHealth <= 0)
        {
            gameController.AddDNA();
            Destroy(gameObject);
        }
        rb.velocity = Vector3.left * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Contains("Player"))
        {
            Vector3 dir = transform.position - other.transform.position;
            dir.Normalize();
            other.GetComponent<Damageable>().ReceiveDamage(damage);

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController.currentHealth <= 0)
                playerController.hpText.text = "HP: 0";
            else
                playerController.hpText.text = "HP: " + ((int)other.GetComponent<PlayerController>().currentHealth).ToString();


            Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();
            otherRB.AddForce(-dir * knockbackForce, ForceMode2D.Impulse);
            StartCoroutine(StopForce(otherRB));
        }
    }
    IEnumerator StopForce(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
    }
}
