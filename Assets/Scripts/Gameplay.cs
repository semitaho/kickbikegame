using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class Gameplay : IActivator
{



    private float timeInSecondsLeft;

    [SerializeField] private ParticleSystem particles;

    [SerializeField] private Transform player;

    [SerializeField] private GameObject rock;

    [SerializeField] private StartCounter notifyText;

    [SerializeField] private GameObject uiMenu;

    private CheckpointManager checkpointManager;


    private PlayableDirector director;



    public static Gameplay instance { get; private set; }


    public void CheckpointTriggered(string text)
    {
        notifyText.TriggerFlicker(text, 3);
        checkpointManager.CheckpointTriggered();

    }

    public void TriggerGameOver()
    {
        notifyText.TriggerFlicker("GAME OVER!", 5);
        IActivator[] activators = FindObjectsOfType<IActivator>();
        foreach (var activator in activators)
        {
            activator.enabled = false;
        }
        Invoke("ActivateUiMenu", 3);
    }

    private void ActivateUiMenu()
    {
        uiMenu.SetActive(true);
    }


    public void RestartGame()
    {
        Debug.Log("RESTARTING SCENE!");
        Destroy(gameObject);
        instance = null;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();

        checkpointManager = GetComponent<CheckpointManager>();

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

    }
    // Start is called before the first frame update


    private void Update()
    {
    
    }



    public void PlayChaChaParticles()
    {

        var particlesForPlay = Instantiate(particles, player.transform.position, player.transform.localRotation);
        particlesForPlay.Play();
        Destroy(particlesForPlay, 3.0f);
    }


    // Update is called once per frame

}
