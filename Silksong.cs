using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Modding;
using Satchel;
using UnityEngine;
using static Satchel.EnemyUtils;
using static Satchel.AssemblyUtils;
using Object = UnityEngine.Object;
using Silksong.Hornet;
using UnityEngine.SceneManagement;

namespace Silksong
{
    public class Silksong : Mod
    {

        internal static Silksong Instance;
        public static GameObject BossPrefab,NpcPrefab,ControllerGo;

        public static Controller ControllerScript;
        private static Sprite SilkSongTitle;
            
        public override string GetVersion() => "v0.3.0 - 1";
        public new string GetName() => "Silksong (but not really)";

        public Silksong()
        {
            if (SilkSongTitle == null)
            {
                SilkSongTitle = GetSpriteFromResources("SilkSongTitle.png");
            }
            //In constructor because initialize too late
            On.MenuStyleTitle.SetTitle += FixBanner;
        }
        private void FixBanner(On.MenuStyleTitle.orig_SetTitle orig, MenuStyleTitle self, int index)
        {
            self.Title.sprite = SilkSongTitle;
            var tcl = GameObject.Find("TeamCherryLogo");
            var ycl = GameObject.Find("YoinkCityLogo");
            if(tcl != null && ycl == null){
                ycl = new GameObject();
                ycl.name = "YoinkCityLogo";
                ycl.transform.position = tcl.transform.position + new Vector3(0.75f, 0.2f, 0f);
                ycl.transform.localScale = new Vector3(0.11f,0.11f,1f);
                ycl.transform.SetParent(tcl.transform, true);
                var sr = ycl.GetAddComponent<SpriteRenderer>();
                sr.sprite = GetSpriteFromResources("YoinkCity.png");
            }
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Hornet_2", "Boss Holder/Hornet Boss 2"),
                ("Deepnest_Spider_Town", "Hornet Beast Den NPC")
            };   
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            Modding.ModHooks.LanguageGetHook += WanguageGet;
            BossPrefab = preloadedObjects["GG_Hornet_2"]["Boss Holder/Hornet Boss 2"];
            NpcPrefab = preloadedObjects["Deepnest_Spider_Town"]["Hornet Beast Den NPC"];
            Object.DontDestroyOnLoad(BossPrefab);
            Object.DontDestroyOnLoad(NpcPrefab);
            CreateHornetController();
        }

        private string WanguageGet(string key, string sheetTitle, string owig)
        {
            //This mod has now been claimed by the OwO army
            /*Logger($"LanguageGet key: {key}");
            Logger($"LanguageGet title: {sheetTitle}");
            Debug.Log($"[Silksong]:LanguageGet string: {owig}");*/

            if (key == "CREDITS_CONGRATS_BODY")
            {
                owig += "<br> Mod made by Yoink City";
                return owig;
            }
            if (key == "CREDITS_EXTRA_THANKS_TEXT")
            {
                owig = "2158 Backers, Kickstarter<br> + Dandy, Mulhima & Ruttie for the mod. (^.^)";
                return owig;
            }
            if (key == "CREDITS_GAME_BY_NAME")
            {
                owig += "<br> Dandy, Mulhima & Ruttie";
                return owig;
            }
            if (key == "CREDITS_GAME_BY")
            {
                owig = "A game and mod by";
                return owig;
            }
            return owig;
        }

        public void CreateHornetController(){
            ControllerGo = new GameObject();
            ControllerGo.name = "SilksongController";
            ControllerScript = ControllerGo.GetAddComponent<Controller>();
            ControllerScript.Init(BossPrefab,NpcPrefab);
            ControllerGo.SetActive(true);
            Object.DontDestroyOnLoad(ControllerGo);
        }
        
        private void Logger(string text)
        {
            Modding.Logger.Log($":[Silksong]:{text}");
            Debug.Log($"[Silksong]:{text}");
        }

    }
}
