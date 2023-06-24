using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{

    private int currentWaypointIndex;


    // Start is called before the first frame update
    void Awake()
    {
        currentWaypointIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetCurrentWaypoint()
    {
        return transform.GetChild(currentWaypointIndex);
    }

    private void OnDrawGizmos()
    {

            Draw(0);
        //    Draw(maxDistanceFromCenter);


    }

    private void Draw(float Distance)
     {

        
        Gizmos.color = Color.yellow;

        var currentPoint = transform.position + Vector3.left * Distance;
        var countOfChildren = transform.childCount;
        for (var index = 0; index < countOfChildren; index++)
        {
            var childTransform = transform.GetChild(index);
            DrawLine(Color.yellow, currentPoint, childTransform.position+ Vector3.left * Distance);
            currentPoint = childTransform.position +  Vector3.left * Distance;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(currentPoint, 0.4f);
        }

    }

    private static void DrawLine(Color color, Vector3 from, Vector3 toTransform)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(from, toTransform);
    }

    private static Vector3 GetPositionFromXDistance(Transform transform, float xdistance)
    {
        var newPoint = transform.TransformVector(Vector3.left * xdistance);
        return newPoint;

    }

    public Transform ChangeWaypoint()
    {
        currentWaypointIndex += 1;
        if (currentWaypointIndex >= transform.childCount)
        {
            currentWaypointIndex = 0;
        }
        return transform.GetChild(currentWaypointIndex);
    }
}
