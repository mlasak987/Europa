using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Europa.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float clamp = 90f;
        [Header("Water post processing")]
        [SerializeField] private VolumeProfile surfaceProfile;
        [SerializeField] private VolumeProfile underwaterProfile;
        [SerializeField] private GameObject[] underwaterBackgrounds;

        [HideInInspector] public Camera Cam { get; private set; }

        private float xRot = 0f;

        private Volume postProcessing;
        private VolumeProfile lastProfile;

        private void Start()
        {
            postProcessing = GetComponent<Volume>();
            Cam = GetComponent<Camera>();
            PauseMenu(false);

            if (Player.Singleton.DefaultWater)
            {
                RenderSettings.fog = true;
                postProcessing.profile = underwaterProfile;

                Player.Singleton.CanBreath = false;
                Player.Singleton.MovController.IsCompletelyUnderwater = true;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { if (Player.Singleton.GamePaused) Player.Singleton.Resume(); else Player.Singleton.Pause(false); }
            else if (Input.GetKeyDown(KeyCode.Tab)) { if (Player.Singleton.GamePaused) Player.Singleton.Resume(); else Player.Singleton.Pause(true); }
            if (Player.Singleton.GamePaused) return;

            float mouseX = Input.GetAxis("Mouse X") * (PlayerPrefs.GetFloat("mouseSensitivity") + 50) * Time.deltaTime * 5f;
            float mouseY = Input.GetAxis("Mouse Y") * (PlayerPrefs.GetFloat("mouseSensitivity") + 50) * Time.deltaTime * 5f;
            xRot = Mathf.Clamp(xRot - mouseY, -clamp, clamp);

            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            Player.Singleton.transform.Rotate(Vector3.up * mouseX);
        }

        public void PauseMenu(bool state)
        {
            if (postProcessing.profile == underwaterProfile) { postProcessing.profile = surfaceProfile; foreach (var val in underwaterBackgrounds) val.SetActive(true); lastProfile = underwaterProfile; }
            else if (lastProfile == underwaterProfile) { postProcessing.profile = underwaterProfile; foreach (var val in underwaterBackgrounds) val.SetActive(false); lastProfile = null; }
            CursorUnlock(state);
        }

        public void CursorUnlock(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                RenderSettings.fog = true;
                postProcessing.profile = underwaterProfile;

                Player.Singleton.CanBreath = false;
                Player.Singleton.MovController.IsCompletelyUnderwater = true;
            }
            else if (other.CompareTag("Air"))
            {
                RenderSettings.fog = false;
                postProcessing.profile = surfaceProfile;

                Player.Singleton.CanBreath = true;
                Player.Singleton.MovController.IsCompletelyUnderwater = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Water"))
            {
                RenderSettings.fog = false;
                postProcessing.profile = surfaceProfile;

                Player.Singleton.CanBreath = true;
                Player.Singleton.MovController.IsCompletelyUnderwater = false;
            }
            else if (other.CompareTag("Air"))
            {
                RenderSettings.fog = true;
                postProcessing.profile = underwaterProfile;

                Player.Singleton.CanBreath = false;
                Player.Singleton.MovController.IsCompletelyUnderwater = true;
            }
        }
    }
}
