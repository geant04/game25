using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int speed;

    public void TakeDamage(int amt)
    {
        health = Mathf.Max(0, health - amt);
        if (health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Debug.Log("God I am dead");
        Destroy(gameObject);
    }
}
