using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Manager : MonoBehaviour
{
    
    public GameObject scene1;
    public GameObject scene2;


    public static bool arrivedAtPortal = false;
    public PlayerManager playerManager;


    

    void Awake()
    {

        int sNum = Random.Range(1, 3);
        if (sNum == 1) scene2.SetActive(true);
        else if (sNum == 2) scene1.SetActive(true);
        playerManager.Hp = 200;
     
       
    }
    private void Start()
    {
        GameManager.Instance.Life = 3;
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.isClear = false;
        GameManager.Instance.isMonsterLeft = false;
    }


    void Update()
    {
        Stage1Clear();
       
    }

    void Stage1Clear()
    {
        if(Monster.monsterCount==0 && arrivedAtPortal) 
        {
            arrivedAtPortal=false;
            GameManager.Instance.Stage1Hp = playerManager.Hp;
            SceneManager.LoadScene("Stage11.0");
        }
        else if(Monster.monsterCount>0 && arrivedAtPortal)
        {
            GameManager.Instance.isMonsterLeft = true;
        }

    }
}
