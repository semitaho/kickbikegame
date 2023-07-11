using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Playables;
using System;

public class StartCounter : MonoBehaviour
{

    [SerializeField][Range(1, 10)] private int countFrom = 5;


    private float currentCounter;
    private int currentLoop;
    private int maxAmountOfLoops;

    private Text counter;

    [SerializeField] private int maxFlickerGoCount = 5;



    [SerializeField] private AudioClip GoVFX;

    private PlayableDirector director;
    private Animator counterAnimator;


    private int currentVisibleFloatNumber;
    // Start is called before the first frame update
    private void Awake()
    {

        this.director = GetComponent<PlayableDirector>();
        this.counter = GetComponent<Text>();
        this.currentCounter = this.countFrom;
        this.counterAnimator = this.GetComponent<Animator>();
        //this.ToggleActivation(false);        
    }

    void Start()
    {
    }

    public void OnAnimationEnd()
    {
        this.currentCounter -= 1;
        if (this.currentCounter > 0)
        {
            this.TriggerCounterAnimation();
        }
        else
        {
            this.StartGame();
        }
    }

    public void OnFlickerEnd()
    {
        this.currentCounter -= 1;
        this.TriggerCounterAnimation();
    }
    public void TriggerFlicker(string text, int loopCount)
    {
        counter.text = text;
        this.currentLoop = 0;
        this.maxAmountOfLoops = loopCount;
        this.counterAnimator.SetTrigger("Flicker");


    }


    public void OnLoopEnd()
    {
        this.currentLoop += 1;
        if (this.currentLoop >= this.maxAmountOfLoops)
        {
            Debug.Log("On loop end...");
            this.counter.text = "";
        }
        else 
        {
            this.counterAnimator.SetTrigger("Flicker");

        }

    }



    private void OnEnable()
    {
        this.currentCounter = this.countFrom;
        this.counterAnimator.enabled = true;
        this.currentLoop = 0;
        this.TriggerCounterAnimation();
    }

    private void TriggerCounterAnimation()
    {
        this.counter.text = this.currentCounter.ToString();
        this.counterAnimator.SetTrigger("Countdown");

    }




    private void StartGame()
    {

        this.TriggerFlicker("GO!", this.maxFlickerGoCount);
        AudioSource.PlayClipAtPoint(GoVFX, GameObject.FindGameObjectWithTag("Player").transform.position);
        this.ToggleActivation(true);
          
       // enabled = false;

    }

    private void ToggleActivation(bool toggleValue)
    {
        IActivator[] activators = FindObjectsOfType<IActivator>();
        foreach (var activator in activators)
        {
            activator.enabled = toggleValue;
        }


    }


}
