using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    public float Speed;
    public float ShootPerSecond;

    private Enemy enemy;
    private Animator anim;
    private Rigidbody2D body;
    private WeaponWield wield;
    private Possessable possessable;

    private Vector2 facing = new Vector2(0, 1);
    private bool isWalking;
    private bool isDead;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        wield = GetComponent<WeaponWield>();
        possessable = GetComponent<Possessable>();

        enemy.Died += () =>
        {
            anim.SetBool("IsDead", true);
            isDead = true;

            wield.SetVisible(false);
        };
    }

    void Update()
    {
        if (isDead)
            return;
        if (!possessable.IsPossessed)
        {
            if (enemy.Target != null)
            {
                var delta = transform.position - enemy.Target.Current.transform.position;
                var error = 1 - delta.magnitude;
                var range = body.velocity.magnitude == 0 ? .2 : .05;

                isWalking = true;
                body.velocity = delta.normalized * -Speed;

                wield.Weapon.Friendly = false;
                var gun = wield.Weapon as Gun;
                if (gun != null)
                {
                    gun.ShootAt(enemy.Target.Current.transform.position);
                    gun.Shooting = Time.time % (1 / ShootPerSecond) < .04f;
                }
            }
            else
            {
                isWalking = false;
                body.velocity = Vector2.zero;
            }
        }

        if (body.velocity.magnitude > .05)
            facing = body.velocity;

        anim.SetFloat("Angle", Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg);
        anim.SetBool("IsWalking", isWalking);
    }
}
