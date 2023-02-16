using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject Part1;
    public GameObject Part2;

    public Vector2 Open1 = new Vector2(-0.24f, 0);
    public Vector2 Open2 = new Vector2(0.24f, 0);
    public Vector2 Close1 = new Vector2(-0.08f, 0);
    public Vector2 Close2 = new Vector2(0.08f, 0);

    public bool IsOpen = false;
    public float Duration = 0.5f;

    private float timeRemaining = 0;

    public void Toggle()
    {
        this.Toggle(!IsOpen);
    }

    public void Toggle(bool open)
    {
        IsOpen = open;
        timeRemaining = Duration;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            var t = timeRemaining / Duration;
            if (!IsOpen) t = 1 - t;

            Part1.transform.localPosition = Vector2.Lerp(Open1, Close1, t);
            Part2.transform.localPosition = Vector2.Lerp(Open2, Close2, t);
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
                timeRemaining = 0;
        }
        else if (IsOpen)
        {
            Part1.transform.localPosition = Open1;
            Part2.transform.localPosition = Open2;
        }
        else
        {
            Part1.transform.localPosition = Close1;
            Part2.transform.localPosition = Close2;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = IsOpen;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = IsOpen;
    }
}
