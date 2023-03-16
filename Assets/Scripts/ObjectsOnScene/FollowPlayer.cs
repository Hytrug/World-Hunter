using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //gameobject para por o player
    public GameObject player;

    void Update()
    {
        //transforma a posição do objeto na do player
        transform.position = player.transform.position;;
    }
}
