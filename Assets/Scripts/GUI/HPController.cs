using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    private static HPController instance;
    public static HPController Instance { 
        get
        {
            if (!instance)
                Debug.LogError("There's no HP GUI Controller on Scene! Please, add one on Canvas.");
            return instance;
        }    
    }

    [SerializeField] private RawImage[] hpIcons;
    [SerializeField] private Texture hpEnabled;
    [SerializeField] private Texture hpDisabled;

    private int hp;
    public int HP {
        get => hp;
        set
        {
            hp = value;
            if (hp > hpIcons.Length)
                Debug.LogWarning("HP set is more that GUI icons count!");
            else
            {
                for (int i = 0; i < hp; i++)
                    hpIcons[i].texture = hpEnabled;
                for (int i = 0; i < hpIcons.Length - hp; i++)
                    hpIcons[hpIcons.Length - i - 1].texture = hpDisabled;

                // Feels like it's could be optimised... :|
            }
        }
    }

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
}
