using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UI_game_manager : MonoBehaviour
{
    private UIDocument _doc;

    private VisualElement _gameUIWrapper;
    private Button _buttonPause;

    [SerializeField]
    private VisualTreeAsset _pauseTemplate;
    private VisualElement _pauseMenu;
    [SerializeField]
    private VisualTreeAsset _settingsTemplate;
    private VisualElement _buttonsSettings;


    private bool stop;
    public Timer timer;

    private void Awake() {
        _doc = GetComponent<UIDocument>();

        _gameUIWrapper = _doc.rootVisualElement.Q<VisualElement>("GameUI");
        _buttonPause = _doc.rootVisualElement.Q<Button>("ButtonPause");

        _buttonPause.clicked += ButtonPause_clicked;
        
        _buttonsSettings = _settingsTemplate.CloneTree();

        _pauseMenu = _pauseTemplate.CloneTree();
        var buttonResume = _pauseMenu.Q<Button>("ButtonResume");
        var buttonSettings = _pauseMenu.Q<Button>("ButtonSettings");    
        var buttonBackMenu = _pauseMenu.Q<Button>("ButtonBack");

        buttonResume.clicked += ButtonResume_clicked;
        buttonSettings.clicked += ButtonSettings_clicked;
        buttonBackMenu.clicked += ButtonBackMenu_clicked;

        
        var buttonBack = _buttonsSettings.Q<Button>("ButtonBack");
        var volumeSlider = _buttonsSettings.Q<Slider>("AmbientSoundSlider");

        buttonBack.clicked += ButtonBack_clicked;

        volumeSlider.RegisterValueChangedCallback(v=>{
            var newValue = v.newValue;
            AudioListener.volume = newValue;
        }); 


    }

    private void ButtonPause_clicked(){
        _gameUIWrapper.Clear();
        _gameUIWrapper.Add(_pauseMenu);
        timer.stopTimer(false);
    }

    private void ButtonResume_clicked(){
        _gameUIWrapper.Clear();
        _gameUIWrapper.Add(_buttonPause);
        timer.stopTimer(false);
    }

    private void ButtonSettings_clicked(){
        _gameUIWrapper.Clear();
        _gameUIWrapper.Add(_buttonsSettings);
    }

    private void ButtonBackMenu_clicked(){
        SceneManager.LoadScene("MenuScene",LoadSceneMode.Single);
    }

    private void ButtonBack_clicked(){
        _gameUIWrapper.Clear();
        _gameUIWrapper.Add(_pauseMenu);
    }

    // Start is called before the first frame update
    void Start()
    {
        //timer = GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            this.ButtonPause_clicked();
            timer.stopTimer(true);
            //_gameUIWrapper.Add(_pauseMenu);
        }
    }
}
