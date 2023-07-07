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
    [SerializeField]
    private VisualTreeAsset _upgradeButtonsTemplate;
    private VisualElement _upgradeButtons;

    private VisualElement[] _statsNames;
    private VisualElement[] _statsValues;

    private const string POPUP_ANIMATION = "pop-animation-hide";
    private int _mainPopupIndex = -1;

    private bool stop;
    public Timer timer;
    public GameStats gameStats;

    private int[] selectedInts;

    public List<Dictionary<int, string>> dictionariesList = new List<Dictionary<int, string>>();

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
        Time.timeScale = 1;
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
        Dictionaries();

    }

    // Update is called once per frame
    void Update()
    {
        if(gameStats.player.PlayerHealth> 0){
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                //this.ButtonPause_clicked();
                Time.timeScale = 0;
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

        if(gameStats.player.PlayerXP >= gameStats.player.PlayerLevel * 10 ){
            
            //_gameUIWrapper.Clear();
            //_gameUIWrapper.Add(_upgradeButtons);
            
            
            Upgrade();

        }
    }

    void Upgrade(){
        // List of posible upgrades

        
        gameStats.player.PlayerLevel++;
        gameStats.player.PlayerXP = 0;
        Time.timeScale = 0;

        List<int> upgrades = new List<int> { 0, 1, 2, 4, 5, 6, 7 };

        // Remove from list if max level
        if(gameStats.player.PlayerMaxHealthLevel == 7)
            upgrades.Remove(0);
        if(gameStats.healingStones.HealLevel == 7)
            upgrades.Remove(1);
        if(gameStats.player.PlayerSpeedLevel == 7)
            upgrades.Remove(2);
        if(gameStats.whip.WhipLevel == 9)
            upgrades.Remove(4);
        if(gameStats.bible.BibleLevel == 9)
            upgrades.Remove(5);
        if(gameStats.holyWater.WaterLevel == 9)
            upgrades.Remove(6);
        if(gameStats.throwingKnife.KnifeLevel == 9)
            upgrades.Remove(7);

        // Choose three different upgrades
        selectedInts = upgrades.OrderBy(x => Random.value).Take(3).ToArray();

        
        // need to add option to only put 2 or 1 buttons when upgrades.size() < 3 !!!

        // Add corresponding text to buttons
        string option1 = dictionariesList[selectedInts[0]][selectLevel(selectedInts[0])];
        string option2 = dictionariesList[selectedInts[1]][selectLevel(selectedInts[1])];
        string option3 = dictionariesList[selectedInts[2]][selectLevel(selectedInts[2])];

        _upgradeButtons=_upgradeButtonsTemplate.CloneTree();
        var upgradeButton1 = _upgradeButtons.Q<Button>("upgradeButton1");
        var upgradeButton2 = _upgradeButtons.Q<Button>("upgradeButton2");
        var upgradeButton3 = _upgradeButtons.Q<Button>("upgradeButton3");
        var imageButton1 = _upgradeButtons.Q<Label>("imageButton1");
        var imageButton2 = _upgradeButtons.Q<Label>("imageButton2");
        var imageButton3 = _upgradeButtons.Q<Label>("imageButton3");

        _gameUIWrapper.Clear();
        _gameUIWrapper.Add(_upgradeButtons);

        upgradeButton1.text = option1;
        upgradeButton2.text = option2;
        upgradeButton3.text = option3;

        // Add corresponding images to button
        StyleBackground styleBackground1 = new StyleBackground(SelectImage(selectedInts[0]));
        StyleBackground styleBackground2 = new StyleBackground(SelectImage(selectedInts[1]));
        StyleBackground styleBackground3 = new StyleBackground(SelectImage(selectedInts[2]));
        //styleBackground.texture = SelectImage(selectedInts[0]);
        imageButton1.style.backgroundImage = styleBackground1;
        imageButton2.style.backgroundImage = styleBackground2;
        imageButton3.style.backgroundImage = styleBackground3;

        

        upgradeButton1.clicked += upgradeButton1_clicked;
        upgradeButton2.clicked += upgradeButton2_clicked;
        upgradeButton3.clicked += upgradeButton3_clicked;
        
    }

    public int selectLevel(int option){
        switch(option){
            case 0: return(gameStats.player.PlayerMaxHealthLevel + 1);
            case 1: return (gameStats.healingStones.HealLevel + 1);
            case 2: return(gameStats.player.PlayerSpeedLevel + 1); 
            //case 3: break; XP Radius; break;
            case 4: return(gameStats.whip.WhipLevel + 1); 
            case 5: return(gameStats.bible.BibleLevel + 1); 
            case 6: return(gameStats.holyWater.WaterLevel + 1); 
            case 7: return(gameStats.throwingKnife.KnifeLevel + 1); 
            default: return 0;
        }   
    }

    private void upgradeButton1_clicked(){

        LevelUp(selectedInts[0]);

    }

    private void upgradeButton2_clicked(){

        LevelUp(selectedInts[1]);

    }

    private void upgradeButton3_clicked(){

        LevelUp(selectedInts[2]);

    }

    public void LevelUp(int option){
        // Max Health
        if(option == 0){ 
            gameStats.player.PlayerMaxHealth += 100; 
            gameStats.player.PlayerMaxHealthLevel += 1;
            gameStats.player.PlayerHealth += 100;
            Debug.Log("Level Up Max Health");
        }
        // Healing Stones
        if(option == 1){ 
            gameStats.healingStones.HealLevel += 1;
            gameStats.healingStones.HealAmount += 25;
            Debug.Log("Level Up Healing Stones");
        }
        // Player Speed
        if(option == 2){ 
            gameStats.player.PlayerSpeedLevel += 1;
            gameStats.player.PlayerSpeed += 0.5f;
            Debug.Log("Level Up Player Speed");
        }
        // XP Radius
        // Not Implemented
        // Whip
        if(option == 4){ 
            gameStats.whip.WhipLevel += 1;
            switch(gameStats.whip.WhipLevel){
                case 2: gameStats.whip.WhipDamage = 6.25f; break;
                case 3: gameStats.whip.WhipCooldown = 0.8f; break;
                case 4: gameStats.whip.WhipDamage = 8.0f; gameStats.whip.WhipProjectiles = 2; break;
                case 5: gameStats.whip.WhipCooldown = 0.7f; break;
                case 6: gameStats.whip.WhipDamage = 10.0f; break;
                case 7: gameStats.whip.WhipCooldown = 0.6f; break;
                case 8: gameStats.whip.WhipDamage = 12.5f; break;
                case 9: gameStats.whip.WhipDamage = 15.0f; gameStats.whip.WhipCooldown = 0.6f; break;
            }
            Debug.Log("Level Up Whip");
        }
        // Bible
        if(option == 5){ 
            gameStats.bible.BibleLevel += 1;
            switch(gameStats.bible.BibleLevel){
                case 1: gameStats.bible.BibleProjectiles = 1; break;
                case 2: gameStats.bible.BibleDamage = 6.25f; break;
                case 3: gameStats.bible.BibleProjectiles = 2; gameStats.bible.BibleLifetime = 4.0f; break;
                case 4: gameStats.bible.BibleDamage = 8.0f; break;
                case 5: gameStats.bible.BibleProjectiles = 3; gameStats.bible.BibleCooldown = 4.0f; break;
                case 6: gameStats.bible.BibleDamage = 10.0f; break;
                case 7: gameStats.bible.BibleProjectiles = 4; gameStats.bible.BibleLifetime = 5.0f; break;
                case 8: gameStats.bible.BibleDamage = 12.5f; break;
                case 9: gameStats.bible.BibleDamage = 16.0f; break;
            }
            Debug.Log("Level Up Bible");
        }
        // Holy Water
        if(option == 6){ 
            gameStats.holyWater.WaterLevel += 1;
            switch(gameStats.holyWater.WaterLevel){
                case 1: gameStats.holyWater.WaterProjectiles = 1; break;
                case 2: gameStats.holyWater.WaterProjectiles = 2; gameStats.holyWater.WaterDamage = 0.125f; break;
                case 3: gameStats.holyWater.WaterDamage = 0.15f; gameStats.holyWater.WaterLifetime = 5; break;
                case 4: gameStats.holyWater.WaterProjectiles = 3; gameStats.holyWater.WaterCooldown = 7; break;
                case 5: gameStats.holyWater.WaterDamage = 0.2f; break;
                case 6: gameStats.holyWater.WaterProjectiles = 4; gameStats.holyWater.WaterLifetime = 6; break;
                case 7: gameStats.holyWater.WaterProjectiles = 5; gameStats.holyWater.WaterCooldown = 6; break;
                case 8: gameStats.holyWater.WaterProjectiles = 6; gameStats.holyWater.WaterDamage = 0.25f; break;
                case 9: gameStats.holyWater.WaterProjectiles = 6; gameStats.holyWater.WaterDamage = 0.3f; break;
            }
            Debug.Log("Level Up Holy Water");
        }
        // Knife
        if(option == 7){ 
            gameStats.throwingKnife.KnifeLevel +=1;
            switch(gameStats.throwingKnife.KnifeLevel){
                case 1: gameStats.throwingKnife.KnifeProjectiles = 1; break;
                case 2: gameStats.throwingKnife.KnifeProjectiles = 2; gameStats.throwingKnife.KnifeDamage = 6.25f; break;
                case 3: gameStats.throwingKnife.KnifeCooldown = 7; gameStats.throwingKnife.KnifeDurability = 2; break;
                case 4: gameStats.throwingKnife.KnifeProjectiles = 3; gameStats.throwingKnife.KnifeDamage = 8; break;
                case 5: gameStats.throwingKnife.KnifeProjectiles = 4; gameStats.throwingKnife.KnifeCooldown = 6; break;
                case 6: gameStats.throwingKnife.KnifeDamage = 10.0f; break;
                case 7: gameStats.throwingKnife.KnifeProjectiles = 5; gameStats.throwingKnife.KnifeDamage = 12.5f; break;
                case 8: gameStats.throwingKnife.KnifeCooldown = 5; gameStats.throwingKnife.KnifeDurability = 3; break;
                case 9: gameStats.throwingKnife.KnifeProjectiles = 6; gameStats.throwingKnife.KnifeDamage = 16.0f; break;
            }
            Debug.Log("Level Up Knife");
        }
        // Remove level up panel and resume game
            Time.timeScale = 1;
            _gameUIWrapper.Clear();
    }
    
    public Sprite SelectImage(int option){
        switch(option){
            case 0: Sprite maxHealthImage = Resources.Load<Sprite>("Images/maxHealth"); return maxHealthImage;
            case 1: Sprite healthImage = Resources.Load<Sprite>("Images/health"); return healthImage;
            case 2: Sprite speedImage = Resources.Load<Sprite>("Images/speed"); return speedImage;
            case 3: Sprite xpRadiusImage = Resources.Load<Sprite>("Images/xpRadius"); return xpRadiusImage;
            case 4: Sprite whipSprite = Resources.Load<Sprite>("Images/whip"); return whipSprite;
            case 5: Sprite bibleSprite = Resources.Load<Sprite>("Images/bible"); return bibleSprite;
            case 6: Sprite holyWaterSprite = Resources.Load<Sprite>("Images/holyWater"); return holyWaterSprite;
            case 7: Sprite knifeSprite = Resources.Load<Sprite>("Images/knife"); return knifeSprite;
            default: Sprite maxHealthImagea = Resources.Load<Sprite>("Images/maxHealth"); return maxHealthImagea;
        }
    }

    public void Dictionaries()
    {
        // Create dictionaries
        Dictionary<int, string> maxHealthDictionary = new Dictionary<int, string>();
        Dictionary<int, string> healthPickupsDictionary = new Dictionary<int, string>();
        Dictionary<int, string> speedDictionary = new Dictionary<int, string>();
        Dictionary<int, string> xpRadiusDictionary = new Dictionary<int, string>();
        Dictionary<int, string> whipDictionary = new Dictionary<int, string>();
        Dictionary<int, string> bibleDictionary = new Dictionary<int, string>();
        Dictionary<int, string> holyWaterDictionary = new Dictionary<int, string>();
        Dictionary<int, string> knifeDictionary = new Dictionary<int, string>();

        // Assign Values - Player
        // Max Health -> 0
        maxHealthDictionary.Add(2, "Level 2: Increase Max Health by 10%");
        maxHealthDictionary.Add(3, "Level 3: Increase Max Health by 10%");
        maxHealthDictionary.Add(4, "Level 4: Increase Max Health by 10%");
        maxHealthDictionary.Add(5, "Level 5: Increase Max Health by 10%");
        maxHealthDictionary.Add(6, "Level 6: Increase Max Health by 10%");
        maxHealthDictionary.Add(7, "Level 7: Increase Max Health by 10%");

        // Health Pickups -> 1
        healthPickupsDictionary.Add(2, "Level 2: Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(3, "Level 3: Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(4, "Level 4: Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(5, "Level 5: Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(6, "Level 6: Increase health restored by Pickups 50%");
        healthPickupsDictionary.Add(7, "Level 7: Increase health restored by Pickups 50%");
        
        // Player Speed -> 2
        speedDictionary.Add(2, "Level 2: Increase Player Speed 10%");
        speedDictionary.Add(3, "Level 3: Increase Player Speed 10%");
        speedDictionary.Add(4, "Level 4: Increase Player Speed 10%");
        speedDictionary.Add(5, "Level 5: Increase Player Speed 10%");
        speedDictionary.Add(6, "Level 6: Increase Player Speed 10%");
        speedDictionary.Add(7, "Level 7: Increase Player Speed 10%");

        // XP Radius -> 3
        xpRadiusDictionary.Add(2, "Level 2: Increase XP Radius to 1");
        xpRadiusDictionary.Add(3, "Level 3: Increase XP Radius to 1.5");
        xpRadiusDictionary.Add(4, "Level 4: Increase XP Radius to 2");
        xpRadiusDictionary.Add(5, "Level 5: Increase XP Radius to 2.5");
        xpRadiusDictionary.Add(6, "Level 6: Increase XP Radius to 3");
        xpRadiusDictionary.Add(7, "Level 7: Increase XP Radius to 3.5");
        
        // Assign Values - Weapons
        // Whip -> 4
        whipDictionary.Add(2, "Level 2: Increase Whip Damage by 25%");
        whipDictionary.Add(3, "Level 3: Reduce Whip Cooldown by 20%");
        whipDictionary.Add(4, "Level 4: Increase Whip Projectiles to 2 and Damage by 25%");
        whipDictionary.Add(5, "Level 5: Reduce Whip Cooldown by 10%");
        whipDictionary.Add(6, "Level 6: Increase Whip Damage by 25%");
        whipDictionary.Add(7, "Level 7: Reduce Whip Cooldown by 10%");
        whipDictionary.Add(8, "Level 8: Increase Whip Damage by 25%");
        whipDictionary.Add(9, "Level 9: Increase Whip Damage by 25% and Reduce Whip Cooldown by 10%");

        // Bible -> 5
        bibleDictionary.Add(1, "Level 1: Unlock Bible Weapon");
        bibleDictionary.Add(2, "Level 2: Increase Bible Damage by 25%");
        bibleDictionary.Add(3, "Level 3: Increase Amount of Bibles to 2 and Lifetime by 1s");
        bibleDictionary.Add(4, "Level 4: Increase Bible Damage by 25%");
        bibleDictionary.Add(5, "Level 5: Increase Amount of Bibles to 3 and Reduce Cooldown by 1s");
        bibleDictionary.Add(6, "Level 6: Increase Bible Damage by 25%");
        bibleDictionary.Add(7, "Level 7: Increase Amount of Bibles to 4 and Lifetime by 1s");
        bibleDictionary.Add(8, "Level 8: Increase Bible Damage by 25%");
        bibleDictionary.Add(9, "Level 9: Increase Bible Damage by 25%");

        // Holy Water -> 6
        holyWaterDictionary.Add(1, "Level 1: Unlock Holy Water Weapon");
        holyWaterDictionary.Add(2, "Level 2: Increase Amount of Holy Waters to 2 and Damage by 25%");
        holyWaterDictionary.Add(3, "Level 3: Increase Holy Water Damage by 25% and Lifetime by 1s");
        holyWaterDictionary.Add(4, "Level 4: Increase Amount of Holy Waters to 3 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(5, "Level 5: Increase Holy Water Damage by 25%");
        holyWaterDictionary.Add(6, "Level 6: Increase Amount of Holy Waters to 4 and Lifetime by 1s");
        holyWaterDictionary.Add(7, "Level 7: Increase Amount of Holy Waters to 5 and Reduce Cooldown by 1s");
        holyWaterDictionary.Add(8, "Level 8: Increase Amount of Holy Waters to 6 and Damage by 25%");
        holyWaterDictionary.Add(9, "Level 9: Increase Amount of Holy Waters to 7 and Damage by 25%");

        // Knife -> 7
        knifeDictionary.Add(1, "Level 1: Unlock Throwing Knifes Weapon");
        knifeDictionary.Add(2, "Level 2: Increase Amount of Knifes to 2 and Damage by 25%");
        knifeDictionary.Add(3, "Level 3: Reduce Knifes Cooldown by 1s and Increase Durability to 2");
        knifeDictionary.Add(4, "Level 4: Increase Amount of Knifes to 3 and Damage by 25%");
        knifeDictionary.Add(5, "Level 5: Increase Amount of Knifes to 4 Reduce Cooldown by 1s");
        knifeDictionary.Add(6, "Level 6: Increase Knifes Damage by 25%");
        knifeDictionary.Add(7, "Level 7: Increase Amount of Knifes to 5 and Damage by 25%");
        knifeDictionary.Add(8, "Level 8: Reduce Knifes Cooldown by 1s and Increase Durability to 3");
        knifeDictionary.Add(9, "Level 9: Increase Amount of Knifes to 6 and Damage by 25%");

        // Add Dictionaries to list of dictionaries
        dictionariesList.Add(maxHealthDictionary);              //  -> 0
        dictionariesList.Add(healthPickupsDictionary);          //  -> 1
        dictionariesList.Add(speedDictionary);                  //  -> 2
        dictionariesList.Add(xpRadiusDictionary);               //  -> 3
        dictionariesList.Add(whipDictionary);                   //  -> 4
        dictionariesList.Add(bibleDictionary);                  //  -> 5
        dictionariesList.Add(holyWaterDictionary);              //  -> 6
        dictionariesList.Add(knifeDictionary);                  //  -> 7
    }
}
