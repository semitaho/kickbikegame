using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : IActivator, IKickable
{

    [SerializeField] private float currentPower = 1f;

    private Animator friendAnimator;

    private Transform currentWaypoint;
    private KickBikeController kickBikeController;

    [SerializeField] WaypointController aiPath;

    private float distanceToTurn = 1f;
    // Start is called before the first frame update

    private void Awake()
    {
        friendAnimator = GetComponentInChildren<Animator>();
        currentWaypoint = aiPath.GetCurrentWaypoint();

    }

    void Start()
    {
        kickBikeController = transform.GetComponentInChildren<KickBikeController>();
        friendAnimator.SetTrigger("Kicking");
    }
    public void OnKick()
    {
        kickBikeController.Boost(currentPower);
    }

    public void OnKickEnded()
    {
        friendAnimator.SetTrigger("Kicking");

    }



    // Update is called once per frame
    void Update()
    {
        kickBikeController.Steer(GetHorizontalSteerValue());
        // kickBikeController.Steer(GetHorizontalSteerValue();)
        if (Vector3.Distance(toTransformHeightPosition(currentWaypoint.position), transform.position) <= distanceToTurn)
        {
            currentWaypoint = aiPath.ChangeWaypoint();
        }
    }



    Vector3 toTransformHeightPosition(Vector3 vector3)
    {
        return new Vector3(vector3.x, transform.position.y, vector3.z);

    }

    private float GetHorizontalSteerValue()
    {
        var direction3d = new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z) - transform.position;

        var directionInverse = transform.InverseTransformDirection(direction3d);
        Quaternion relativeLookRotation = Quaternion.LookRotation(directionInverse, transform.up);
        // var que = Quaternion.FromToRotation(new Vector2(transform.position.x, transform.position.z), direction2d);
        return relativeLookRotation.y;
    }

    public void OnReverse()
    {
        throw new System.NotImplementedException();
    }
}
