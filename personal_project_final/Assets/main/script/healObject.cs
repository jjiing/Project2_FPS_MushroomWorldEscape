using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healObject : MonoBehaviour
{
    public AudioClip  healSound;
    
    void Update()
    {
        transform.Rotate(new Vector3(0, 3, 0));

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerManager>().Hp < 200)
            {
                Debug.Log("Ãæµ¹");
                other.GetComponent<PlayerManager>().Hp += 30;
                other.GetComponent<PlayerManager>().PlaySE(healSound);
                Destroy(gameObject);
            }
        }

    }
}
