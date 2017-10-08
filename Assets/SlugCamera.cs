using UnityEngine;
using SlugLib;

//Really just a wraper for all the components the camera needs

public class SlugCamera : MonoBehaviour {

    [SerializeField] FollowTarget followTarget;
    [SerializeField] FollowPath followPath;
    [SerializeField] Parallax parallax;

    public void SetTargetToFollow(Transform target)
    {
        followTarget.InitTarget(target);
    }

    public void SetWayPoints(Transform[] wayPoints)
    {
        followPath.SetWayPoints(wayPoints);
    }

    public void PositionAtWayPoint(int wayPointIndex)
    {
        followPath.PositionCameraAtNode(wayPointIndex);
    }

    public void SetParallaxBgs(BgParallax[] bgs)
    {
        parallax.bgLayers = bgs;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
