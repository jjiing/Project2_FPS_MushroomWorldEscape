using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster22_Burrow : Monster
{
    public GameObject target;
 

    float attackRate;
    public float currentAttackRate;
    int atkDis;
    int detectDis;

    public Animator anim;
    Rigidbody rigidbody;
    NavMeshAgent nav;

    //---°ø°Ý-----
   

    public GameObject attack2Effect;
    public Transform attack2FirePoint;
    public Transform attack2FirePoint2;
    public Transform attack2FirePoint3;

    public GameObject attack3Effect;

    

    public GameObject prefab5;
    

    Vector3 targetPos;
    public GameObject warnCircle;
    public GameObject collider_;
    





    void Start()
    {
        Hp = 1000;
        //Atk = 30;
        atkDis = 100;
        detectDis = 150;

        nav = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        attackRate = 2.5f;
        currentAttackRate = 0;
        nav.speed = 60;
        nav.stoppingDistance = atkDis;

        monsterCount++;

    }

    // Update is called once per frame
    void Update()
    {
      
        Detect();
        DamageAni();
   
    }


    private void Detect()
    {
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis <= detectDis)
        {
            transform.LookAt(target.transform.position - new Vector3(0, 10, 0));

            if (atkDis <= dis)
            {
                MoveToPlayer();
                anim.SetBool("isMove", true);
                
            }
            else if (dis < atkDis && !isDead )
            {
                //¹Ì²ô·¯Áü - º¸·ù
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                nav.velocity = Vector3.zero;

                if (currentAttackRate > 0)
                {
                    currentAttackRate -= Time.deltaTime;
                    
                }

                else if (currentAttackRate <= 0)
                {

                    currentAttackRate = attackRate;
                    int attackNum = Random.Range(1, 4);

                    if (attackNum == 1) AttackPlayer1();
                    else if (attackNum == 2) AttackPlayer2();
                    else if (attackNum == 3) AttackPlayer3();

                    

                    anim.SetBool("isMove", false);
                }
            }
        }
        else if (IsDamaged)
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



    private void AttackPlayer1()
    {
        targetPos = target.transform.position - new Vector3(0, 11, 0);
        var circle = Instantiate(warnCircle, new Vector3(targetPos.x, transform.position.y, targetPos.z+20), Quaternion.Euler(90, 0, 0));
        StartCoroutine(Attack1Co());
        Destroy(circle, 1f);
    }
    IEnumerator Attack1Co()
    {
        yield return new WaitForSeconds(1f);
        
        GameObject water = Instantiate(prefab5, new Vector3(targetPos.x, transform.position.y +0.5f , targetPos.z+20), Quaternion.identity);
        GameObject col = Instantiate(collider_, new Vector3(targetPos.x, transform.position.y + 0.5f, targetPos.z+20), Quaternion.identity);

        Destroy(water, 2f);
        Destroy(col, 2f);
    }
    private void AttackPlayer2()
    {

        anim.SetTrigger("isAttack2");
        Invoke("Attack2Play", 1.3f);

    }
    private void Attack2Play()
    {
        GameObject attack2_0 = Instantiate(attack2Effect, attack2FirePoint.position, attack2FirePoint.rotation);
        GameObject attack2_1 = Instantiate(attack2Effect, attack2FirePoint2.position, attack2FirePoint2.rotation);
        GameObject attack2_2 = Instantiate(attack2Effect, attack2FirePoint3.position, attack2FirePoint3.rotation);
        Rigidbody attack2Rigid = attack2_0.GetComponent<Rigidbody>();
        Rigidbody attack2Rigid1 = attack2_1.GetComponent<Rigidbody>();
        Rigidbody attack2Rigid2 = attack2_2.GetComponent<Rigidbody>();
        attack2Rigid.velocity = transform.forward * 170f;
        attack2Rigid1.velocity = attack2FirePoint2.transform.forward * 170f;
        attack2Rigid2.velocity = attack2FirePoint3.transform.forward * 170f;
    }
    private void AttackPlayer3()
    {
        anim.SetTrigger("isAttack2");
        Invoke("Attack3Play", 1.3f);
    }
    private void Attack3Play()
    {
        GameObject attack3 = Instantiate(attack3Effect, attack2FirePoint.position, attack2FirePoint.rotation);
        Rigidbody attack3Rigid = attack3.GetComponent<Rigidbody>();
        attack3Rigid.velocity = transform.forward * 250f;
    }

    
    
    
    public override void MonsterDie()
    {
        anim.SetTrigger("isDie");
        Destroy(gameObject, 2.4f); //Á×´Â ¾Ö´Ï¸ÞÀÌ¼Ç ÈÄ ÆÄ±«
        
        nav.speed = 0;
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
