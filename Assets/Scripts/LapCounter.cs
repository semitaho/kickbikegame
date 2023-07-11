using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LapCounter : MonoBehaviour
{

    [SerializeField] private Text counter;

    [SerializeField] private int maxFlickerLapLoopCount = 5;

    private int currentLap;
    // Start is called before the first frame update
    
    void Awake()
    {
        currentLap = 0;
    }
    void Start()
    {

        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("one lap triggered!");
            currentLap += 1;
            RefreshNewLap();
        }
        
    }

    private void RefreshNewLap()
    {
        var text = string.Format("Lap {0}", currentLap);
        counter.GetComponent<StartCounter>().TriggerFlicker(text, maxFlickerLapLoopCount);
    }
}
