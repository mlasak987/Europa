using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Europa.Language;
using Europa.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Europa.Utils
{
	public class MainMenuManager : MonoBehaviour
	{
		private static MainMenuManager _singleton;
		public static MainMenuManager Singleton
		{
			get => _singleton;
			private set
			{
				if (_singleton == null) _singleton = value;
				else if (_singleton != value)
				{
					Debug.Log($"{nameof(MainMenuManager)} instance already exists, destroying duplicate!");
					Destroy(value);
				}
			}
		}

		[Header("Graphic Settings")]
		[SerializeField] private Slider brightnessSlider;
		[SerializeField] private TMP_Text brightnessText;
		[SerializeField] private Toggle fullscreenToggle;
		[SerializeField] private TMP_Dropdown resolutionDropdown;
		[SerializeField] private TMP_Dropdown qualityDropdown;
		[SerializeField] private VolumeProfile brigthnessProfile;

		[Header("Audio Settings")]
		[SerializeField] private Slider masterVolumeSlider;
		[SerializeField] private Slider musicVolumeSlider;
		[SerializeField] private Slider effectsVolumeSlider;
		[Space(10)]
		[SerializeField] private TMP_Text masterVolumeText;
		[SerializeField] private TMP_Text musicVolumeText;
		[SerializeField] private TMP_Text effectsVolumeText;

		[Header("Gameplay Settings")]
		[SerializeField] private Slider controllerSensitivitySlider;
		[SerializeField] private TMP_Text controllerSensitivityText;
		[Space(10)]
		[SerializeField] private TMP_Dropdown languageDropdown;

		[Header("Join")]
		[SerializeField] private GameObject mainMenu;
		[Space(10)]
		[SerializeField] private GameObject loadingMenu;
		[SerializeField] private GameObject loadingButton;
        [SerializeField] private GameObject loadingProgressBar;
        [SerializeField] private Image loadingProgressBarFill;
        [SerializeField] private TMP_Text loadingText;

		[Header("Save")]
		[SerializeField] private GameObject prefab;
        [SerializeField] private GameObject prefab2;
        [SerializeField] private GameObject mainLoadMenu;
		[SerializeField] private GameObject exitConfirmation = null;
		[SerializeField] private GameObject saveMenu = null;

        private List<Resolution> resolutions = new();
		private int defaultResolutionIndex = 0;
		private ColorAdjustments colorAdjustments;

		private string systemLanguage;

		private void Start()
		{
			CalculateResolutions();

			brigthnessProfile.TryGet(out colorAdjustments);

			systemLanguage = Application.systemLanguage switch
			{
				SystemLanguage.English => "EN",
				SystemLanguage.Polish => "PL",
				_ => "EN",
			};

			PlayerPrefsInit();
		}

        private void Awake()
        {
			Singleton = this;
        }

        private void PlayerPrefsInit()
        {
			if (!PlayerPrefs.HasKey("brightness")) PlayerPrefs.SetFloat("brightness", 50);
			if (!PlayerPrefs.HasKey("quality")) PlayerPrefs.SetInt("quality", 1);
			if (!PlayerPrefs.HasKey("fullscreen")) PlayerPrefs.SetInt("fullscreen", 1);
			if (!PlayerPrefs.HasKey("resolution")) PlayerPrefs.SetInt("resolution", defaultResolutionIndex);

			if (!PlayerPrefs.HasKey("masterVolume")) PlayerPrefs.SetFloat("masterVolume", 100);
			if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 100);
			if (!PlayerPrefs.HasKey("effectsVolume")) PlayerPrefs.SetFloat("effectsVolume", 100);

			if (!PlayerPrefs.HasKey("mouseSensitivity")) PlayerPrefs.SetFloat("mouseSensitivity", 100);
			if (!PlayerPrefs.HasKey("language")) PlayerPrefs.SetString("language", systemLanguage);

			brightnessSlider.value = PlayerPrefs.GetFloat("brightness");
			qualityDropdown.value = PlayerPrefs.GetInt("quality");
			fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
			resolutionDropdown.value = PlayerPrefs.GetInt("resolution");

			masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
			musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
			effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume");

			controllerSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensitivity");
			languageDropdown.value = PlayerPrefs.GetString("language") switch
			{
				"EN" => 0,
				"PL" => 1,
				"SZL" => 2,
				_ => 0
			};
			UpdateSlidersTextsValues();
			ApplyAll();
		}

		private void CalculateResolutions()
		{
			Resolution[] allResolutions;
			allResolutions = Screen.resolutions;

			List<string> options = new();

			int currentResolutionIndex = 0;
			string previousOption = "0 x 0";

			for (int i = 0; i < allResolutions.Length; i++)
			{
				string option = $"{allResolutions[i].width} x {allResolutions[i].height}";
				if (option == previousOption) continue;

				options.Add(option);
				resolutions.Add(allResolutions[i]);

				if (allResolutions[i].width == Screen.width && allResolutions[i].height == Screen.height)
				{
					currentResolutionIndex = i;
					defaultResolutionIndex = i;
				}

				previousOption = $"{allResolutions[i].width} x {allResolutions[i].height}";
			}

			resolutionDropdown.ClearOptions();
			resolutionDropdown.AddOptions(options);
			resolutionDropdown.value = currentResolutionIndex;
			resolutionDropdown.RefreshShownValue();
		}

		public void UpdateSlidersTextsValues()
        {
			brightnessText.text = brightnessSlider.value.ToString();
			masterVolumeText.text = masterVolumeSlider.value.ToString();
			musicVolumeText.text = musicVolumeSlider.value.ToString();
			effectsVolumeText.text = effectsVolumeSlider.value.ToString();
			controllerSensitivityText.text = controllerSensitivitySlider.value.ToString();

			if (brightnessSlider.value > 50) colorAdjustments.postExposure.value = (brightnessSlider.value - 50) / 25f;
			else if ((brightnessSlider.value < 50)) colorAdjustments.postExposure.value = -(50 - brightnessSlider.value) / 25f;
            else colorAdjustments.postExposure.value = 0f;
		}

		public void ApplyAll()
        {
			ApplySettingsGraphics();
			ApplySettingsSound();
			ApplySettingsGameplay();
        }

		public void ApplySettingsGraphics()
        {
			PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
			PlayerPrefs.SetInt("quality", qualityDropdown.value);
			PlayerPrefs.SetInt("fullscreen", fullscreenToggle.isOn ? 1 : 0);
			PlayerPrefs.SetInt("resolution", resolutionDropdown.value);

			QualitySettings.SetQualityLevel(qualityDropdown.value);
			Screen.fullScreen = fullscreenToggle.isOn;

			Resolution resolution = resolutions[resolutionDropdown.value];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}
		public void ApplySettingsSound()
		{
			PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
			PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
			PlayerPrefs.SetFloat("effectsVolume", effectsVolumeSlider.value);
		}

		public void ApplySettingsGameplay()
		{
			PlayerPrefs.SetFloat("mouseSensitivity", controllerSensitivitySlider.value);

			string language = languageDropdown.value switch
			{
				0 => "EN",
				1 => "PL",
                2 => "SZL",
                _ => "EN",
			};

			PlayerPrefs.SetString("language", language);

			LanguageManager.LoadLanguage(language);
		}


		public void ResetToDefault()
        {
			brightnessSlider.value = 50;
			resolutionDropdown.value = defaultResolutionIndex;
			qualityDropdown.value = 1;
			fullscreenToggle.isOn = true;

			masterVolumeSlider.value = 100;
			musicVolumeSlider.value = 100;
			effectsVolumeSlider.value = 100;

			controllerSensitivitySlider.value = 100;
			languageDropdown.value = systemLanguage switch
			{
				"EN" => 0,
				"PL" => 1,
				"SZL" => 2,
				_ => 0
			};

			UpdateSlidersTextsValues();
			ApplyAll();
		}

		public void LoadScene(int sceneId)
        {
            loadingText.text = LanguageManager.GetTranslation("MENU.STATUS.LOADING");
            StartCoroutine(LoadSceneAsync(sceneId));
		}

        IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            loadingMenu.SetActive(true);

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01((float)operation.progress / 0.9f);
                loadingProgressBarFill.fillAmount = progressValue;

                yield return null;
            }
        }

        public void BackToMainMenu()
        {
			if (loadingMenu.activeInHierarchy)
            {
				loadingText.text = LanguageManager.GetTranslation("MENU.STATUS.LOADING");
				loadingButton.SetActive(false);
                loadingProgressBar.SetActive(true);
                loadingMenu.SetActive(false);
			}
			mainMenu.SetActive(true);
		}

		public void HandleError(string text)
        {
			if (!loadingMenu.activeInHierarchy) loadingMenu.SetActive(true);
			loadingButton.SetActive(true);
			loadingProgressBar.SetActive(false);
			loadingText.text = LanguageManager.GetTranslation($"MENU.STATUS.{text}");
		}

		public void LoadingSaveGameWindow(TMP_InputField input)
		{
			if (SaveSystem.SaveSystem.SaveName != null) input.text = SaveSystem.SaveSystem.SaveName;
        }

		public void SaveGame(TMP_InputField input)
		{
			if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Map")) return;
            if (!GameObject.Find("Components").TryGetComponent<GameManager>(out GameManager gameManager)) Debug.LogError($"GameManager not initialized corectly!");
			if (input.text == null) return;
            SaveSystem.SaveSystem.SaveGame(input.text, gameManager);
		}

        public void SaveExitGame(TMP_InputField input)
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Map")) return;
            if (!GameObject.Find("Components").TryGetComponent<GameManager>(out GameManager gameManager)) Debug.LogError($"GameManager not initialized corectly!");
            if (input.text == null) return;
            SaveSystem.SaveSystem.SaveGame(input.text, gameManager);
			LoadScene(0);
        }

		public void GenerateListOfSaves(GameObject parent)
		{
            string path = Application.persistentDataPath + $"/saves/";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

			if (Directory.GetFiles(path, "*.dat").OrderByDescending(File.GetLastWriteTime).ToList().Count() == 0)
			{
                Instantiate(prefab2, parent.transform);
                return;
			}

            foreach (var file in Directory.GetFiles(path, "*.dat").OrderByDescending(File.GetLastWriteTime).ToList())
			{
				string name = Path.GetFileName(file);
				name = name[..name.LastIndexOf('.')];
                SaveObject obj = Instantiate(prefab, parent.transform).GetComponent<SaveObject>();
				obj.saveName.text = name;
				obj.saveDate.text = File.GetLastWriteTime(file).ToString();
				UnityAction action = () => LoadSave(name);
                action += LoadMenuOff;
                obj.playButton.onClick.AddListener(action);
			}
        }

        private void LoadMenuOff()
		{
			mainLoadMenu.SetActive(false);
		}


        private void LoadSave(string name)
		{
			SaveSystem.SaveSystem.SaveName = name;
			LoadScene(1);
		}

		public void ExitToMainMenu(bool save = false)
		{
			if (save && SaveSystem.SaveSystem.SaveName != null)
			{
                if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Map")) return;
                if (!GameObject.Find("Components").TryGetComponent<GameManager>(out GameManager gameManager)) Debug.LogError($"GameManager not initialized corectly!");
                SaveSystem.SaveSystem.SaveGame(SaveSystem.SaveSystem.SaveName, gameManager);
                SaveSystem.SaveSystem.SaveName = null;
                LoadScene(0);
            }
			else if (save)
			{
				exitConfirmation.SetActive(false);
                saveMenu.SetActive(true);
            }
			else LoadScene(0);
        }

        public void ExitToMainMenuInit()
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Map")) return;
            if (!GameObject.Find("Components").TryGetComponent<GameManager>(out GameManager gameManager)) Debug.LogError($"GameManager not initialized corectly!");
			if ((DateTime.Now - SaveSystem.SaveSystem.lastSave).Seconds <= 15 && gameManager.GameId == SaveSystem.SaveSystem.GameId)
			{
                SaveSystem.SaveSystem.SaveName = null;
                LoadScene(0);
			}
			else exitConfirmation.SetActive(true);
        }

        public void Exit()
        {
			Application.Quit();
        }
    }
}
