using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField]
    private int hp;
    private int atk;
    public bool isDamaged;
    public bool isDead;
    public GameObject healDrop;
    [SerializeField]
    private bool isMode2;

    public bool IsMode2
    {
        get { return isMode2; }
        set { 
            Debug.Log("isMode 2 : "+ value);
            isMode2 = value; }
    }


    public static int monsterCount;

    public int Hp
    {
        get { return hp; }
        set 
        { hp = value;
            if (hp <= 0 && !isDead)
            {
                hp = 0;
                MonsterDie();
                monsterCount--;
            }
        }
    }
    public int Atk
    {
        get { return atk; }
        set { atk = value; }
    }
    public bool IsDead
    {
        get { return isDead;}
        set { isDead = value;}
    }
    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }

    private void Start()
    {
        isDamaged = false;
        isDead = false;
        monsterCount = 0;
        isMode2 = false;
    }

    public virtual void MonsterDie() { }
    public virtual void AttackPlayer() { }
    
    public void DropHeal()
    {
        if (!isMode2)
        {
            int random = Random.Range(1, 6);
            if (random == 1)
            {
                Instantiate(healDrop, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
            }
        }
    }
    

  
   

    


}
