using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    private List<PathPoint> _PathTransform = new List<PathPoint>();
    private int _TotalPathPoints = 0;                   // Total number of points in this path
    public int TotalPathPoints { get => _TotalPathPoints; }
    [SerializeField] private int _WaitAtIndex = -1;
    public int WaitAtIndex { get => _WaitAtIndex; }

    public void Awake()
    {
        for(int i = 0; i < this.transform.childCount; ++i)
        {
            Transform pathPointChild = this.transform.GetChild(i);
            if(pathPointChild)
            {
                PathPoint point = pathPointChild.GetComponent<PathPoint>();
                if(point)
                    _PathTransform.Add(point);
            }

            _TotalPathPoints++;
        }
    }

    public Vector3 GetPointPosition(int index)
    {
        return _PathTransform[index].transform.position;
    }


}
