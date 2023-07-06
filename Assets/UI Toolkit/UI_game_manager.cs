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
    //private Button _buttonPause;

    [SerializeField]
    private VisualTreeAsset _pauseTemplate;
    private VisualElement _pauseMenu;
    [SerializeField]
    private VisualTreeAsset _settingsTemplate;
    private VisualElement _buttonsSettings;
    [SerializeField]
    private VisualTreeAsset _deadStats;
    private VisualElement _stats;

    private VisualElement[] _statsNames;
    private VisualElement[] _statsValues;

    private const string POPUP_ANIMATION = "pop-animation-hide";
    private int _mainPopupIndex = -1;

    private bool stop;
    public Timer timer;
    public GameStats gameStats;

    private void Awake() {
        _doc = GetComponent<UIDocument>();

        _gameUIWrapper = _doc.rootVisualElement.Q<VisualElement>("GameUI");
        //_buttonPause = _doc.rootVisualElement.Q<Button>("ButtonPause");

        //_buttonPause.clicked += ButtonPause_clicked;
        
        _buttonsSettings = _settingsTemplate.CloneTree();

        _pauseMenu = _pauseTemplate.CloneTree();
        var buttonResume = _pauseMenu.Q<Button>("ButtonResume");
        var buttonSettings = _pauseMenu.Q<Button>("ButtonSettings");    
        var buttonBackMenu = _pauseMenu.Q<Button>("ButtonBack");

        _stats = _deadStats.CloneTree();
        var buttonMenu = _stats.Q<Button>("MenuButton");
        
        /*_statsNames = _stats.Q<VisualElement>("Stats").Children().ToArray();
        _statsValues = _stats.Q<VisualElement>("Values").Children().ToArray();*/
        

        //_stats.RegisterCallback<TransitionEndEvent>(Stats_TransitionEnd);

        buttonResume.clicked += ButtonResume_clicked;
        buttonSettings.clicked += ButtonSettings_clicked;
        buttonBackMenu.clicked += ButtonBackMenu_clicked;
        

        
        var buttonBack = _buttonsSettings.Q<Button>("ButtonBack");
        var volumeSlider = _buttonsSettings.Q<Slider>("AmbientSoundSlider");

        buttonBack.clicked += ButtonBack_clicked;

        buttonMenu.clicked += ButtonBackMenu_clicked;

        volumeSlider.RegisterValueChangedCallback(v=>{
            var newValue = v.newValue;
            AudioListener.volume = newValue;
        }); 


    }

    private void Stats_TransitionEnd(TransitionEndEvent evt) 
    {
        if (!evt.stylePropertyNames.Contains("opacity")) { return; }
        if (_mainPopupIndex < _statsNames.Length - 1)
        {
            _mainPopupIndex++;
            _statsNames[_mainPopupIndex].ToggleInClassList(POPUP_ANIMATION);
        }
        if (_mainPopupIndex < _statsValues.Length - 1)
        {
            _mainPopupIndex++;
            _statsValues[_mainPopupIndex].ToggleInClassList(POPUP_ANIMATION);
        }
    }

    private void ButtonPause_clicked(){
        _gameUIWrapper.Clear();
        _gameUIWrapper.Add(_pauseMenu);
        timer.stopTimer(false);
    }

    private void ButtonResume_clicked(){
        _gameUIWrapper.Clear();
       // _gameUIWrapper.Add(_buttonPause);
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
        gameStats.enemiesKilled.zombies = 0;
        gameStats.enemiesKilled.bats = 0;
        gameStats.enemiesKilled.skeletons = 0;
        gameStats.enemiesKilled.crawlers = 0;
        gameStats.enemiesKilled.wraiths = 0;
        gameStats.enemiesKilled.flyingEyes = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(gameStats.player.PlayerHealth> 0){
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                //this.ButtonPause_clicked();
                _gameUIWrapper.Clear();
                _gameUIWrapper.Add(_pauseMenu);
                timer.stopTimer(true);
                //_gameUIWrapper.Add(_pauseMenu);
            }
        }else{
            _gameUIWrapper.Clear();
            _gameUIWrapper.Add(_stats);
            _stats.Q<Label>("ZombiesVal").text = gameStats.enemiesKilled.zombies.ToString("0");
            _stats.Q<Label>("BatsVal").text = gameStats.enemiesKilled.bats.ToString("0");
            _stats.Q<Label>("SkeletonsVal").text = gameStats.enemiesKilled.skeletons.ToString("0");
            _stats.Q<Label>("CrawlersVal").text = gameStats.enemiesKilled.crawlers.ToString("0");
            _stats.Q<Label>("TimeVal").text = timer.GetTime();
            //_stats.RegisterCallback<TransitionEndEvent>(Stats_TransitionEnd);
            //Stats_TransitionEnd(new TransitionEndEvent());
            //_stats.ToggleInClassList(POPUP_ANIMATION);
            
            timer.stopTimer(true);

        }
    }
}
