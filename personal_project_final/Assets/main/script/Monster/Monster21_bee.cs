using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster21_bee : Monster
{
    public GameObject target;


    float attackRate;
    public float currentAttackRate;
    int atkDis;
    int detectDis;
    bool isIdle = true;

    Rigidbody rigidbody;
    NavMeshAgent nav;
    public Animator anim;


    Vector3 currentPos;
    float range = 5f;
    float speed = 2f;

    public GameObject beeAttack;
    public GameObject beeTail;



    void Start()
    {
        Hp = 500;
        Atk = 25;
        atkDis = 120;
        detectDis = 180;
 
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        attackRate = 2f;
        currentAttackRate = 0;
        nav.speed = 200;
        nav.stoppingDistance = atkDis;

        currentPos = transform.position;

        monsterCount++;
    }




    void Update()
    {
        if (isIdle)
            BasicMove();
        Detect();
        DamageAni();
    }
    private void BasicMove()
    {
        Vector3 v = currentPos;
        v.y += range * Mathf.Sin(Time.time * speed);
        transform.position = v;



    }
    private void Detect()
    {
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis <= detectDis)
        {
            isIdle = false;
            transform.LookAt(target.transform.position- new Vector3(0,5,0));
            if (atkDis <= dis)
            {
                MoveToPlayer();
                anim.SetBool("isMove", true);

            }
            else if (dis < atkDis && !isDead)
            {
                //¹Ì²ô·¯Áü - º¸·ù
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                nav.velocity = Vector3.zero;

                AttackPlayer();
                anim.SetBool("isMove", false);
            }
        }
        else if (IsDamaged)
        {
            transform.LookAt(target.transform.position);
            MoveToPlayer();
            anim.SetBool("isMove", true);
        }
        else
            isIdle = true;
    }
    private void MoveToPlayer()
    {

        nav.SetDestination(target.transform.position);
    }

    public override void AttackPlayer()
    {

        if (currentAttackRate > 0)
            currentAttackRate -= Time.deltaTime;


        else if (currentAttackRate <= 0)
        {
            currentAttackRate = attackRate;
            anim.SetTrigger("isAttack");
            Invoke("fire", 0.6f);


        }

    }
    private void fire()
    {
        GameObject beeEffect = Instantiate(beeAttack, beeTail.transform.position, beeTail.transform.rotation);
        Rigidbody beeRigid = beeEffect.GetComponent<Rigidbody>();
        beeRigid.velocity = transform.forward * 170f;

    }

    public override void MonsterDie()
    {
        anim.SetTrigger("isDie");
        Destroy(gameObject, 1.8f); //Á×´Â ¾Ö´Ï¸ÞÀÌ¼Ç ÈÄ ÆÄ±«
        

        isDead = true;
        DropHeal();
    }


    private void DamageAni()
    {
        if (isDamaged)
        {
            anim.SetTrigger("isDamaged");
           
            isDamaged = false;
        }
    }

}
