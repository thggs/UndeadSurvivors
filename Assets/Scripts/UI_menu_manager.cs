using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UI_menu_manager : MonoBehaviour
{
    private UIDocument _doc;

    private VisualElement _buttonsWrapper;

    private Button _buttonPlay;
    private Button _buttonSettings;
    private Button _buttonHighScores;
    private Button _buttonExit;

    [SerializeField]
    private VisualTreeAsset _settingsTemplate;
    private VisualElement _buttonsSettings;

    [SerializeField]
    private VisualTreeAsset _highScoresTemplate;
    private VisualElement _highScores;

    private VisualElement _menu;
    private VisualElement[] _mainMenuOptions;

    private const string POPUP_ANIMATION = "pop-animation-hide";
    private int _mainPopupIndex = -1;

    private Slider _volumeSlider;

    private void Awake()
    {
        Time.timeScale = 1;


        _doc = GetComponent<UIDocument>();

        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

        var root = GetComponent<UIDocument>().rootVisualElement;
        _menu = root.Q<VisualElement>("MenuSection");
        _mainMenuOptions = _menu.Q<VisualElement>("Buttons").Children().ToArray();
        _menu.RegisterCallback<TransitionEndEvent>(Menu_TransitionEnd);

        _buttonPlay = _doc.rootVisualElement.Q<Button>("ButtonPlay");
        _buttonSettings = _doc.rootVisualElement.Q<Button>("ButtonSettings");
        _buttonHighScores = _doc.rootVisualElement.Q<Button>("ButtonScores");
        _buttonExit = _doc.rootVisualElement.Q<Button>("ButtonExit");

        _buttonsSettings = _settingsTemplate.CloneTree();
        var buttonBack = _buttonsSettings.Q<Button>("ButtonBack");
        var musicVolumeSlider = _buttonsSettings.Q<Slider>("AmbientSoundSlider");
        var effectsVolumeSlider = _buttonsSettings.Q<Slider>("SoundEffectsSlider");

        _highScores = _highScoresTemplate.CloneTree();
        var buttonBack2 = _highScores.Q<Button>("ButtonBack");

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 1.0f);
        }

        if (PlayerPrefs.HasKey("EffectsVolume"))
        {
            effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("EffectsVolume", 1.0f);
        }

        _buttonPlay.clicked += ButtonPlay_clicked;
        _buttonSettings.clicked += ButtonSettings_clicked;
        _buttonExit.clicked += ButtonExit_clicked;
        _buttonHighScores.clicked += ButtonHighScores_clicked;

        buttonBack.clicked += ButtonBack_clicked;

        buttonBack2.clicked += ButtonBack_clicked;

        musicVolumeSlider.RegisterValueChangedCallback(v =>
        {
            var newValue = v.newValue;
            PlayerPrefs.SetFloat("MusicVolume", newValue);
        });

        effectsVolumeSlider.RegisterValueChangedCallback(v =>
        {
            var newValue = v.newValue;
            PlayerPrefs.SetFloat("EffectsVolume", newValue);
        });
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        _menu.ToggleInClassList(POPUP_ANIMATION);
    }

    private void Menu_TransitionEnd(TransitionEndEvent evt)
    {
        if (!evt.stylePropertyNames.Contains("opacity")) { return; }
        if (_mainPopupIndex < _mainMenuOptions.Length - 1)
        {
            _mainPopupIndex++;
            _mainMenuOptions[_mainPopupIndex].ToggleInClassList(POPUP_ANIMATION);
        }
    }
    private void ButtonPlay_clicked()
    {
        SceneManager.LoadScene("TileScene", LoadSceneMode.Single);
    }

    private void ButtonSettings_clicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_buttonsSettings);

        //SceneManager.LoadScene("SettingsScene",LoadSceneMode.Single);
    }

    private void ButtonHighScores_clicked(){
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_highScores);

    }

    private void ButtonExit_clicked()
    {
        Application.Quit();
    }

    private void ButtonBack_clicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_buttonPlay);
        _buttonsWrapper.Add(_buttonSettings);
        _buttonsWrapper.Add(_buttonHighScores);
        _buttonsWrapper.Add(_buttonExit);
    }
}
