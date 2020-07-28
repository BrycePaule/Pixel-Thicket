using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gateway : MonoBehaviour
{

    [SerializeField] public direction Direction;

    public enum direction 
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 12)
        {
            print(Direction);
        }
    }
}
