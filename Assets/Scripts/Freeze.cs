using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Freeze : NetworkBehaviour {

    public Material frozen;
    public Material unFronzen_player1;
    public Material unFronzen_player2;
    private bool isFrozen = false;

    private void OnCollisionEnter(Collision other)
    {
        //if you are player and other is enemy
        if (gameObject.CompareTag("Player") && other.gameObject.CompareTag("Enemy") )
        {
            Debug.Log(isFrozen);
            //freeze yourself
            if (hasAuthority && isFrozen == false)
            {
                isFrozen = true;
                CmdFreeze(this.gameObject);
                
            }
            

        }
        //if you are player and other is player
        else if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Player"))
        {
            Debug.Log(isFrozen);
            //unfreeze yourself
            if (hasAuthority && isFrozen )
            {
                isFrozen = false;
                CmdUnFreeze(this.gameObject);
                
            }            
        }
    }

    [Command]
    private void CmdFreeze(GameObject player)
    {
        RpcFreeze(player);
        FindObjectOfType<_GameManager>().CmdAddFrozenCount();
    }

    [ClientRpc]
    private void RpcFreeze(GameObject player)
    {
        player.GetComponent<MeshRenderer>().material = frozen;
        player.GetComponent<PlayerController>().enabled = false;
        
    }
    
    [Command]
    private void CmdUnFreeze(GameObject player)
    {
        RpcUnFreeze(player);
        FindObjectOfType<_GameManager>().CmdMinusFrozenCount();
    }

    [ClientRpc]
    private void RpcUnFreeze(GameObject player)
    {
        player.GetComponent<MeshRenderer>().material = unFronzen_player1;
        player.GetComponent<PlayerController>().enabled = true;
    }
}
