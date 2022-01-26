using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyCarrier : Enemy
{
    public GameObject basicEnemy;
    public override void ReceiveDamage(int damage)
    {
        if (photonView.IsMine){
            basicEnemy.GetComponent<Enemy>().path = path;
            basicEnemy.GetComponent<Enemy>().type = type;
            basicEnemy.GetComponent<Enemy>().nextWaypoint = nextWaypoint;
            basicEnemy.GetComponent<Enemy>().currentCenter = currentCenter;
            basicEnemy.GetComponent<Enemy>().setStart = setStart;
            PhotonNetwork.Instantiate(basicEnemy.name, this.transform.position, Quaternion.identity);
            base.ReceiveDamage(damage);
        }
    }
}
