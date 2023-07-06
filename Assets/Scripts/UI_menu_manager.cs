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
    private Button _buttonExit;

    [SerializeField]
    private VisualTreeAsset _settingsTemplate;
    private VisualElement _buttonsSettings;

    private VisualElement _menu;
    private VisualElement[] _mainMenuOptions;

    private const string POPUP_ANIMATION = "pop-animation-hide";
    private int _mainPopupIndex = -1;

    private Slider _volumeSlider;

    private void Awake() {

        _doc = GetComponent<UIDocument>();

        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

        var root = GetComponent<UIDocument>().rootVisualElement;
        _menu = root.Q<VisualElement>("MenuSection");
        _mainMenuOptions = _menu.Q<VisualElement>("Buttons").Children().ToArray();
        _menu.RegisterCallback<TransitionEndEvent>(Menu_TransitionEnd);

        _buttonPlay = _doc.rootVisualElement.Q<Button>("ButtonPlay");
        _buttonSettings = _doc.rootVisualElement.Q<Button>("ButtonSettings");    
        _buttonExit = _doc.rootVisualElement.Q<Button>("ButtonExit");

        _buttonsSettings = _settingsTemplate.CloneTree();
        var buttonBack = _buttonsSettings.Q<Button>("ButtonBack");
        var volumeSlider = _buttonsSettings.Q<Slider>("AmbientSoundSlider");

        _buttonPlay.clicked +=  ButtonPlay_clicked;
        _buttonSettings.clicked += ButtonSettings_clicked;
        _buttonExit.clicked += ButtonExit_clicked;
        buttonBack.clicked += ButtonBack_clicked;

        volumeSlider.RegisterValueChangedCallback(v=>{
            var newValue = v.newValue;
            AudioListener.volume = newValue;
        });
    }

    private IEnumerator Start() {
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
    private void ButtonPlay_clicked(){
        SceneManager.LoadScene("TileScene",LoadSceneMode.Single);
    }

    private void ButtonSettings_clicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_buttonsSettings);

        //SceneManager.LoadScene("SettingsScene",LoadSceneMode.Single);
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
        _buttonsWrapper.Add(_buttonExit);   
    } 
}
