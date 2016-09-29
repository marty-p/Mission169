using UnityEngine;
using System.Collections;

public class CameraPath : MonoBehaviour {

        public GameObject[] wayPoints;

	void LateUpdate () {
	    for (int i=0; i < wayPoints.Length; i++){
                if (wayPoints[i].transform.position.x > transform.position.x) {

                    if (transform.position.y != wayPoints[i].transform.position.y){
                        // y = ax + b;
                        // a = (y2 - y1) / (x2 - x1)
                        float a = (wayPoints[i].transform.position.y - wayPoints[i-1].transform.position.y) / (wayPoints[i].transform.position.x - wayPoints[i-1].transform.position.x);
                        // b = y - ax
                        float b = wayPoints[i].transform.position.y - a*wayPoints[i].transform.position.x;
                        
                        float yToGoForThisX = a*transform.position.x + b;
                        Vector3 fixedPos = new Vector3( transform.position.x, yToGoForThisX, -10 );
                        transform.position = fixedPos;
                    }
                    break;
                }
            }

	}
}
