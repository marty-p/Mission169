using UnityEngine;

public class CameraPath : MonoBehaviour
{
    public GameObject[] wayPoints;

    private Camera cam;

    public void SetCamera(Camera camera)
    {
        this.cam = camera;
    }

    public void PositionCameraAtNode(int nodeIndex)
    {
        if (cam == null)
        {
            Debug.LogError("No camera set, can't position it");
            return;
        }

        cam.transform.position = wayPoints[nodeIndex].transform.position;
    } 

	void LateUpdate ()
    {
        if (cam == null)
        {
            return;
        }

	    for (int i=1; i < wayPoints.Length; i++)
        {
            if (wayPoints[i].transform.position.x > cam.transform.position.x)
            {
                if (cam.transform.position.y != wayPoints[i].transform.position.y)
                {
                    // y = ax + b;
                    // a = (y2 - y1) / (x2 - x1)
                    float a = (wayPoints[i].transform.position.y - wayPoints[i-1].transform.position.y) / (wayPoints[i].transform.position.x - wayPoints[i-1].transform.position.x);
                    // b = y - ax
                    float b = wayPoints[i].transform.position.y - a*wayPoints[i].transform.position.x;
                    
                    float yToGoForThisX = a*cam.transform.position.x + b;
                    Vector3 fixedPos = new Vector3( cam.transform.position.x, yToGoForThisX, 0 );
                    cam.transform.position = fixedPos;
                }
                break;
            }
        }
	}
}
