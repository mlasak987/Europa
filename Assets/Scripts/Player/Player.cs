using Europa.Inventory;
using Europa.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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

        public bool[] UnlockedLore;

        public float Oxygen { get; private set; }

        [HideInInspector] public bool CanBreath = true;

        [Header("Game")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject tabMenu;
        [SerializeField] private GameObject uiMenu;
        [SerializeField] public bool DefaultWater;
        [SerializeField] private Vector3 startPos = new(0, 1, 0);
        [Header("Tab")]
        [SerializeField] private GameObject[] tabMenus;
        [Header("Stats")]
        [SerializeField] private float maxOxygen = 100f;
        [Header("UI")]
        [SerializeField] private Image oxygenSlider;
        [SerializeField] private TMP_Text status;

        private GameObject pickUpObject = null;
        private bool canOverrideStatus = true;
        private Container openContainer = null;

        private bool die = false;

        private void Awake()
        {
            Singleton = this;
            tabMenu.SetActive(true);
            tabMenus[0].SetActive(true);
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
            if (die)
            {
                MainMenuManager.Singleton.BrigthnessProfile.TryGet(out ColorAdjustments colorAdjustments);
                colorAdjustments.postExposure.value -= 0.01f;

                if (colorAdjustments.postExposure.value <= -5f)
                {
                    transform.position = startPos;
                    MainMenuManager.Singleton.UpdateSlidersTextsValues();

                    status.text = "";
                    Oxygen = maxOxygen;
                }

                return;
            }

            if (tabMenus[1].activeInHierarchy && Input.GetKeyDown(KeyCode.F))
            {
                tabMenu.SetActive(false);
                SelectSideGUI(0);
                uiMenu.SetActive(true);
                GamePaused = false;
                CamController.CursorUnlock(false);

                openContainer.SaveSlots();
                return;
            }

            if (GamePaused) return;

            if (pickUpObject != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Inventory.Inventory.Singleton.ItemsCount >= Inventory.Inventory.Singleton.MaxItems) { SetStatus("Your inventory is full!", Color.red, false, 1); return; }
                Inventory.Inventory.Singleton.PickUpItem(pickUpObject);
                DisablePickUp();
            }
            else if (openContainer != null && Input.GetKeyDown(KeyCode.F))
            {
                tabMenu.SetActive(true);
                SelectSideGUI(1);
                uiMenu.SetActive(false);
                GamePaused = true;
                CamController.CursorUnlock(true);
                SetStatus("");

                openContainer.LoadSlots();
            }
        }

        private void SelectSideGUI(int index)
        {
            foreach (var obj in tabMenus) obj.SetActive(false);
            tabMenus[index].SetActive(true);
        }

        public void Die()
        {
            die = true;
            GamePaused = true;
            CamController.CursorUnlock(false);
        }

        public void Pause(bool tab)
        {
            if (uiMenu != null) uiMenu.SetActive(false);
            if (pauseMenu != null) { if (tab) pauseMenu.SetActive(false); else pauseMenu.SetActive(true); }
            if (tabMenu != null) { if (tab) { tabMenu.SetActive(true); SelectSideGUI(0); } else tabMenu.SetActive(false); }
            if (CamController != null) CamController.PauseMenu(true);
            GamePaused = true;
            SetStatus("");
        }

        public void Resume()
        {
            if (tabMenus[1].activeInHierarchy && openContainer != null) openContainer.SaveSlots();

            if (uiMenu != null) uiMenu.SetActive(true);
            if (pauseMenu != null) pauseMenu.SetActive(false);
            if (tabMenu != null) tabMenu.SetActive(false);
            if (CamController != null) CamController.PauseMenu(false);
            GamePaused = false;
        }

        public void InteractWithContainer(Container locker)
        {
            SetStatus("Open Locker (F)");
            openContainer = locker;
        }

        public void DisableInteractionWithContainer()
        {
            SetStatus("");
            openContainer = null;
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
