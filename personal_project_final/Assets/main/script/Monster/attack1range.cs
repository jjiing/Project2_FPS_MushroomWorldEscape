using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack1range : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("EffectOff", 1f);
    }
  
    void EffectOff()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerManager>().Hp -= 50;
    }
}
