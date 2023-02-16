using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PossessionManager Target;
    public event Action Died;

    public float MaxHealth;
    public float Health;

    public int healthBarLocation;
    public int healthBarLength;

    private new SpriteRenderer renderer;
    private float damageAnim;

    public bool IsDead
    {
        get {
            return Health <= 0;
        }
    }

    public void TakeDamage(float amount)
    {
        if (Health <= 0) return;

        Health -= amount;
        damageAnim = 1f;
        if (Health <= 0)
        {
            Health = 0;
            GetComponent<Rigidbody2D>().simulated = false;
            if (Died != null) Died.Invoke();
        }
    }

    // Use this for initialization
    void Start()
    {
        Health = MaxHealth;
        renderer = GetComponent<SpriteRenderer>();
    }

    void OnGUI()
    {
        if (Health <= 0) return;

        var background = new Texture2D(1, 1);
        background.SetPixel(0, 0, Color.gray);
        background.Apply();

        var foreground = new Texture2D(1, 1);
        foreground.SetPixel(0, 0, Color.red);
        foreground.Apply();

        Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);

        var bar = new Rect(targetPos.x, Screen.height - targetPos.y - healthBarLocation, 60 * healthBarLength, 8);
        bar.x -= bar.width / 2;
        bar.y -= 50;

        GUI.DrawTexture(bar, background);
        bar.width *= Health / MaxHealth;
        GUI.DrawTexture(bar, foreground);
    }

    // Update is called once per frame
    void Update()
    {
        if (damageAnim > 0)
        {
            damageAnim -= Time.deltaTime;
            renderer.color = Color.Lerp(Color.white, Color.red, damageAnim / 2);
        }
        else
        {
            renderer.color = Color.white;
        }
    }
}
