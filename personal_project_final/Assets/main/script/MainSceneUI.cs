using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    public Image modeSelectPopUp;
    public Image keyGuidePopUp;
    public Image soundOffImage;
    public Camera mainCamera;
    AudioListener audioListener;


    private void Start()
    {
        audioListener = mainCamera.GetComponent<AudioListener>();
        audioListener = mainCamera.GetComponent<AudioListener>();
        if (SoundManager.Instance.isSoundOn)
            audioListener.enabled = true;
        else
        {
            audioListener.enabled = false;
            soundOffImage.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
       

    }

    public void OnClickMainPlay()
    {
        modeSelectPopUp.gameObject.SetActive(true);
    }
    public void OnClickModeSelectClose()
    {
        modeSelectPopUp.gameObject.SetActive(false);
    }
    public void OnClickClearModeNormalPlay()
    {
        SceneManager.LoadScene("Scene10.0");
    }
    public void OnClickKeyQuestionMark()
    {
        keyGuidePopUp.gameObject.SetActive(true);
    }
    public void OnClickeyGuideModeClose()
    {
        keyGuidePopUp.gameObject.SetActive(false);
    }
    public void OnClickSoundMark()
    {
        soundOffImage.gameObject.SetActive(true);
        audioListener.enabled = false;
        SoundManager.Instance.isSoundOn = false;
    }
 
    public void OnClickSoundOffImage()
    {
        soundOffImage.gameObject.SetActive(false);
        audioListener.enabled = true;
        SoundManager.Instance.isSoundOn = true;

    }





}
