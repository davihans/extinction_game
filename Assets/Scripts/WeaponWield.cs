using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWield : MonoBehaviour
{
    public Weapon WeaponTemplate;
    public Weapon Weapon { get; private set; }

    public void SetVisible(bool visible)
    {
        print("disable");
        Weapon.gameObject.SetActive(visible);
    }

    // Use this for initialization
    void Start()
    {
        Weapon = Instantiate(WeaponTemplate);
        Weapon.Init(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
            print(gameObject + " " + gameObject.activeSelf);
    }
}
