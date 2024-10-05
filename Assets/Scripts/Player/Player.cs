using Europa.Inventory;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Europa.Player
{
    [RequireComponent(typeof(MovementController))]
    public class Player : MonoBehaviour
    {
        private static Player _singleton;
        public static Player Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null) _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(Player)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }
        public bool GamePaused { get; private set; }
        public CameraController CamController { get; private set; }
        public MovementController MovController { get; private set; }

        public float Oxygen { get; private set; }

        [HideInInspector] public bool CanBreath = true;

        [Header("Game")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject tabMenu;
        [SerializeField] private GameObject uiMenu;
        [Header("Stats")]
        [SerializeField] private float maxOxygen = 100f;
        [Header("UI")]
        [SerializeField] private Image oxygenSlider;
        [SerializeField] private TMP_Text status;

        private GameObject pickUpObject = null;
        private bool canOverrideStatus = true;

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            CamController = GetComponentInChildren<CameraController>();
            MovController = GetComponent<MovementController>();
            status.text = "";
            Oxygen = maxOxygen;
        }

        private void FixedUpdate()
        {
            if (GamePaused) return;

            if (CanBreath && Oxygen < maxOxygen) Oxygen += (maxOxygen / (maxOxygen * 2f));
            else Oxygen -= (maxOxygen / 6000f);

            oxygenSlider.fillAmount = Oxygen / maxOxygen;
        }

        private void Update()
        {
            if (pickUpObject != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Inventory.Inventory.Singleton.Items.Count >= Inventory.Inventory.Singleton.MaxItems) { SetStatus("Your inventory is full!", Color.red, false, 1); return; }
                Inventory.Inventory.Singleton.PickUpItem(pickUpObject);
                DisablePickUp();
            }
        }

        public void Pause(bool tab)
        {
            if (uiMenu != null) uiMenu.SetActive(false);
            if (pauseMenu != null) { if (tab) pauseMenu.SetActive(false); else pauseMenu.SetActive(true); }
            if (tabMenu != null) { if (tab) tabMenu.SetActive(true); else tabMenu.SetActive(false); }
            if (CamController != null) CamController.PauseMenu(true);
            GamePaused = true;
        }

        public void Resume()
        {
            if (uiMenu != null) uiMenu.SetActive(true);
            if (pauseMenu != null) pauseMenu.SetActive(false);
            if (tabMenu != null) tabMenu.SetActive(false);
            if (CamController != null) CamController.PauseMenu(false);
            GamePaused = false;
        }

        public void EnablePickUp(GameObject gameObject)
        {
            SetStatus("Pick up (LMB)");
            pickUpObject = gameObject;
        }

        public void DisablePickUp()
        {
            SetStatus("");
            pickUpObject = null;
        }

        public void SetStatus(string status, Color color, bool canOverride, float time)
        {
            if (!canOverrideStatus) return;
            this.status.text = status;
            this.status.color = color;
            canOverrideStatus = canOverride;
            StartCoroutine(AutoDisableStatus(time));
        }

        public void SetStatus(string status, Color color, bool canOverride)
        {
            if (!canOverrideStatus) return;
            this.status.text = status;
            this.status.color = color;
            canOverrideStatus = canOverride;
        }

        public void SetStatus(string status, Color color)
        {
            if (!canOverrideStatus) return;
            this.status.text = status;
            this.status.color = color;
            canOverrideStatus = true;
        }

        public void SetStatus(string status)
        {
            if (!canOverrideStatus) return;
            this.status.text = status;
            this.status.color = Color.white;
            canOverrideStatus = true;
        }

        IEnumerator AutoDisableStatus(float time)
        {
            yield return new WaitForSeconds(time);

            canOverrideStatus = true;
            status.color = Color.white;
            status.text = "";
        }
    }
}
