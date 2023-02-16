using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Possessable : MonoBehaviour
{
    public float DecayRate = .1f;

    public bool IsPossessed { get; private set; }

    private Rigidbody2D body;
    private Enemy enemy;
    private Animator anim;
    private WeaponWield wield;

    public void Possess()
    {
        IsPossessed = true;
        gameObject.layer = 12;
        enemy.Health = enemy.MaxHealth;
    }

    public bool IsPossessable
    {
        get {
            return enemy.Health <= 25;
        }
    }

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        wield = GetComponent<WeaponWield>();
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPossessed) return;

        body.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (body.velocity.x == 0 && body.velocity.y == 0)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);

        var input = new Vector2(Input.GetAxis("AimH"), Input.GetAxis("AimV"));

        if (Input.GetButtonDown("Fire"))
            enemy.TakeDamage(enemy.Health);

        enemy.TakeDamage(DecayRate);

        var gun = wield.Weapon as Gun;
        if (gun != null)
        {
            if (input.magnitude > .05)
                gun.Direction = input;
            gun.Friendly = true;
            gun.Shooting = Input.GetButton("Shoot");
        }
    }
}
