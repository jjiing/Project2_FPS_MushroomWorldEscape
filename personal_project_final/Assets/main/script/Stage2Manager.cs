using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    public GameObject scene1;
    public GameObject scene2;
    public GameObject[] wave1Monster = new GameObject[5];

    public static bool arrivedAtClearPoint = false;
    public PlayerManager playerManager;

    void Awake()
    {
        int sNum = Random.Range(1, 3);
        if (sNum == 1) scene1.SetActive(true);
        else if (sNum == 2) scene2.SetActive(true);
        playerManager.Hp = GameManager.Instance.Stage1Hp;
        GameManager.Instance.isMonsterLeft = false;
    }


    void Update()
    {
        Stage2Clear();
    }
    
    void Stage2Clear()
    {
        if (Monster.monsterCount == 0 && arrivedAtClearPoint)
        {
            
            playerManager.redScreen.gameObject.SetActive(false);
            arrivedAtClearPoint=false;
            GameManager.Instance.isClear = true;
        }
        else if (Monster.monsterCount > 0 && arrivedAtClearPoint)
            GameManager.Instance.isMonsterLeft = true;
    }
}
