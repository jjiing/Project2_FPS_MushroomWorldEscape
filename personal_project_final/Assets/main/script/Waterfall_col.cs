using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall_col : MonoBehaviour
{

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DamageCo(other.GetComponent<PlayerManager>()));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator DamageCo(PlayerManager player)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            player.Hp -= 2;
        }
    }
}
