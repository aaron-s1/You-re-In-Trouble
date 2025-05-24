using UnityEngine;
using CodeMonkey.HealthSystemCM;

namespace CodeMonkey.HealthSystemCM {
    public class HealthSystemComponent : MonoBehaviour, IGetHealthSystem
    {
        [SerializeField] private float healthAmountMax = 100f;

        [Tooltip("Leave at 0 to start at full health.")]
        [SerializeField] private float startingHealthAmount;

        HealthSystem healthSystem;

        void Awake()
        {
            healthSystem = new HealthSystem(healthAmountMax);

            if (startingHealthAmount != 0)
                healthSystem.SetHealth(startingHealthAmount);
        }

        public void Damage(float amount) =>
            healthSystem.Damage(amount);


        public HealthSystem GetHealthSystem() =>
            healthSystem;

    }
}