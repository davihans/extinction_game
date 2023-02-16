using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public bool IsActive;
    public Door[] Doors;
    public Enemy[] Enemies;

    public PossessionManager Possession;

    public void SetActive(bool active)
    {
        IsActive = active;

        foreach (var enemy in Enemies)
        {
            if (!IsActive)
                enemy.Target = null;
            else
                enemy.Target = Possession;
        }

        foreach (var door in Doors)
            door.Toggle(!IsActive);
    }

    // Use this for initialization
    void Start()
    {
        SetActive(IsActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActive)
            return;

        if (Enemies.All(a => Possession.Current == a.gameObject|| a.IsDead))
            SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        var possess = collision.gameObject.GetComponent<Possessable>();

        if (player != null || (possess != null && possess.IsPossessed))
        {
            if (Enemies.All(a => Possession.Current == a.gameObject || a.IsDead))
                return;

            SetActive(true);
        }
    }
}
