using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject target;
    public int damage;
    public float speed;

    public float range;

    public PlayerType originPlayer;

    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Remove();
        }
        else
        {
            Vector3 nextPos = target.transform.position;
            Vector3 currentPosition = transform.position;
            Vector3 directionOfTravel = nextPos - currentPosition;
            directionOfTravel.Normalize();

            transform.Translate(
                    directionOfTravel * (speed * Time.deltaTime),
                    Space.World
                );

            if (Vector3.Distance(currentPosition, startPosition) > range)
            {
                Remove();
            }
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Enemy>().type != originPlayer)
            {
                Remove();
            }
        }

    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }


}
