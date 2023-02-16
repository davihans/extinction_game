using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool Friendly { get; set; }
    public GameObject Parent { get; private set; }

    public void Init(GameObject parent)
    {
        Parent = parent;
        gameObject.name = parent.name + " " + GetType().Name;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
