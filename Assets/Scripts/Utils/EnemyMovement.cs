using System;
using UnityEngine;

namespace EnemyUtils {
    class EnemyMovement {

        private static Vector3 rightSide = Vector3.zero;
        private static Vector3 leftSide = new Vector3(0, 180, 0);

        public static void TurnAround(Transform enemy) {
            if (enemy.right.x == 1) {
                enemy.eulerAngles = leftSide;
            } else if (enemy.right.x == -1) {
                enemy.eulerAngles = rightSide;
            }
        }

        public static void WalkAhead(Transform enemyT, Rigidbody2D enemyR) {
            enemyR.velocity = new Vector2(enemyT.right.x, 0);
        }

    }
}
