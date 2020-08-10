using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalContainer : MonoBehaviour
{
    private static GlobalContainer _instance;

    public Transform SpellContainer;


    public static GlobalContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalContainer>();
                if (_instance == null)
                {
                    _instance = new GlobalContainer();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null) { Destroy(this.gameObject); }
    }

    public void ClearAllSpells()
    {
        foreach (Spell spell in SpellContainer.GetComponentsInChildren<Spell>())
        {
            Destroy(spell.gameObject);
        }
    }

}
