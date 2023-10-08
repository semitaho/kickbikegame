using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAI : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private AudioClip cheeringClip;


    private Animator crowdAnimator;

    private List<Transform> crowds;

    private  Dictionary<string, bool> cheering = new Dictionary<string, bool>();

    // Start is called before the first frame update
    private void Awake()
    {
        crowds = new List<Transform>();
    }
    void Start()
    {

        crowdAnimator = GetComponent<Animator>();

        for (var i = 0; i < transform.childCount; i++)
        {
            crowds.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var crowd in crowds)
        {
            crowd.LookAt(playerTransform);
            if ((!cheering.ContainsKey(crowd.name) || cheering[crowd.name] == false) &&
            Maths.CalculateDistance2D(playerTransform.position, crowd.position) < 13)
            {
                Debug.Log("THIS PLAY"+crowd.name);
                cheering[crowd.name] = true;
                AudioSource.PlayClipAtPoint(cheeringClip, playerTransform.position);
            } 
           

        }

    }

    public void TriggerTuuletus()
    {
        foreach (var crowd in crowds)
        {
            crowd.GetComponent<Animator>()?.SetTrigger("chachachaa");

        }
    }
}
