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
    private Button _buttonCredits;

    [SerializeField]
    private VisualTreeAsset _settingsTemplate;
    private VisualElement _buttonsSettings;

    [SerializeField]
    private VisualTreeAsset _highScoresTemplate;
    private VisualElement _highScores;

    [SerializeField]
    private VisualTreeAsset _creditsTemplate;
    private VisualElement _credits;


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
        _buttonCredits = _doc.rootVisualElement.Q<Button>("infoButton");

        _buttonsSettings = _settingsTemplate.CloneTree();
        var buttonBack = _buttonsSettings.Q<Button>("ButtonBack");
        var musicVolumeSlider = _buttonsSettings.Q<Slider>("AmbientSoundSlider");
        var effectsVolumeSlider = _buttonsSettings.Q<Slider>("SoundEffectsSlider");


        _highScores = _highScoresTemplate.CloneTree();
        var buttonBack2 = _highScores.Q<Button>("ButtonBack");

        _credits = _creditsTemplate.CloneTree();
        var buttonBack3 = _credits.Q<Button>("ButtonBack");

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
        _buttonCredits.clicked += ButtonCredits_clicked;

        buttonBack.clicked += ButtonBack_clicked;

        buttonBack2.clicked += ButtonBack_clicked;

        buttonBack3.clicked += ButtonBack_clicked;

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
    }

    private void ButtonCredits_clicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_credits);
    }

    private void ButtonHighScores_clicked(){
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_highScores);

        if(!PlayerPrefs.HasKey("FirstName"))
        {
            PlayerPrefs.SetString("FirstName","--------");
            PlayerPrefs.SetInt("FirstScore",0);
        }
        if(!PlayerPrefs.HasKey("SecondName"))
        {
            PlayerPrefs.SetString("SecondName","--------");
            PlayerPrefs.SetInt("SecondScore",0);
        }
        if(!PlayerPrefs.HasKey("ThirdName"))
        {
            PlayerPrefs.SetString("ThirdName","--------");
            PlayerPrefs.SetInt("ThirdScore",0);
        }
        if(!PlayerPrefs.HasKey("FourthName"))
        {
            PlayerPrefs.SetString("FourthName","--------");
            PlayerPrefs.SetInt("FourthScore",0);
        }
        if(!PlayerPrefs.HasKey("FifthName"))
        {
            PlayerPrefs.SetString("FifthName","--------");
            PlayerPrefs.SetInt("FifthScore",0);
        }

        _highScores.Q<Label>("First").text = PlayerPrefs.GetString("FirstName");
        _highScores.Q<Label>("Second").text = PlayerPrefs.GetString("SecondName");
        _highScores.Q<Label>("Third").text = PlayerPrefs.GetString("ThirdName");
        _highScores.Q<Label>("Fourth").text = PlayerPrefs.GetString("FourthName");
        _highScores.Q<Label>("Fifth").text = PlayerPrefs.GetString("FifthName");

        _highScores.Q<Label>("FirstVal").text = PlayerPrefs.GetInt("FirstScore").ToString("0");
        _highScores.Q<Label>("SecondVal").text = PlayerPrefs.GetInt("SecondScore").ToString("0");
        _highScores.Q<Label>("ThirdVal").text = PlayerPrefs.GetInt("ThirdScore").ToString("0");
        _highScores.Q<Label>("FourthVal").text = PlayerPrefs.GetInt("FourthScore").ToString("0");
        _highScores.Q<Label>("FifthVal").text = PlayerPrefs.GetInt("FifthScore").ToString("0");

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
        _buttonsWrapper.Add(_buttonCredits);
    }
}
