using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Config/" + nameof(GameConfig), fileName = nameof(GameConfig))]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private LevelConfig[] levels;
        
        [Header("Setup")]
        [SerializeField] private int desiredFrameRate;
        [SerializeField] private float playerSpeed;
        [SerializeField] private float weaponSpeed;
        [SerializeField] private float minimumBallSize;
        [SerializeField] private Vector2 minimumForceVector;
        [SerializeField] private Vector2 maximumForceVector;
        
        [Header("Constants")]
        [SerializeField] private string ballTagName;
        [SerializeField] private string menuSceneName;
        [SerializeField] private string gameSceneName;
        [SerializeField] private string endSceneName;

        public LevelConfig[] Levels => levels;
        public int DesiredFrameRate => desiredFrameRate;
        public float PlayerSpeed => playerSpeed;
        public float WeaponSpeed => weaponSpeed;
        public float MinimumBallSize => minimumBallSize;
        public Vector2 MinimumForceVector => minimumForceVector;
        public Vector2 MaximumForceVector => maximumForceVector;
        public string BallTagName => ballTagName;
        public string MenuSceneName => menuSceneName;
        public string GameSceneName => gameSceneName;
        public string EndSceneName => endSceneName;
    }
}
