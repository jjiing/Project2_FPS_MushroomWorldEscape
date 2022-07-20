using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private int life;
    [SerializeField]
    private int stage1Hp;

    public bool isGameOver;
    public bool isClear;
    public bool isMonsterLeft;
    
    public int Life
    {
        get { return life; }
        set { life = value;
            if (life <= 0)
                Invoke("GameOver", 1f);
                  }
    }
    public int Stage1Hp
    {
        get { return stage1Hp; }
        set { stage1Hp = value; }
    }


    void Awake()
    {

        if (null==Instance)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        life = 3;
        isGameOver = false;
        isClear=false;
        isMonsterLeft=false;
        

    }

    void Update()
    {
        
    }

    void GameOver()
    {
        isGameOver = true;
    }
}
