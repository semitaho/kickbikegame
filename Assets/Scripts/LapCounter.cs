using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LapCounter : Checkpoint
{

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
            currentLap += 1;
            RefreshNewLap();
        }
        
    }

    private void RefreshNewLap()
    {
        var text = string.Format("Lap {0}", currentLap);
         CheckpointTriggered(text);

    }
}
