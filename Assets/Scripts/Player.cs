using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float Speed;
    public float AttackSpeed;
    public float LaunchTime;
    public float AttackDelay;
    public float superMultiply;
    public float chargeTime;

    public float MaxHealth;
    public float Health;

    public PossessionManager Possession;

    private Animator anim;
    private Rigidbody2D rb;
    private WeaponWield wield;
    private Vector3 facing = new Vector3(1, 0, 0);

    private bool charging = false;
    private bool attacking = false;

    private bool canAttack = true;
    private bool canPossess = false;
    private bool superLaunch = false;

    public void TakeDamage()
    {
        if (Health <= 0) return;

        Health -= 25;
        if (Health <= 0)
        {
            Health = 0;
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        wield = GetComponent<WeaponWield>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        wield.Weapon.Friendly = true;
        Health = MaxHealth;
    }

    void Update()
    {
        if (Input.GetButton("Quit"))
        {
            Application.Quit();
        }

        var gun = wield.Weapon as Gun;
        var inputGun = new Vector2(Input.GetAxis("AimH"), Input.GetAxis("AimV"));
        var inputMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (gun != null)
        {
            gun.Shooting = Input.GetButton("Shoot");
            if (inputGun.magnitude > .05)
                gun.Direction = inputGun;
        }
        print(Input.GetButton("Shoot"));
        print(gun.Shooting);

        //MOVEMENT
        if (charging)
            rb.velocity = Vector2.zero;
        else if (!attacking)
            rb.velocity = inputMove * Speed;

        if (inputMove.magnitude > .1)
            facing = inputMove.normalized;

        if (rb.velocity.magnitude > .1)
        {
            anim.SetFloat("walking", 1);
        }
        else
            anim.SetFloat("walking", 0);

        transform.rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), facing);

        if (Input.GetButtonDown("Fire"))
        {
            if (canAttack && !attacking)
            {
                anim.SetBool("charging", true);
                charging = true;
                attacking = true;
                StartCoroutine(Attack());
                StartCoroutine(Charge());
            }

        }
        else if (Input.GetButtonUp("Fire"))
        {
            charging = false;

            if (attacking && !superLaunch)
            {
                StartCoroutine(Launch());
            }
            else if (attacking && superLaunch)
            {

                StartCoroutine(SuperLaunch());

            }
        }
    }


    IEnumerator Attack()
    {
        canAttack = false;
        yield return new WaitForSeconds(AttackDelay);
        canAttack = true;
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(chargeTime);
        if (attacking)
        {
            anim.SetBool("super", true);
            superLaunch = true;
        }
    }

    IEnumerator Launch()
    {
        anim.SetBool("leaping", true);
        canPossess = true;
        Vector2 direction2 = new Vector2(facing.x, facing.y);
        direction2.Normalize();
        direction2.x = direction2.x * AttackSpeed;
        direction2.y = direction2.y * AttackSpeed;
        rb.velocity = direction2;
        yield return new WaitForSeconds(LaunchTime);
        attacking = false;
        canPossess = false;
        anim.SetBool("charging", false);
        anim.SetBool("leaping", false);
    }


    IEnumerator SuperLaunch()
    {
        anim.SetBool("leaping", true);
        canPossess = true;
        Vector2 direction2 = new Vector2(facing.x, facing.y);
        direction2.Normalize();
        direction2.x = direction2.x * AttackSpeed * superMultiply;
        direction2.y = direction2.y * AttackSpeed * superMultiply;
        rb.velocity = direction2;
        yield return new WaitForSeconds(LaunchTime * 1.2f);
        attacking = false;
        canPossess = false;
        superLaunch = false;
        anim.SetBool("charging", false);
        anim.SetBool("super", false);
        anim.SetBool("leaping", false);
    }

    public void UnPossess(Vector2 position)
    {
        wield.SetVisible(true);
        gameObject.SetActive(true);
        transform.position = position;
        canAttack = true;
        attacking = false;
        canPossess = false;
        anim.SetBool("charging", false);
        anim.SetBool("leaping", false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canPossess)
        {
            var possessable = collision.gameObject.GetComponent<Possessable>();
            if (possessable != null && possessable.IsPossessable)
            {
                Possession.Possess(possessable);
                wield.SetVisible(false);
                gameObject.SetActive(false);
            }
        }
    }
}
