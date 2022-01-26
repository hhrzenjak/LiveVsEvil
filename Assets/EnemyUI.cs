using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyUI : MonoBehaviour
{
    public Player player;
    public GameObject basicEnemy;
    public int basicEnemyCost;
    public int basicIncomeIncrease;
    public GameObject fastEnemy;
    public int fastEnemyCost;
    public int fastIncomeIncrease;
    public GameObject carrierEnemy;
    public int carrierEnemyCost;
    public int carrierIncomeIncrease;
    public GameObject bigEnemy;
    public int bigEnemyCost;
    public int bigIncomeIncrease;
    public GameObject miniEnemy;
    public int miniNumber;
    public int miniEnemyCost;
    public int miniIncomeIncrease;
    public GameArea gameArea;
    
    
    public void SpawnBasicEnemy()
    {
        if (player.buy(basicEnemyCost))
        {
            player.moneyIncrease += basicIncomeIncrease;
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SpawnBasicEnemyMaster", RpcTarget.Others, player.getPlayerType());
        }

    }


    public void SpawnFastEnemy()
    {
        if (player.buy(fastEnemyCost))
        {
            player.moneyIncrease += fastIncomeIncrease;
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SpawnFastEnemyMaster", RpcTarget.Others, player.getPlayerType());
        }
    }

    public void SpawnCarrierEnemy()
    {
        if (player.buy(carrierEnemyCost))
        {
            player.moneyIncrease += carrierIncomeIncrease;
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SpawnCarrierEnemyMaster", RpcTarget.Others, player.getPlayerType());
        }
    }

    public void SpawnMiniEnemy()
    {
        if (player.buy(miniEnemyCost))
        {
            player.moneyIncrease += miniIncomeIncrease;
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SpawnMiniEnemyMaster", RpcTarget.Others, player.getPlayerType());
        }
    }
    
    public void SpawnBigEnemy()
    {
        if (player.buy(bigEnemyCost))
        {
            player.moneyIncrease += bigIncomeIncrease;
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("SpawnBigEnemyMaster", RpcTarget.Others, player.getPlayerType());
        }

    }

    [PunRPC]
    void SpawnBasicEnemyMaster(PlayerType type)
    {
        basicEnemy.GetComponent<Enemy>().type = type;
        basicEnemy.GetComponent<Enemy>().nextWaypoint = 0;
        basicEnemy.GetComponent<Enemy>().currentCenter = 0;

        PhotonNetwork.Instantiate(basicEnemy.name, gameArea.getSpawn(type), Quaternion.identity);
    }

    [PunRPC]
    void SpawnFastEnemyMaster(PlayerType type)
    {
        fastEnemy.GetComponent<Enemy>().type = type;
        fastEnemy.GetComponent<Enemy>().nextWaypoint = 0;
        basicEnemy.GetComponent<Enemy>().currentCenter = 0;
        PhotonNetwork.Instantiate(fastEnemy.name, gameArea.getSpawn(type), Quaternion.identity);
    }

    [PunRPC]
    void SpawnCarrierEnemyMaster(PlayerType type)
    {
        carrierEnemy.GetComponent<Enemy>().type = type;
        carrierEnemy.GetComponent<Enemy>().nextWaypoint = 0;
        basicEnemy.GetComponent<Enemy>().currentCenter = 0;
        PhotonNetwork.Instantiate(carrierEnemy.name, gameArea.getSpawn(type), Quaternion.identity);
    }
    
    [PunRPC]
    void SpawnBigEnemyMaster(PlayerType type)
    {
        bigEnemy.GetComponent<Enemy>().type = type;
        bigEnemy.GetComponent<Enemy>().nextWaypoint = 0;
        bigEnemy.GetComponent<Enemy>().currentCenter = 0;

        PhotonNetwork.Instantiate(bigEnemy.name, gameArea.getSpawn(type), Quaternion.identity);
    }


    [PunRPC]
    void SpawnMiniEnemyMaster(PlayerType type)
    {
        miniEnemy.GetComponent<Enemy>().type = type;
        carrierEnemy.GetComponent<Enemy>().nextWaypoint = 0;
        basicEnemy.GetComponent<Enemy>().currentCenter = 0;
        for (int i = 0; i < miniNumber; i++)
        {
            Vector3 position = gameArea.getSpawn(type);
            position.y += (i * 0.2f);
            PhotonNetwork.Instantiate(miniEnemy.name, position, Quaternion.identity);         

        }
    }

}
