using Controllers;
using UnityEngine;

namespace Controllers
{
    public interface IBallController
    {
        void PopBall();
    }

    public class SingleBallController : MonoBehaviour, IBallController
    {
        internal Rigidbody2D _ballRigidbody;
        internal float BallScale;
        
        //events
        public delegate void BallPopped(SingleBallController poppedBall);
        public event BallPopped OnBallPopped;

        public void Init(IBallsController controller, Vector2 startForce)
        {
            //TODO: need to refactor and move some things to view class
            _ballRigidbody = GetComponent<Rigidbody2D>();
            _ballRigidbody.AddForce(startForce, ForceMode2D.Impulse);
            BallScale = GetComponent<Transform>().localScale.x;
            GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        }

        public void PopBall()
        {
            OnOnBallPopped(this);
        }

        protected virtual void OnOnBallPopped(SingleBallController poppedball)
        {
            OnBallPopped?.Invoke(poppedball);
        }
    }
}
