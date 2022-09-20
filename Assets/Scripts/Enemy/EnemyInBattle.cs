using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInBattle : MonoBehaviour
{
    public BattleData data;
    public GameObject healthBar;

    public void Damage(int amount, GameObject attacker)
    {
        Debug.Log("hp: " + data.hp + ", damage: " + amount);

        if (Random.Range(0, 10) < 3) {
            amount *= 5;
        }

        data.hp -= amount;

        Debug.Log("maxHp: " + data.maxHp + ", data.hp: " + data.hp + ", percentage: " + data.hp / data.maxHp);

        healthBar.transform.localScale = new Vector3(
            (float)data.hp / (float)data.maxHp,
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.z);
    }
}
