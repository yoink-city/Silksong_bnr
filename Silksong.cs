using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Modding;
using Satchel;
using UnityEngine;
using static Satchel.EnemyUtils;
using static Satchel.AssemblyUtils;
using Object = UnityEngine.Object;
using Silksong.Hornet;
using UnityEngine.SceneManagement;
using static Silksong.Helpers;
namespace Silksong
{
    public class Silksong : Mod, ICustomMenuMod,ILocalSettings<ModSettings>
    {

        internal static Silksong Instance;

        public static Satchel.Core satchel;
        public CustomDialogueManager customDialogueManager;

        public static GameObject BossPrefab,NpcPrefab,CardPrefab,ControllerGo;

        public static Controller ControllerScript;
        private static Sprite SilkSongTitle;
            
        public override string GetVersion() => "v0.4.0 - 1";
        public new string GetName() => "Silksong (but not really)";

        public static ModSettings settings { get; set; } = new ModSettings();
        public void OnLoadLocal(ModSettings s) => settings = s;
        public ModSettings OnSaveLocal() => settings;

        public bool ToggleButtonInsideMenu => false;

        public Silksong()
        {
            if (SilkSongTitle == null)
            {
                SilkSongTitle = GetSpriteFromResources("SilkSongTitle.png");
            }
            satchel = new Satchel.Core();
            //In constructor because initialize too late
            On.MenuStyleTitle.SetTitle += FixBanner;
        }
        private void FixBanner(On.MenuStyleTitle.orig_SetTitle orig, MenuStyleTitle self, int index)
        {
            self.Title.sprite = SilkSongTitle;
            var tcl = GameObject.Find("TeamCherryLogo");
            var ycl = GameObject.Find("YoinkCityLogo");
            if(tcl != null && ycl == null){
                ycl = CreateYoinkCityLogo(tcl);
            }
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Hornet_2", "Boss Holder/Hornet Boss 2"),
                ("Deepnest_Spider_Town", "Hornet Beast Den NPC"),
                ("Cliffs_01","Cornifer Card")
            };   
        }

        private void CreateCustomDialogueManager(){
            if(customDialogueManager == null){
                customDialogueManager = satchel.GetCustomDialogueManager(CardPrefab);
                Dialogue.AddCustomDialogue(customDialogueManager);
            }
        }
        private IEnumerator InitCards(){
            yield return null;
            var scene = SceneUtils.getCurrentScene();
            Log(scene.name);
            if(scene.name == "Town"){
                CreateCard(CardPrefab,new Vector3(190f,7.3f,0)).GetAddCustomArrowPrompt(()=>{
                    customDialogueManager.ShowDialogue(Dialogue.hornetConversationKey);
                });
            } else if(scene.name == "Fungus1_04"){
                CreateCard(CardPrefab,new Vector3(10f,27.5f,0)).GetAddCustomArrowPrompt(()=>{
                    customDialogueManager.ShowDialogue(Dialogue.hornetAfterYoungKey);
                });
            }
        }
        public void SceneChange(Scene scene,LoadSceneMode mode){
            GameManager.instance.StartCoroutine(InitCards());
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            BossPrefab = preloadedObjects["GG_Hornet_2"]["Boss Holder/Hornet Boss 2"];
            NpcPrefab = preloadedObjects["Deepnest_Spider_Town"]["Hornet Beast Den NPC"];
            CardPrefab = preloadedObjects["Cliffs_01"]["Cornifer Card"];
            Object.DontDestroyOnLoad(BossPrefab);
            Object.DontDestroyOnLoad(NpcPrefab);
            Object.DontDestroyOnLoad(CardPrefab);
            CreateHornetController();
            CreateCustomDialogueManager();
            CustomArrowPrompt.Prepare(CardPrefab);

            Modding.ModHooks.LanguageGetHook += LanguageGet;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneChange;

        }

        private string LanguageGet(string key, string sheetTitle, string orig)
        {
            if(Dialogue.KeyMap.TryGetValue(key,out var replacement)){
                return replacement;
            }
            // Update Credits
            if (key == "CREDITS_CONGRATS_BODY")
            {
                orig += "<br> Mod made by Yoink City";
                return orig;
            }
            if (key == "CREDITS_EXTRA_THANKS_TEXT")
            {
                orig = "2158 Backers, Kickstarter<br> + Dandy, Mulhima & Ruttie for the mod. (^.^)";
                return orig;
            }
            if (key == "CREDITS_GAME_BY_NAME")
            {
                orig += "<br> Dandy, Mulhima & Ruttie";
                return orig;
            }
            if (key == "CREDITS_GAME_BY")
            {
                orig = "and mod by YoinkCity, original game by";
                return orig;
            }
            if (key == "CREDITS_TESTERS_NAME_COL_01")
            {
                orig += "<br>Mod Tester: ";
            }
            if (key == "CREDITS_TESTERS_NAME_COL_02")
            {
                orig += "DwarfWoot";
                return orig;
            }
            return orig;
        }

        public void CreateHornetController(){
            ControllerGo = new GameObject();
            ControllerGo.name = "SilksongController";
            ControllerScript = ControllerGo.GetAddComponent<Controller>();
            ControllerScript.Init(BossPrefab,NpcPrefab);
            ControllerGo.SetActive(true);
            Object.DontDestroyOnLoad(ControllerGo);
        }
        
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            Menu.saveModsMenuScreen(modListMenu);
            return Menu.CreatemenuScreen();
        }

        private void Logger(string text, int mode = 3)
        {
            if (mode == 1)
            {
                Debug.Log($"[Silksong]: {text}");
            }
            else if (mode == 2)
            {
                Modding.Logger.Log($"[Silksong]: {text}");
            }
            else if (mode == 3)
            {
                Modding.Logger.Log($"[Silksong]: {text}");
                Debug.Log($"[Silksong]: {text}");
            }
        }
    }
}
