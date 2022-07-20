using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mode2Manager : MonoBehaviour
{
   public Monster monster;

    private void Awake()
    {
     
        monster.IsMode2 = true;
        
    }


}
