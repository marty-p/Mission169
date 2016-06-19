using UnityEngine;

public interface IProjectile {

    void Launch (string victimsTag, Vector2 destination = new Vector2());
}
