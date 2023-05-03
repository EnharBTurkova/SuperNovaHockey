namespace Masomo.ArenaStrikers.Config
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "BallConfig", menuName = "ArenaStrikers/Configs/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        public float MaxSpeed;
        public float Friction;
        public float Bounciness;
        public float Mass;
        public float Radius;
    }
}


