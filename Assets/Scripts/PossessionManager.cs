using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionManager : MonoBehaviour
{
    public Player Player;
    public GameObject Current;

    public void Possess(Possessable target)
    {
        Current = target.gameObject;
        target.Possess();
        target.GetComponent<Enemy>().Died += () =>
        {
            Current = Player.gameObject;
            Player.UnPossess(target.transform.position);
        };
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
