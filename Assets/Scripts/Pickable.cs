using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{


    [SerializeField] private ParticleSystem pickedEffect;

    [SerializeField] private AudioClip pickableSound;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        var particleSystem = Instantiate(pickedEffect, transform.position, pickedEffect.transform.localRotation);
        particleSystem.Play();
        if (pickableSound != null)
        {
            AudioSource.PlayClipAtPoint(pickableSound, transform.position);
        }
        Destroy(gameObject);


    }
}
