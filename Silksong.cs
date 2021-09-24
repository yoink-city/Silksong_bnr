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

namespace Silksong
{
    public class Silksong : Mod
    {

        internal static Silksong Instance;
        public static GameObject BossPrefab,NpcPrefab,ControllerGo;

        public static Controller ControllerScript;
        private static Sprite SilkSongTitle;
            
        public override string GetVersion() => "v0.3.0 - 0";
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
            BossPrefab = preloadedObjects["GG_Hornet_2"]["Boss Holder/Hornet Boss 2"];
            NpcPrefab = preloadedObjects["Deepnest_Spider_Town"]["Hornet Beast Den NPC"];
            Object.DontDestroyOnLoad(BossPrefab);
            Object.DontDestroyOnLoad(NpcPrefab);
            CreateHornetController();
        }

        public void CreateHornetController(){
            ControllerGo = new GameObject();
            ControllerGo.name = "SilksongController";
            ControllerScript = ControllerGo.GetAddComponent<Controller>();
            ControllerScript.Init(BossPrefab,NpcPrefab);
            ControllerGo.SetActive(true);
            Object.DontDestroyOnLoad(ControllerGo);
        }
        
    }
}
