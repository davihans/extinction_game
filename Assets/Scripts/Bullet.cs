using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
            enemy.TakeDamage(Damage);

        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
            player.TakeDamage();
    }

    public void Init(Vector2 direction, bool friendly)
    {
        gameObject.layer = friendly ? 11 : 9;
        var body = GetComponent<Rigidbody2D>();
        body.velocity = direction.normalized * 1.2f;
    }
}
