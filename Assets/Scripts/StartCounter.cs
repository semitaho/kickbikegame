using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Playables;
public class StartCounter : MonoBehaviour
{

    [SerializeField] [Range(1,10)] private int countFrom = 5;

    [SerializeField] private Text counter;

    private float currentCounter;

    private float fontSizeCurrent = 10;
    [SerializeField] private  float fontSizeTo = 80;

    [SerializeField] private float goCounterTime = 2;

    [SerializeField] private AudioClip GoVFX;

    private PlayableDirector director;


    private bool counterEnabled = true;

    private int currentVisibleFloatNumber;
    // Start is called before the first frame update
    private void Awake() {
        
        this.director = GetComponent<PlayableDirector>();
       //this.ToggleActivation(false);        
    }
    
    void Start()
    {
    }


    private void OnEnable() {
        Debug.Log("Counter enabled!");
        this.currentCounter = 0;
        this.counterEnabled = true;
        this.refreshCounterText();        
    }



    // Update is called once per frame
    void Update()
    {
        if (!counterEnabled) return;
        this.currentCounter += Time.deltaTime;


        this.refreshCounterText();
        if (this.currentCounter >= this.countFrom)
        {
            StartCoroutine(StartGame());

        }
    }

    private IEnumerator StartGame()
    {
        counterEnabled = false;
        this.fontSizeCurrent = this.fontSizeTo;
        AudioSource.PlayClipAtPoint(GoVFX, GameObject.FindGameObjectWithTag("Player").transform.position);
        this.refreshText("GO!");
        this.ToggleActivation(true);
     //   this.director.Play();
        yield return new WaitForSeconds(goCounterTime);
        this.refreshText("");
        enabled = false;

    }

    private void ToggleActivation(bool toggleValue)
    {
        IActivator[] activators = FindObjectsOfType<IActivator>();
        foreach(var activator in activators)
        {
            activator.enabled = toggleValue;
        }


    }

    private void refreshCounterText()
    {

        var number = Mathf.CeilToInt(this.countFrom - this.currentCounter);
        var font = this.fontSizeCurrent * Time.deltaTime * 3000;
        this.fontSizeCurrent = Mathf.Lerp(this.fontSizeCurrent, this.fontSizeTo, Time.deltaTime * 5);
        if (number == this.currentVisibleFloatNumber)
        {
            this.refreshText(number.ToString());
            return;

        }
        this.fontSizeCurrent = 0;
        this.currentVisibleFloatNumber = Mathf.CeilToInt(this.countFrom - this.currentCounter);
        this.refreshText(this.currentVisibleFloatNumber.ToString());
    }
    private void refreshText(string text)
    {
        this.counter.text = text;
        this.counter.fontSize = Mathf.RoundToInt(this.fontSizeCurrent);


    }
}
