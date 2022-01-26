using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// Generalized Tower Class. Every Tower should be of this Type or should inherit this Type
// Set all Tower-Type Specific Informations as Public Variables (e.g. Animations, Sounds)
public class Tower : MonoBehaviourPunCallbacks, IPunObservable
{

    public float attackSpeed;
    public int attackDamage;
    public GameObject projectile;
    public float range;
    public AudioClip shootAudio;

    public PlayerType owner;

    public GameObject target = null;
    //[NonSerialized] public bool isPlaced = false;
    private float shootingTimer;
    private List<int> idList = new List<int>();
    private List<GameObject> enemyList = new List<GameObject>();

    public ParticleSystem empoweredAnimationLight;
    public ParticleSystem empoweredAnimationEvil;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(owner);
        }
        else
        {
            owner = (PlayerType)stream.ReceiveNext();
        }

    }

    private void Start()
    {
        shootingTimer = attackSpeed;

    }
    
    public void Update()
    {
        // if enemy enters range and not shooting
        // set angle according to target

        if (photonView.IsMine)
        {

            // Add rotation if we have sprites
            if (shootingTimer <= 0f)
            {
                target = UpdateTarget();
                if (target != null)
                {
                    PhotonView targetView = target.GetComponent<PhotonView>();
                    if (targetView)
                    {
                        photonView.RPC("Shoot", RpcTarget.All, targetView.ViewID, this.transform.position);

                    }
                }
                shootingTimer = attackSpeed;

            }
            

            if (shootingTimer > 0f)
            {
                shootingTimer -= Time.deltaTime;
            }
        }
    }

    private ParticleSystem currentAnimation = null;
    private bool isEmpowered = false;
    public float empowerSpeed = 1.4f;
    public void Empower()
    {
        // do for all
        if (currentAnimation == null)
        {
            ParticleSystem empoweredAnimation;
            if (owner == PlayerType.Live)
                empoweredAnimation = empoweredAnimationLight;
            else
            {
                empoweredAnimation = empoweredAnimationEvil;
            }
            currentAnimation = Instantiate(empoweredAnimation, this.gameObject.transform.position, this.gameObject.transform.rotation);
        }
        else
        {
            currentAnimation.Play();
        }

        isEmpowered = true;
        attackSpeed = attackSpeed / empowerSpeed;
    }

    public void PowerDown()
    {
        if(currentAnimation != null)
        {
            currentAnimation.Stop();
        }

        attackSpeed = attackSpeed * empowerSpeed;
    }

    [PunRPC]
    public void Shoot(int targetID, Vector3 currentPosition)
    {
        //Spawn gameobject projectile
        //if enemy in range call shoot(itself) with delay based on attackspeed 
        PhotonView target = PhotonView.Find(targetID);
        GameObject projectile = Instantiate(this.projectile, currentPosition, Quaternion.identity);
        projectile.GetComponent<Projectile>().range = range;
        projectile.GetComponent<Projectile>().damage = attackDamage;
        projectile.GetComponent<Projectile>().target = target.gameObject;
        projectile.GetComponent<Projectile>().originPlayer = owner;
        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(shootAudio);
        //Vector3 tempVect = projectile.transform.position + shootingDirection * Time.deltaTime * 300f;
        //projectile.GetComponent<Rigidbody2D>().MovePosition(tempVect);

    }
    //
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!idList.Contains(other.transform.GetInstanceID()))
    //     {
    //         enemyList.Add(other.gameObject);
    //         idList.Add(other.transform.GetInstanceID());
    //     }
    //
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.transform.CompareTag("Enemy"))
    //     {
    //         if (idList.Contains(other.transform.GetInstanceID()))
    //         {
    //             //Debug.Log("Here");
    //             enemyList.Remove(other.gameObject);
    //             idList.Remove(other.transform.GetInstanceID());
    //         } 
    //         //Debug.Log(enemyList.Count);
    //
    //     }
    //
    // }

    public virtual GameObject UpdateTarget()
    {
        /*Debug.Log("target search");
        Debug.Log(target);
        Debug.Log(enemyList.Count);
        if (enemyList.Count == 0)
        {
            target = null;
            return;
        }
        
        float shortestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemyList)
        {
            Debug.Log("enemy no", enemy);
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    target = enemy;
                }
            }

        }*/
        
        // Could also change to range, but collider depicts tower range better visually
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position,
            transform.GetComponent<CircleCollider2D>().radius,
            LayerMask.GetMask("Enemy"));
        
        float shortestDistance = Mathf.Infinity;
        GameObject newTarget = null;
        
        foreach (Collider2D enemy in enemiesInRange)
        {
            // if its enemy from another player
            if (enemy.GetComponent<Enemy>().type != owner)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    newTarget = enemy.gameObject;
                }
            }

        }

        return newTarget;
    }


}
