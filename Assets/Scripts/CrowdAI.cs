using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAI : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private Animator crowdAnimator;

    private List<Transform> crowds;
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
