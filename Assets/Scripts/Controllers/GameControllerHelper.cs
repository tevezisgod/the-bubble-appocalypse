using UnityEngine;

namespace Controllers
{
    public class GameControllerHelper
    {
        public static void GenerateCollidersAcrossScreen()
        {
            if (Camera.main is null) return;
            Camera main;
            Vector2 lDCorner = (main = Camera.main).ViewportToWorldPoint(new Vector3(0, 0f, Camera.main.nearClipPlane));
            Vector2 rUCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, main.nearClipPlane));
        
            var topCollider = new GameObject("topCollider").AddComponent<EdgeCollider2D>();
            var colliderPoints = topCollider.points;
            PrepareCollider(topCollider.gameObject);
            colliderPoints[0] = new Vector2(lDCorner.x, rUCorner.y);
            colliderPoints[1] = new Vector2(rUCorner.x, rUCorner.y);
            topCollider.points = colliderPoints;

            var bottomCollider = new GameObject("bottomCollider").AddComponent<EdgeCollider2D>();
            colliderPoints = bottomCollider.points;
            bottomCollider.gameObject.AddComponent<Rigidbody2D>();
            bottomCollider.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            colliderPoints[0] = new Vector2(lDCorner.x, lDCorner.y);
            colliderPoints[1] = new Vector2(rUCorner.x, lDCorner.y);
            bottomCollider.points = colliderPoints;

            var leftCollider = new GameObject("leftCollider").AddComponent<EdgeCollider2D>();
            colliderPoints = leftCollider.points;
            PrepareCollider(leftCollider.gameObject);
            colliderPoints[0] = new Vector2(lDCorner.x, lDCorner.y);
            colliderPoints[1] = new Vector2(lDCorner.x, rUCorner.y);
            leftCollider.points = colliderPoints;

            var rightCollider = new GameObject("rightCollider").AddComponent<EdgeCollider2D>();
            colliderPoints = rightCollider.points;
            PrepareCollider(rightCollider.gameObject);
            colliderPoints[0] = new Vector2(rUCorner.x, rUCorner.y);
            colliderPoints[1] = new Vector2(rUCorner.x, lDCorner.y);
            rightCollider.points = colliderPoints;
        }

        private static void PrepareCollider(GameObject edge)
        {
            edge.gameObject.layer = LayerMask.NameToLayer("Wall");
            edge.gameObject.AddComponent<Rigidbody2D>();
            edge.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }
}