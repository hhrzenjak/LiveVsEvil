using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Generalized Enemy Class. Every Enemy should be of this Type or should inherit this Type
// Set all Enemy-Type Specific Informations as Public Variables (e.g. Animations, Sounds)
public class Enemy : MonoBehaviourPunCallbacks, IPunObservable
{
    public Image healthBar;
    public int startingHealth = 100;
    public int currentHealth;
    public float speed;
    public bool isSlowed;
    public AudioClip deathAudio;
    public AudioClip spawnAudio;
    public AudioClip hitAudio;

    public ParticleSystem hitAnimation;
    public ParticleSystem deathAnimationLive;
    public ParticleSystem deathAnimationEvil;

    public GameObject gameArea;

    public float[] pathType;

    public bool setStart = false;
    private Vector3 center;
    private float startTime;
    private Vector3 startPosition;
    public Vector3[] centerList;
    public int currentCenter = 0;

    [SerializeField] public PlayerType type;

    [SerializeField] public Vector3[] path;

    [SerializeField] public int nextWaypoint = 0;

    [SerializeField] private Vector3 networkpos;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //networkpos = transform.position;
            //stream.SendNext(networkpos);
            //stream.SendNext(nextWaypoint);
            ////stream.SendNext(path); //needed?
            stream.SendNext(currentHealth);
            stream.SendNext(type); //needed?
            stream.SendNext(speed);
            //stream.SendNext(this.transform.rotation);
            //stream.SendNext(velocity);
            //stream.SendNext(currentCenter);
            //stream.SendNext(setStart);
        }
        else
        {
            //networkpos = (Vector3)stream.ReceiveNext();
            //nextWaypoint = (int)stream.ReceiveNext();
            //path = (Vector3[])stream.ReceiveNext();
            currentHealth = (int) stream.ReceiveNext();
            type = (PlayerType) stream.ReceiveNext();
            speed = (float) stream.ReceiveNext();
            //currentCenter = (int)stream.ReceiveNext();
            //setStart = (bool)stream.ReceiveNext();

            //TODO: rigidbody.rotation = (Quaternion)stream.ReceiveNext();
            //rigidbody.velocity = (Vector3)stream.ReceiveNext();

            // if (Vector3.Distance(transform.position,networkpos)>=1)
            // {
            //     transform.position = networkpos;
            // }
            UpdateHealthBar();
        }
    }

    private void Awake()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();
    }

    private void Start()
    {
        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(spawnAudio);

        gameArea = GameObject.Find("GameArea");
        path = gameArea.GetComponent<GameArea>().getPath(type);
        centerList = gameArea.GetComponent<GameArea>().getCenters(type);
        pathType = gameArea.GetComponent<GameArea>().getPathType();
    }

    public void setPath(Vector3[] path)
    {
        this.path = path;
    }

    public void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }

        //move Enemy 
    }

    public void Move()
    {
        Vector3 nextPos = path[nextWaypoint];
        float nextPathType = pathType[nextWaypoint];
        Vector3 currentPosition = transform.position;

        if (nextPathType == 0) // go straight
        {
            Vector3 directionOfTravel = nextPos - currentPosition;
            directionOfTravel.Normalize();

            this.transform.Translate(
                directionOfTravel.x * speed * Time.deltaTime,
                directionOfTravel.y * speed * Time.deltaTime,
                directionOfTravel.z * speed * Time.deltaTime,
                Space.World
            );
        }
        else
        {
            //fix if i get better idea
            //parameters that need to be set on start
            if (!setStart)
            {
                startPosition = this.transform.position;
                center = centerList[currentCenter];
                currentCenter += 1;
                startTime = Time.time;
                setStart = true;
            }

            if (nextPathType == 6)
                transform.RotateAround(center, new Vector3(0, 0, 1), 0.18f);
            // else if(nextPathType == -5)
            //     transform.RotateAround(center, new Vector3(0, 0, 1), -0.18f);
            Vector3 firstC = startPosition - center;
            Vector3 secC = nextPos - center;

            float tripTime = 2.0f;
            if (Math.Abs(nextPathType) == 5)
            {
                tripTime *= 2;
            }

            float fracComplete = (Time.time - startTime) / tripTime * speed;

            transform.position = Vector3.Slerp(firstC, secC, fracComplete);
            transform.position += center;
        }

        if (Vector3.Distance(currentPosition, nextPos) < .1f)
        {
            if (nextWaypoint + 1 == path.Length)
            {
                //reached Goal
                this.Remove();
            }
            else
            {
                nextWaypoint++;
                setStart = false;
            }
        }
    }

    public virtual void ReceiveDamage(int damage)
    {
        if (photonView.IsMine)
        {
            //reduce Health
            //call Die if necessary
            currentHealth -= damage;
            UpdateHealthBar();
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                photonView.RPC("playHitAnimation", RpcTarget.All);
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / (float) startingHealth;
        healthBar.color = Color.Lerp(Color.red, Color.green, healthBar.fillAmount);
    }

    public void Die()
    {
        photonView.RPC("playDeathAnimation", RpcTarget.All);
        Remove();
    }

    public void Remove()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void playDeathAnimation()
    {
        ParticleSystem deathAnimation;
        if (type == PlayerType.Live)
            deathAnimation = deathAnimationLive;
        else
            deathAnimation = deathAnimationEvil;
        Instantiate(deathAnimation, this.gameObject.transform.position, this.gameObject.transform.rotation);

        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(deathAudio);
    }

    [PunRPC]
    public void playHitAnimation()
    {
        Instantiate(hitAnimation, this.gameObject.transform.position, this.gameObject.transform.rotation);

        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(hitAudio);
    }

    private void Slow()
    {
        if (!isSlowed)
        {
            speed = speed / 2;
            isSlowed = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (photonView.IsMine)
        {
            if (other.GetComponent<Projectile>() != null && other.GetComponent<Projectile>().originPlayer != type)
            {
                if (other.tag == "Bullet")
                {
                    if (other.GetComponent<Projectile>().target == gameObject)
                        ReceiveDamage(other.GetComponent<Projectile>().damage);
                }

                if (other.tag == "SlowingBullet")
                {
                    if (other.GetComponent<Projectile>().target == gameObject) Slow();
                }
            }
        }
    }
}