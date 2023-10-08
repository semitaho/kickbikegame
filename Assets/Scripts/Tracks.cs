using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{


    [SerializeField] private float forwardSlipForTracks = 0.4f;

    [SerializeField] private float sidewaySlipForTracks = 0.3f;
    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private WheelCollider wheelCollider;
    // Start is called before the first frame update
    void Start()
    {
        trailRenderer.Clear();
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        WheelHit hit;
        if (wheelCollider.GetGroundHit(out hit))
        {
            if (ShowTrack(hit))

            {
                trailRenderer.emitting = true;
            }
            else
            {
                trailRenderer.emitting = false;
            }

        }

    }

    private bool ShowTrack(WheelHit hit)
    {
        return hit.collider.name.Contains("Road") &&
 (
    Mathf.Abs(hit.forwardSlip) >= forwardSlipForTracks
 || Mathf.Abs(hit.sidewaysSlip) >= sidewaySlipForTracks);
    }
}
