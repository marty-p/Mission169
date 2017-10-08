using UnityEngine;

public class LevelData : MonoBehaviour {

    [SerializeField] Transform startLocation;
    [SerializeField] Transform[] cameraWayPoints;
    [SerializeField] BgParallax[] parallaxBgs;

    public Transform StartLocation { get { return startLocation; } }
    public Transform[] CameraWayPoints {  get { return cameraWayPoints; } }
    public BgParallax[] ParallaxBgs {  get { return parallaxBgs; } }
}
