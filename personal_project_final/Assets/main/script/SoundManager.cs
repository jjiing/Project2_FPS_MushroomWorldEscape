using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public bool isSoundOn;


    void Awake()
    {
        if (null == Instance)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        isSoundOn = true;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Debug.Log("ISSOUNDDON : " + isSoundOn);
    }
}
