using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeboss : MonoBehaviour
{
    public float Speed;
    public float ShootSpeed;
    public bool Friendly = false;

    private Enemy enemy;
    private Animator anim;
    private Rigidbody2D body;

    private bool isWalking;
    private bool isDead;
    private bool canShoot = true;

    public Bullet BulletTemplate;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();

        enemy.Died += () =>
        {
            anim.SetBool("IsDead", true);
            isDead = true;
        };
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        var deltaMid = enemy.Target.Current.transform.position - transform.position;

        var rotateMid = Quaternion.FromToRotation(new Vector2(1, 0), deltaMid);
        var rotateLeft = rotateMid *Quaternion.Euler(30, 0, 0);
        var rotateRight = rotateMid * Quaternion.Euler(-30, 0, 0);

        var deltaLeft = Quaternion.AngleAxis(30, Vector3.forward) * deltaMid;
        var deltaRight = Quaternion.AngleAxis(-30, Vector3.forward) * deltaMid;  

        var shotM = Instantiate(BulletTemplate, transform.position, rotateMid);
        var shotL = Instantiate(BulletTemplate, transform.position, rotateLeft);
        var shotR = Instantiate(BulletTemplate, transform.position, rotateRight);

        shotM.Init(deltaMid, Friendly);
        shotL.Init(deltaLeft, Friendly);
        shotR.Init(deltaRight, Friendly);

        yield return new WaitForSeconds(.4f * ShootSpeed);
        canShoot = true;
    }

    void Update()
    {
        if (isDead)
            return;

        if (enemy.Target != null)
        {
            if (canShoot)
                StartCoroutine(Shoot());

            var delta = transform.position - enemy.Target.Current.transform.position;
            var error = 1 - delta.magnitude;
            var range = body.velocity.magnitude == 0 ? .2 : .05;
            body.velocity = delta.normalized * -Speed;
        }




        //Gun.ShootAt(enemy.Target.Current.transform.position);
        //Gun.Shooting = Time.time % (1 / ShootPerSecond) < .04f;
        else
        {
            body.velocity = Vector2.zero;
        }
    }
}
