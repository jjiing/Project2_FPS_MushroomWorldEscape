using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster13_red : Monster
{
    public GameObject target;

    
    float attackRate;
    float currentAttackRate;
    int atkDis;
    int detectDis;
    int explosionDamage;

    Rigidbody rigidbody;
    NavMeshAgent nav;
    public Animator anim;
    public GameObject dieEffect;
    public GameObject body;


    void Start()
    {
        Hp = 120;
        Atk = 10;
        atkDis = 30;
        detectDis = 100;
        explosionDamage = 30;

        nav = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        attackRate = 2f;
        currentAttackRate = 0;
        nav.speed = 120;
        nav.stoppingDistance = atkDis;
        monsterCount++;
    }


    void Update()
    {
        Detect();
        StartCoroutine(DamageAni());
    }

    private void Detect()
    {
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis <= detectDis)
        {
            transform.LookAt(target.transform.position);
            if (atkDis <= dis)
            {
                MoveToPlayer();
                anim.SetBool("isMove", true);

            }
            else if (dis < atkDis && !isDead && !isDamaged)
            {
                //¹Ì²ô·¯Áü - º¸·ù
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                nav.velocity = Vector3.zero;

                AttackPlayer();
                anim.SetBool("isMove", false);
            }
        }
        else if(IsDamaged)
        {
            transform.LookAt(target.transform.position);
            MoveToPlayer();
            anim.SetBool("isMove", true);
        }
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
            target.GetComponent<PlayerManager>().Hp -= Atk;

            
            
        }

    }

    public override void MonsterDie()
    {
        anim.SetTrigger("isDie");
        Destroy(gameObject, 1.8f); //Á×´Â ¾Ö´Ï¸ÞÀÌ¼Ç ÈÄ ÆÄ±«
        Invoke("DieEffectOn", 1.5f);
        Invoke("DieOff", 1.5f);

        isDead = true;
        DropHeal();


    }
    private void DieOff()
    {
        body.SetActive(false);
    }
    private void DieEffectOn()
    {
        //Æø¹ß »ç¿îµå ³Ö±â
        dieEffect.SetActive(true);
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis <= 35)
            target.GetComponent<PlayerManager>().Hp -= explosionDamage; //Æø¹ßµ¥¹ÌÁö
    }

    IEnumerator DamageAni()
    {
        if (isDamaged)
        {
            anim.SetTrigger("isDamaged");
            yield return new WaitForSeconds(0.2f);
            isDamaged = false;
        }
    }
}
