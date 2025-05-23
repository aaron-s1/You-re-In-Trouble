using UnityEngine;
using CodeMonkey.HealthSystemCM;


namespace CodeMonkey.HealthSystemCM {
    public class HealthSystemComponent : MonoBehaviour, IGetHealthSystem
    {

        [Tooltip("Maximum Health amount")]
        [SerializeField] private float healthAmountMax = 100f;

        [Tooltip("Starting Health amount, leave at 0 to start at full health.")]
        [SerializeField] private float startingHealthAmount;

        private HealthSystem healthSystem;


        private void Awake() {
            healthSystem = new HealthSystem(healthAmountMax);

            if (startingHealthAmount != 0)
                healthSystem.SetHealth(startingHealthAmount);
        }

        /// <summary>
        /// Get the Health System created by this Component
        /// </summary>
        public HealthSystem GetHealthSystem() => healthSystem;

        public void Damage(float amount) 
        {
            Debug.Log("damage??");
            healthSystem.Damage(amount);
        }
    }

}