using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LevelConfig
    {
        [SerializeField] private float time;
        [SerializeField] private int ballAmount;
        [SerializeField] private int firstBallSize;
        [SerializeField] private AudioClip levelMusic;
        [SerializeField] private Sprite levelBackground;
        
        public float Time => time;
        public int BallAmount => ballAmount;
        public int FirstBallSize => firstBallSize;
        public AudioClip LevelMusic => levelMusic;
        public Sprite LevelBackground => levelBackground;
    }
}
