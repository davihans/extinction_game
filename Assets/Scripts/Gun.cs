using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Bullet BulletTemplate;

    public Vector2 Direction { get; set; }
    public bool Shooting { get; set; }

    private bool canShoot = true;

    public void ShootAt(Vector2 target)
    {
        Direction = target - new Vector2(transform.position.x, transform.position.y);
    }

    // Use this for initialization
    private void Start()
    {
        Direction = new Vector2(1, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (canShoot && Shooting)
            StartCoroutine(Shoot());

        transform.position = Parent.transform.position + new Vector3(0.1f, 0);

        if (Direction.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, Mathf.Atan2(Direction.y, -Direction.x) * 180 / Mathf.PI);
        else
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Direction.y, Direction.x) * 180 / Mathf.PI);
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        var rotate = Quaternion.FromToRotation(new Vector2(1, 0), Direction);
        var shot = Instantiate(BulletTemplate, transform.position, rotate);
        shot.Init(Direction, Friendly);

        yield return new WaitForSeconds(.4f);
        canShoot = true;
    }
}
