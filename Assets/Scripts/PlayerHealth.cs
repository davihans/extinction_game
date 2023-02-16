using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Player Player;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        var background = new Texture2D(1, 1);
        background.SetPixel(0, 0, Color.gray);
        background.Apply();

        var foreground = new Texture2D(1, 1);
        foreground.SetPixel(0, 0, Color.green);
        foreground.Apply();

        var bar = new Rect(10, 10, 200, 10);

        GUI.DrawTexture(bar, background);
        bar.width *= Player.Health / Player.MaxHealth;
        GUI.DrawTexture(bar, foreground);
    }
}
