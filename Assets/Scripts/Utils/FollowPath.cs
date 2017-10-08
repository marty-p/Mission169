using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;

    public void SetWayPoints(Transform[] wayPoints)
    {
        this.wayPoints = wayPoints;
    }

    public void PositionCameraAtNode(int nodeIndex)
    {
        transform.position = wayPoints[nodeIndex].transform.position;
    } 

	void LateUpdate ()
    {
	    for (int i=1; i < wayPoints.Length; i++)
        {
            if (wayPoints[i].position.x > transform.position.x)
            {
                if (transform.position.y != wayPoints[i].position.y)
                {
                    // a = (y2 - y1) / (x2 - x1)
                    float a = (wayPoints[i].position.y - wayPoints[i-1].position.y) / (wayPoints[i].position.x - wayPoints[i-1].position.x);
                    // b = y - ax
                    float b = wayPoints[i].position.y - a*wayPoints[i].position.x;
                    // y = ax + b
                    float y = a * transform.position.x + b;

                    transform.position = new Vector3(transform.position.x, y, 0 );
                }
                break;
            }
        }
	}
}
