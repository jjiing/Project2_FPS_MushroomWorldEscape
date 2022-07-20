using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeImageControl : MonoBehaviour
{
    Image[] lifes;
    public GameObject lifesOn;

 
    private void Start()
    {
        lifes = lifesOn.GetComponentsInChildren<Image>();
        
    }
    private void Update()
    {
        PlayerPrefs.SetInt("lifeNum", GameManager.Instance.Life);
        for(int i=0; i<3-PlayerPrefs.GetInt("lifeNum");i++)
        {
            lifes[i].gameObject.SetActive(false);
        }
        
    }
}
