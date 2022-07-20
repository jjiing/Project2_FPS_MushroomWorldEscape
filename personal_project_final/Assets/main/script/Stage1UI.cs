using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Stage1UI : MonoBehaviour
{
    public GameObject hpCircle;
    public Text hpNum;
    public Image bulletBarFill;
    public PlayerManager playerManager;
    public Gun gun;
    public Image gunChargeShot;
    public Text gunChargeShotQ;
    public Image guideText;

    //ESC
    public Image escPopUp;
    public Image escBackground;
    public Image escKeyGuide;
    public Image soundOffImage;
    public Camera camera;
    AudioListener audioListener;



    public PlayerMove playerMove;
    float originTurnSpeed;

    //gameOver
    public Image gameOverPopUp;
    public Image gameClearPopUp;
    public Text monsterLeftPopUp;

    public enum Type { STAGE_1, STAGE_2};
    public Type enumType;
    private void Start()
    {
        audioListener = camera.GetComponent<AudioListener>();
        if (SoundManager.Instance.isSoundOn)
            audioListener.enabled = true;
        else
        {
            audioListener.enabled = false;
            soundOffImage.gameObject.SetActive(true);
        }


        switch (enumType)
        {
            case Type.STAGE_1:
                
                Time.timeScale = 0;
                originTurnSpeed = playerMove.TurnSpeed;
                playerMove.TurnSpeed = 0;
                escBackground.gameObject.SetActive(true);
                guideText.gameObject.SetActive(true);
                Gun.isPaused = true;
                break;
            case Type.STAGE_2:
                break;
            default:
                break;
        }

        
    }
    private void Update()
    {
        GuideTextOff();
        HpControl();
        bulletNum();
        FuelCellSliderControl();
        EscPopup();
        GameOverUI();
        ClearUI();
        MonsterLeftUI();

    }
    private void EscPopup()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            Time.timeScale = 0;
            escBackground.gameObject.SetActive(true );
            escPopUp.gameObject.SetActive(true );
            Cursor.lockState = CursorLockMode.None;
            originTurnSpeed = playerMove.TurnSpeed;
            playerMove.TurnSpeed = 0;
            Gun.isPaused = true;

        }
    }
    
    public void OnClickCloseEsc()
    {
        Cursor.lockState = CursorLockMode.Locked;
        escPopUp.gameObject.SetActive(false);
        escBackground.gameObject.SetActive(false);
        Time.timeScale = 1;
        Gun.isPaused = false;
        playerMove.TurnSpeed = originTurnSpeed;

    }
    public void OnClickMainInEsc()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void OnClickKeyGuideInEsc()
    {
        escKeyGuide.gameObject.SetActive(true);
        escPopUp.gameObject.SetActive(false );
    }
    public void OnClickKeyGuideClose()
    {
        escKeyGuide.gameObject.SetActive(false);
        escPopUp.gameObject.SetActive(true);
    }
    public void OnclickRestart()
    {
        SceneManager.LoadScene("Scene10.0");
    }
    public void OnClickQuit()
    {
        //채워넣기
    }
  
    public void OnClickSoundMark()
    {
        soundOffImage.gameObject.SetActive(true);
        audioListener.enabled = false;
    }

    public void OnClickSoundOffImage()
    {
        soundOffImage.gameObject.SetActive(false);
        audioListener.enabled = true;

    }
    private void GuideTextOff()
    {
        if (guideText.gameObject.activeSelf==true && Input.GetKeyDown(KeyCode.Return))     //엔터
        {
           
            Time.timeScale = 1;
            escBackground.gameObject.SetActive(false);
            guideText.gameObject.SetActive(false);
            playerMove.TurnSpeed = originTurnSpeed;
           
            Gun.isPaused = false;
        }
    }

    private void HpControl()
    {
        float hp = (float)playerManager.Hp / 200;
        hpCircle.GetComponent<CircularProgressBar>().m_FillAmount = hp;
        hpNum.text = playerManager.Hp.ToString();
    }
    private void bulletNum()
    {
        if (!gun.isReload)
            bulletBarFill.fillAmount = gun.BulletCurrentRatio;
        else
            StartCoroutine(reloadUICo());
            
    }

    IEnumerator reloadUICo()
    {
        while (bulletBarFill.fillAmount < 1)
        {
            bulletBarFill.fillAmount += 0.0002f;
            yield return null;
        }
    }
    private void FuelCellSliderControl()
    {
        gunChargeShot.fillAmount = 1-gun.FuelCellRatio;
        if (gunChargeShot.fillAmount == 1)
        {
            gunChargeShot.color = new Color32(0, 255, 168, 255);
            gunChargeShotQ.color = new Color32(0, 255, 168, 255);

        }
        else
        {
            gunChargeShot.color = Color.white;
            gunChargeShotQ.color = Color.white;
        }
    }

    public void OnClickExitatGameOver()
    {
        Debug.Log("버튼클릭");
        SceneManager.LoadScene("MainScene");
        
    }

    public void GameOverUI()
    {
        if(GameManager.Instance.isGameOver)
        {
            Time.timeScale = 0;
            escBackground.gameObject.SetActive(true);
            gameOverPopUp.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerMove.TurnSpeed = 0;
            Gun.isPaused = true;
            
        }
    }
   
    public void ClearUI()
    {
        if (GameManager.Instance.isClear)
        {
            Time.timeScale = 0;

            gameClearPopUp.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerMove.TurnSpeed = 0;
            Gun.isPaused = true;

        }
    }
    public void MonsterLeftUI()
    {
        if(GameManager.Instance.isMonsterLeft)
        {
            monsterLeftPopUp.gameObject.SetActive(true);
            StartCoroutine(MonsterLeftFadeOutCo());
        }
    }

    IEnumerator MonsterLeftFadeOutCo()
    {
        Color color = monsterLeftPopUp.color;
        while (color.a >0 )
        {
            color.a -= Time.deltaTime / 3f; //1초동안 실행
            monsterLeftPopUp.color = color;
            yield return null;
        }


        color.a = 1;
        monsterLeftPopUp.color = color;
        monsterLeftPopUp.gameObject.SetActive(false);
        GameManager.Instance.isMonsterLeft = false;
    }
}
