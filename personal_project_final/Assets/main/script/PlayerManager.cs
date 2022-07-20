using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
 
    [SerializeField]
    private int hp;
    [SerializeField]
    private int wave;

    public Image blackOut;
    public Image redScreen;
    CharacterController characterController;


    private GameObject[] revivePoints= new GameObject[3];
    private GameObject revivePoint1;
    private GameObject revivePoint2;
    private GameObject revivePoint3;
    bool isAlive;

    public AudioSource audioSource;
    


    public int Hp
    {
        get { return hp; }
        set
        {
            Debug.Log("hp : " + value);
            hp = value;
            if (hp <= 0)
            {
                hp = 0;
                PlayerDie();

            }
            else if (hp > 200)
                hp = 200;

            if (hp <= 60 )
                redScreen.gameObject.SetActive(true);

            else
                redScreen.gameObject.SetActive(false);


        }
    }
    private void Awake()
    {
        
        wave = 1;

        revivePoint1 = GameObject.FindGameObjectWithTag("RevivePoint1");
        revivePoint2 = GameObject.FindGameObjectWithTag("RevivePoint2");
        revivePoint3 = GameObject.FindGameObjectWithTag("RevivePoint3");
        revivePoints[0] = revivePoint1;
        revivePoints[1] = revivePoint2;
        revivePoints[2] = revivePoint3;

        //audioSource = GetComponent<AudioSource>();


        characterController = GetComponent<CharacterController>();

        isAlive = true;

    }
    private void Update()
    {
       
        



    }
   
 
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Floor"))
        {
            PlayerDie();
        }
        else if (other.gameObject.name == "waveCollider1")
        {
            wave = 1;
        }
        else if(other.gameObject.name =="waveCollider2")
        {
            wave = 2;
        }
        else if (other.gameObject.name == "waveCollider3")
        {
            wave = 3;
        }
     
        //씬 넘어가면 웨이브 1로 초기화

    }

    void PlayerDie()
    {
        if (isAlive)
        {
            isAlive = false;
            
            StartCoroutine(BlackOut());
            Invoke("Revive", 1.5f);   //2초 뒤 부활
        }
    }

    IEnumerator BlackOut()  //페이드인
    {
        blackOut.gameObject.SetActive(true);
        Color color = blackOut.color;
        while(color.a<1f)
        {
            color.a += Time.deltaTime / 1.5f; //1초동안 실행
            blackOut.color = color;
            yield return null;
        }
        GameManager.Instance.Life--;
        yield return new WaitForSeconds(1.5f);
        color.a = 0;
        blackOut.color = color;
        blackOut.gameObject.SetActive(false);
        
        
    }
    void Revive()
    {
        characterController.enabled = false;
        Hp = 200;

        transform.position = revivePoints[wave-1].transform.position;
        characterController.enabled = true;

        isAlive = true;

    }
    public void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    
}