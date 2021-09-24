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

namespace Silksong
{
    public class Silksong : Mod
    {

        internal static Silksong Instance;
        private static GameObject BossPrefab,NpcPrefab, current;
        private static GameObject BossGo, NpcGo;
        private static Sprite SilkSongTitle;
        
        private static string currentSourceClip,currentDestinationClip;
        private static tk2dSpriteAnimator source,deathsource,spikedeathsource,aciddeathsource,destination; 
        
        public enum Hornets
        {
            Boss = 0,
            NPC
        }

        private static Hornets CurrentHornet = Hornets.NPC;

        private Dictionary<string,AudioClip> Audios;
        //Hornet_Fight_Flourish_02
        //Hornet_Fight_Laugh_01
        //Hornet_Fight_Laugh_02
        //Hornet_Fight_Stun_01
        //Hornet_Fight_Stun_03
        //Hornet_Fight_Yell_04
        //Hornet_Fight_Yell_06
        //Hornet_Fight_Yell_08
        //Hornet_Fight_Yell_09

        private static Dictionary<string,string> clips =  new Dictionary<string,string>()
        {
            {"Idle","Idle"},
            {"Idle Hurt","Wounded"},
            {"Recoil","Barb Throw Recover"},
            {"Run","Run"},
            {"Sprint","Run"},
            {"Land","Land"},
            {"Walk","Run"},
            {"Map Walk","Run"},
            {"Shadow Dash","A Dash"},
            {"Airborne","Fall"},
            {"Double Jump","Jump"},
            {"Death","Death Air"},
            {"Slash","Throw"},
            {"SlashAlt","Throw Antic"},
            {"Fireball Antic","Throw Antic"},
            {"Fireball1 Cast","Throw"},
            {"Focus","Sphere Antic G"},
            {"Focus Get","Sphere Antic G"},
            {"Focus Get Once","Sphere Antic G"},
            {"Focus End","Land"},
            {"HardLand","Hard Land"},
            {"Spike Death","Death Air"},
            {"Collect Magical 1","Sphere Antic A"},
            {"Collect Magical 2","Sphere Antic A"},
            {"Collect Magical 3","Sphere Antic A"},
            {"Collect Normal 1","Sphere Antic A"},
            {"Collect Normal 2","Sphere Antic A"},
            {"Collect Normal 3","Sphere Antic A"},
            {"Acid Death","Death Air"},
            {"Idle Wind","Idle"},
            {"Fall","Fall"},
            {"Collect Magical Fall","Sphere Recover A"},
            {"Collect Magical Land","Land"},
            {"Collect Heart Piece","Sphere Recover A"},
            {"Collect Heart Piece End","Land"},
            {"Roar Lock","Flourish"},
            {"Stun","Stun"},
            {"Run To Idle","Run"},
            {"Map Idle","Idle"},
            {"SD Hit Wall","Wall Impact"},
            {"SD Wall Charge","Wall Impact"},
            {"Lantern Idle","Idle"},
            {"Lantern Run","Run"},
            {"Fireball2 Cast","Throw"},
            {"Map Open","Idle"},
            {"Map Turn","Idle"},
            {"Death Dream","Death Air"},
            {"Wall Slide","Wall Impact"},
            {"DN Charge","Sphere Antic G"},
            {"Surface Swim","Fall"},
            {"Surface Idle","Fall"},
            {"Surface In","Land"},
            {"Thorn Attack","Sphere Attack"},
            {"Enter","Sphere Antic G"},
            {"Prostrate","Sphere Recover G"},
            {"Prostrate Rise","Sphere Recover G"},
            {"TurnToBG","Sphere Antic G"},
            {"SD Charge Ground","Sphere Antic G"},
            {"SD Charge Ground End","Sphere Antic G"},
            {"SD Dash","A Dash"},
            {"SD Air Brake","Evade"},
            {"Exit","Sphere Antic G"},
            {"UpSlash","Counter Attack 2"},
            {"DownSlash","Evade"},
            {"NA Big Slash","Counter Attack 1"},
            {"NA Dash Slash","Counter Attack 1"},
            {"Dash Down Land","Hard Land"},
            {"Quake Land","Hard Land"},
            {"Super Hard Land","Hard Land"},
            {"Dreamer Land","Land"},
            {"Dreamer Lift","Fall"},
            {"Walljump","Jump"},
            {"Shadow Dash Down","Hard Land"},
            {"Shadow Dash Sharp","A Dash"},
            {"DJ Get Land","Land"},
            {"Shadow Dash Down Sharp","Hard Land"},
            {"Spike Death Antic","Death Air"},
            {"NA Charge","Sphere Antic G"},
            {"NA Cyclone","Sphere Attack"},
            {"NA Cyclone End","Sphere Recover A"},
            {"NA Cyclone Start","Sphere Antic A"},
            {"Quake Fall 2","Fall"},
            {"Quake Land 2","Hard Land"},
            {"Scream","Jump"},
            {"Scream Start","Jump Antic"},
            {"Scream 2","Jump"},
            {"DN Cancel","Throw Recover"},
            {"DN Slash Antic","Throw Antic"},
            {"DN Slash","Throw"},
            {"DG Set Charge","Land"},
            {"DG Set End","Idle"},
            {"DG Warp Charge","Sphere Antic G"},
            {"DG Warp","Fall"},
            {"DG Warp Cancel","Idle"},
            {"DG Warp In","Fall"},
        };
        
        private static Dictionary<string, string> NpcClips = new Dictionary<string, string>()
        {
            {"Sit","Den Idle"},
            {"Sit Idle","Den Talk R"},
            {"Sit Lean","Den Talk R"},
            {"Sitting Asleep","Den Talk R"},
            {"Wake","Den End R"},
            {"Get Off","Den Idle"},
            {"Dash","Harpoon Side"},
            {"TurnToIdle","TurnToIdle"},
            {"Turn","Turn"},
            {"Challenge Start","Point"},
            {"Challenge End","Point End"}
        };
            
            
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

        public List<string> tempStrings = new List<string>();
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            BossPrefab = preloadedObjects["GG_Hornet_2"]["Boss Holder/Hornet Boss 2"];
            NpcPrefab = preloadedObjects["Deepnest_Spider_Town"]["Hornet Beast Den NPC"];
            
            Object.DontDestroyOnLoad(BossPrefab);
            Object.DontDestroyOnLoad(NpcPrefab);
            Audios = BossPrefab.GetComponent<PlayMakerFSM>().getAudioClips();
            foreach(KeyValuePair<string,AudioClip> kvp in Audios){
                tempStrings.Add(kvp.Key);
            }
            ModHooks.HeroUpdateHook += Update;
            ModHooks.BeforePlayerDeadHook += () => {
                Log("Dead");
                hasDied = true;
                lastDeathTime = DateTime.Now;
            };
        }
        private void Update()
        {   
            
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
            ChangeToHornet();
            ImitateClips();

            if(Input.GetKeyDown(KeyCode.Space)){
                if(i >= tempStrings.Count){
                    i = 0;
                } else {
                    i+=1;
                }
                HeroController.instance.gameObject.logTk2dAnimationClips();
                BossPrefab.logTk2dAnimationClips();
                NpcPrefab.logTk2dAnimationClips();
                GameObjectUtils.PrintAllActiveGameObjectsInScene();  
            }
        }
        
        private void ChangeToHornet()
        {
            if (current != null)
            {
                return;
            }
            SetCurrentHornet();
        }
        
        private void SetCurrentHornet()
        {

            if (BossGo == null)
            {
                BossGo = CreateChangeling(BossPrefab);
            }

            if (NpcGo == null)
            {
                NpcGo = CreateChangeling(NpcPrefab);
            }

            if(CurrentHornet == Hornets.Boss){
                current = BossGo;
            }
            if(CurrentHornet == Hornets.NPC){
                current = NpcGo;
            }
            
            
            DisableOtherHornets();
            current.GetComponent<MeshRenderer>().enabled = true;
        }
        
        private GameObject CreateChangeling(GameObject go)
        {
            var hero = HeroController.instance.gameObject;
            var changed = go.createCompanionFromPrefab();
            changed.name = "h0rnet";
            changed.layer = hero.layer;
            changed.SetActive(true);
            changed.transform.position = hero.transform.position + new Vector3(0f, 0f, 0.001f);
            changed.transform.SetParent(hero.transform, true);
            return changed;
        }
        
        private void DisableOtherHornets()
        {
            if (CurrentHornet == Hornets.Boss)
            {
                if (NpcGo != null)
                {
                    NpcGo.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            if (CurrentHornet == Hornets.NPC)
            {
                if (BossGo != null)
                {
                    BossGo.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        public Dictionary<string,string> lastClipForSource = new Dictionary<string,string>();

        public DateTime lastDeathTime = DateTime.Now;
        public bool hasDied = false;
 
        private void ImitateClips()
        {
            if(source == null)
            {
                source = HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>();
                if(source == null){
                    return;
                }
            }

            if(deathsource == null){
                var heroDeath = HeroController.instance.gameObject.Find("Hero Death");
                if(heroDeath != null){
                    var mr = heroDeath.GetComponent<MeshRenderer>();
                    if(mr != null){
                        mr.enabled = false;
                    }
                    deathsource = heroDeath.GetComponent<tk2dSpriteAnimator>();
                }
            }
            if(spikedeathsource == null){
                var heroDeath = HeroController.instance.gameObject.Find("Knight Spike Death");
                if(heroDeath != null){
                    var mr = heroDeath.GetComponent<MeshRenderer>();
                    if(mr != null){
                        mr.enabled = false;
                    }
                    spikedeathsource = heroDeath.GetComponent<tk2dSpriteAnimator>();
                }
            }

            if(aciddeathsource == null){
                var heroDeath = HeroController.instance.gameObject.Find("Knight Acid Death");
                if(heroDeath != null){
                    var mr = heroDeath.GetComponent<MeshRenderer>();
                    if(mr != null){
                        mr.enabled = false;
                    }
                    aciddeathsource = heroDeath.GetComponent<tk2dSpriteAnimator>();
                }
            }

            if(destination == null)
            {
                destination = GetCurrentAnimator();
                if(destination == null){
                    return;
                }
            }

            var clip = source.CurrentClip.name;
            if(hasDied && (DateTime.Now - lastDeathTime).TotalMilliseconds < 3000){
                lastDeathTime = DateTime.Now;
                if(deathsource != null && deathsource.Playing){
                    clip = deathsource.CurrentClip.name;
                }
                if(spikedeathsource != null && spikedeathsource.Playing){
                    clip = spikedeathsource.CurrentClip.name;
                }
                if(aciddeathsource != null && aciddeathsource.Playing){
                    clip = aciddeathsource.CurrentClip.name;
                }
            }

            if(clip != currentSourceClip)
            {
                currentSourceClip = clip;
                if (clips.TryGetValue(currentSourceClip, out var dclip)) {
                    if (CurrentHornet != Hornets.Boss)
                    {
                        CurrentHornet = Hornets.Boss;
                        destination = GetCurrentAnimator();
                    }
                    currentDestinationClip = dclip;
                    if(destination != null){
                        destination.Play(currentDestinationClip);
                    }
                } 
                if (NpcClips.TryGetValue(currentSourceClip, out var dclip2)) {
                    if (CurrentHornet != Hornets.NPC)
                    {
                        CurrentHornet = Hornets.NPC;
                        destination = GetCurrentAnimator();
                    }
                    currentDestinationClip = dclip2;
                    if(destination != null){
                        destination.Play(currentDestinationClip);
                    }
                }

                // simple way to handle audios
                var roll = UnityEngine.Random.Range(0.0f, 1.0f);
                if(currentSourceClip != null && currentSourceClip.Contains("Death")){
                    if(roll < 0.5f){
                        playAudio("Hornet_Fight_Stun_01", true);
                    } else {
                        playAudio("Hornet_Fight_Stun_03", true);
                    }
                }
                if(currentDestinationClip != null && currentDestinationClip.Contains("Point")){
                    if(roll < 0.6f){
                        playAudio("Hornet_Fight_Flourish_02", false);
                    }else if(roll < 0.8f) {
                        playAudio("Hornet_Fight_Yell_09", false);
                    } else {
                        playAudio("Hornet_Fight_Laugh_02", false);
                    }
                }
                if(currentDestinationClip == "Land"){
                    if(roll < 0.1f){
                        playAudio("Hornet_Fight_Laugh_01", false);
                    } else if(roll < 0.2f) {
                        playAudio("Hornet_Fight_Laugh_02", false);
                    }
                }
                if(currentSourceClip != null && currentSourceClip.Contains("Slash")){
                    if(roll < 0.01f){
                        playAudio("Hornet_Fight_Yell_04", false);
                    } else if(roll < 0.02f) {
                        playAudio("Hornet_Fight_Yell_06", false);
                    } else if(roll < 0.03f) {
                        playAudio("Hornet_Fight_Yell_08", false);
                    } else if(roll < 0.04f) {
                        playAudio("Hornet_Fight_Yell_09", false);
                    }
                }
            }
        }

        private tk2dSpriteAnimator GetCurrentAnimator()
        {
            SetCurrentHornet();
            return current.GetComponent<tk2dSpriteAnimator>();
        }
        

        public int i = 0;

        public AudioSource audioSource;
        public void playAudio(string clipName, bool force){
            audioSource = current.GetAddComponent<AudioSource>();
            if(audioSource.isPlaying && !force) { return;}
            if(Audios != null && Audios.TryGetValue(clipName,out var clip)){
                audioSource.PlayOneShot(clip);
            } 
        }
        
    }
}
