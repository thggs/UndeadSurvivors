using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Slider _volumeSlider;

    private void Awake() {

        _doc = GetComponent<UIDocument>();

        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

        

        _buttonPlay = _doc.rootVisualElement.Q<Button>("ButtonPlay");
        _buttonSettings = _doc.rootVisualElement.Q<Button>("ButtonSettings");    
        _buttonExit = _doc.rootVisualElement.Q<Button>("ButtonExit");

        _buttonsSettings = _settingsTemplate.CloneTree();
        var buttonBack = _buttonsSettings.Q<Button>("ButtonBack");
        var volumeSlider = _buttonsSettings.Q<Slider>("AmbientSoundSlider");

        _buttonSettings.clicked += ButtonSettings_clicked;
        _buttonExit.clicked += ButtonExit_clicked;
        buttonBack.clicked += ButtonBack_clicked;

        volumeSlider.RegisterValueChangedCallback(v=>{
            var newValue = v.newValue;
            AudioListener.volume = newValue;
        });
        
        
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
