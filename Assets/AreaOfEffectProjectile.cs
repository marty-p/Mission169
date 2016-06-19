using UnityEngine;

public class AreaOfEffectProjectile : MonoBehaviour{

    public Vector2 boxSize = new Vector2(0.2f, 0.2f);
    public Vector2 boxOffset = new Vector2();
    public ProjectileProperties projectileProp;

    public void CastAOE(string victimsTag, Vector2 pos) {
        Vector2 center = new Vector2(pos.x - transform.right.x * (boxSize.x / 2) + boxOffset.x * transform.right.x,
            pos.y -(boxSize.y / 2) + boxOffset.y);
                
        float duration = 1.5f;
        Debug.DrawLine(center, new Vector2(center.x, center.y + boxSize.y), Color.red, duration);
        Debug.DrawLine(center, new Vector2(center.x + boxSize.x * transform.right.x, center.y), Color.blue, duration);
        Debug.DrawLine(new Vector2( center.x + boxSize.x * transform.right.x, center.y),
            new Vector2(center.x + boxSize.x * transform.right.x, center.y + boxSize.y), Color.cyan, duration);

        Vector2 centre = new Vector2(pos.x + boxOffset.x * transform.right.x, pos.y + boxOffset.y);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(centre, boxSize, 0, transform.right, 0);

        for (int i=0; i<hits.Length; i++) {
            if (hits[i].collider.tag == victimsTag) {
                HealthManager healthManager = hits[i].collider.GetComponentInChildren<HealthManager>();
                healthManager.OnHitByProjectile(projectileProp);
            }
        }
    }

}
