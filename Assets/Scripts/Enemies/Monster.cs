using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public enum Element {
        WATER,
        FIRE,
        GRASS
    };
    [SerializeField] public Element element;

    public void disableObject()
    {
        gameObject.SetActive(false);
    }

    public abstract void startMonster();
    public abstract void dealDamage();
}
