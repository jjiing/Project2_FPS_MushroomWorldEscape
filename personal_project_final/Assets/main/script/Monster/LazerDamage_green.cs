using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerDamage_green : MonoBehaviour
{
    public Monster11_green monster;
    float attackRate;
    float currentAttackRate;

    void Awake()
    {
        attackRate = 1f;


    }
    private void OnEnable()
    {
        Invoke("ActiveCollider", 1f);
        Invoke("DeactiveCollider", 3f);

    }

    private void ActiveCollider()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    private void DeactiveCollider()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        currentAttackRate = 0;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //레이저 맞는 사운드
            if (currentAttackRate > 0)
                currentAttackRate -= Time.deltaTime;
            if (currentAttackRate <= 0)
            {
                other.GetComponent<PlayerManager>().Hp -= monster.Atk;
                currentAttackRate = attackRate;
            }

        }
    }
}
