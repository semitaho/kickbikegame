using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Playables;
public class Gameplay : IActivator
{

    private float timeInSecondsLeft;


    [SerializeField] private ParticleSystem particles;

    [SerializeField] private Transform player;

    [SerializeField] private GameObject rock;

    [SerializeField] private float distanceFromPlayerToDropRock = 5f;

    [SerializeField] private float heightFromPlayerToDropRock = 10f;

    private PlayableDirector director;

    [SerializeField] TextMeshProUGUI timerText;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
    // Start is called before the first frame update
    void Start()
    {
       // refreshTimerText();
        timerText.text = director.time.ToString();

    }

    private void Update()
    {
        //director.
        //timeInSecondsLeft -= Time.smoothDeltaTime;
        refreshTimerText();

    }

    private void refreshTimerText()
    {
        var timespan = TimeSpan.FromSeconds(timeInSecondsLeft);
        string str = timespan.ToString(@"mm\:ss");
        timerText.text = str;
    }


    public void PlayChaChaParticles()
    {

        var particlesForPlay = Instantiate(particles, player.transform.position, player.transform.localRotation);
        particlesForPlay.Play();
        Destroy(particlesForPlay, 3.0f);
    }

    void PlaySpawners()
    {
        //Vector3.MoveTowards(position, Vector3.forward * 3)
        Instantiate(rock, player.transform.position + (player.transform.forward * distanceFromPlayerToDropRock) + (Vector3.up * heightFromPlayerToDropRock), player.transform.localRotation);
    }

    // Update is called once per frame

}
