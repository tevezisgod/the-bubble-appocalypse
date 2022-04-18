using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class BallsController: MonoBehaviour
    {
        [SerializeField] private SingleBallController ball;
        [SerializeField] private GameObject ballsParent;
       
         private List<SingleBallController> _balls;
         private int _ballSize;
         private int _ballAmount;

         #region Events and delegates

         //Events
         public delegate void BallsListEmpty();
         public event BallsListEmpty OnBallsListEmpty;

         #endregion
         
         private void OnEnable()
         {
             _balls = new List<SingleBallController>();
             GameController.OnLevelStarted += OnLevelStarted;
             GameController.OnLevelLost += OnLevelLost;
             GameController.OnLevelWon += OnLevelWon;
             GameController.OnGameWon += OnGameWon;
         }

         #region Ball Handling Methods

         private void SpawnBalls(int amount, int size)
         {
             _ballAmount = amount;
             _ballSize = size;
             var newBallSize = new Vector3(_ballSize, _ballSize, 1);
             for (var i = 0; i < _ballAmount; i++)
             {
                 var force = GetRandomForce();
                 var newBall =  Instantiate(ball,ballsParent.transform);
                 newBall.OnBallPopped += OnBallPopped;
                 newBall.transform.localScale = newBallSize;
                 AddBallToTracking(newBall);
                 newBall.Init(force);
             }
         }

         private void SplitBall(float ballSize, Vector2 position)
         {
             var rightBall = Instantiate(ball, position + Vector2.right / 4f, Quaternion.identity);
             var leftBall = Instantiate(ball, position + Vector2.left/ 4f, Quaternion.identity);
             rightBall.transform.localScale = new Vector3(ballSize / 2, ballSize / 2, 1);
             leftBall.transform.localScale = new Vector3(ballSize / 2, ballSize / 2, 1);

             AddBallToTracking(rightBall);
             AddBallToTracking(leftBall);
             leftBall.OnBallPopped += OnBallPopped;
             rightBall.OnBallPopped += OnBallPopped;
            
             var rightBallController = rightBall;
             var rightForce = new Vector2(2, 5);
             rightBallController.Init(rightForce);
            
             var leftBallController = leftBall;
             var leftForce = new Vector2(-2, 5);
             leftBallController.Init(leftForce);
         }
         
         private void AddBallToTracking(SingleBallController addedBall)
         {
             if (_balls.FirstOrDefault(x => x == addedBall) == null)
             {
                 _balls.Add(addedBall);
             }
         }
        
         private void RemoveBallFromTracking(SingleBallController ballToRemove)
         {
             if (!_balls.Contains(ballToRemove)) return;
             _balls.Remove(ballToRemove);
             Destroy(ballToRemove.gameObject);
             if (_balls.Count <= 0)
             {
                 OnOnBallsListEmpty();
             }
         }

         #endregion

         #region Helper Methods

         private Vector2 GetRandomForce()
         {
             return new Vector2(Random.Range(GameController.Config.MinimumForceVector.x, 
                 GameController.Config.MaximumForceVector.x),Random.Range(GameController.Config.MinimumForceVector.y,
                 GameController.Config.MaximumForceVector.y));
         }

         private void EmptyBallsList()
         {
             foreach (var item in _balls)
             {
                 item.OnBallPopped -= OnBallPopped;
                 if(item !=null) Destroy(item.gameObject);
             }
             _balls.Clear();
         }

         #endregion
         
        #region Event Invokers

        protected virtual void OnOnBallsListEmpty()
        {
            OnBallsListEmpty?.Invoke();
        }

        #endregion

        #region Event Listeners

        private void OnGameWon()
        {
            EmptyBallsList();
        }

        private void OnLevelWon()
        {
            EmptyBallsList();
        }

        private void OnLevelLost()
        {
            EmptyBallsList();
        }

        private void OnLevelStarted(int level)
        {
            SpawnBalls(GameController.CurrentLevelConfig.BallAmount,GameController.CurrentLevelConfig.FirstBallSize);
        }
        
        private void OnBallPopped(SingleBallController poppedball)
        {
            var ballScale = poppedball.GetComponent<Transform>().localScale.x;
            if (ballScale >= GameController.Config.MinimumBallSize)
            {
                SplitBall(ballScale, poppedball.GetComponent<Rigidbody2D>().position);
            }
            RemoveBallFromTracking(poppedball);
            poppedball.OnBallPopped -= OnBallPopped;
        }

        #endregion

        private void OnDisable()
        {
            EmptyBallsList();
            GameController.OnLevelStarted -= OnLevelStarted;
            GameController.OnLevelLost -= OnLevelLost;
            GameController.OnLevelWon -= OnLevelWon;
            GameController.OnGameWon -= OnGameWon;
        }
    }
}