using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    public Vector3[] waypoints_Live;
    public Vector3[] waypoints_Evil;
    public Vector3 spawn_Live;
    public Vector3 spawn_Evil;
    public float[] waypointsType;
    [Tooltip("- = clockwise 90 degree corner\n  + = counterclockwise 180 degree corner\n 1 = 180 degree corner\n 2 = center right\n 3 = center down\n 4 = center left\n 5 = center up")]
    public Vector3[] centerList_Live;
    public Vector3[] centerList_Evil;

    public Vector3 getSpawn(PlayerType type)
    {
        if(type == PlayerType.Live)
        {
            return spawn_Live;
        }
        else
        {
            return spawn_Evil;
        }
    }

    public Vector3[] getPath(PlayerType type)
    {
        if (type == PlayerType.Live)
        {
            return waypoints_Live;
        }
        else
        {
            return waypoints_Evil;
        }
    }

    public float[] getPathType()
    {
        return waypointsType;
    }

    public Vector3[] getCenters(PlayerType type)
    {
        if (type == PlayerType.Live)
        {
            return centerList_Live;
        }
        else
        {
            return centerList_Evil;
        }
    }

}
