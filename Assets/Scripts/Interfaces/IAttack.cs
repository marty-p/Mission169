using UnityEngine;

public interface IAttack {

    void Execute(string victimTag, Vector3 dir = new Vector3(),
            Vector3 ProjectileInitalPos = new Vector3());
}
