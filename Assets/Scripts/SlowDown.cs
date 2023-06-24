using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{

    [SerializeField][Range(0f, 10f)] private float slowDownRatio = 2f;

    [SerializeField] private ParticleSystem waterSplashSystem;

    private GameObject playerContainer;
    private float originalBreakSpeed;



    private Dictionary<string, int> gameobjectColliderCount;
    // Start is called before the first frame update
    void Start()
    {
        gameobjectColliderCount = new Dictionary<string, int>();


    }

    public float GetSlowDownRatio() {
        return slowDownRatio;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerContainer != null)
        {
            //gameObject.transform.rotation = Quaternion.Euler(0, gameObject.transform.rotation.y, 0);
        }


    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnCollisionEnter(Collision other)
    {

        var fixer = other.gameObject.GetComponent<Fixer>();
        if (fixer != null)
        {
        }

    }

    public void Hitting(GameObject other)
    {
        TriggerParticles(other);
    }

    private void TriggerParticles(GameObject other)
    {
        var particlePosition = other.transform.position;

        var particle = Instantiate(waterSplashSystem, particlePosition, Quaternion.identity);

        particle.Play();
        Destroy(particle, 1.5f);
    }


}
