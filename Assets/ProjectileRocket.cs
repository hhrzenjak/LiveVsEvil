using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRocket : Projectile
{
    public ParticleSystem explosionAnimation;
    public AudioClip explosionAudio;
    public CircleCollider2D explosionCollider;
    public float explosionRadius;
    public override void Remove()
    {
        Instantiate(explosionAnimation, this.gameObject.transform.position, this.gameObject.transform.rotation);

        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(explosionAudio);

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position,
            explosionRadius,
            LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in enemiesInRange)
        {
            Enemy current = enemy.gameObject.GetComponent<Enemy>();
            current.ReceiveDamage(damage);
        }

        Destroy(gameObject);

    }
}
