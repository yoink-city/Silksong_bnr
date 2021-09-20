using System.Collections.Generic;
using Modding;
using Satchel;
using UnityEngine;
using static Satchel.EnemyUtils;
using Object = UnityEngine.Object;

namespace Silksong
{
    public class Silksong : Mod
    {

        internal static Silksong Instance;
        public GameObject BossPrefab,current;
        public GameObject NpcPrefab;
        public GameObject BossGo, NpcGo;
        new public string GetName() => "Silksong (but not really)";

        public enum Hornets
        {
            Boss = 0,
            NPC
        }

        public Hornets CurrentHornet = Hornets.Boss;

        public string currentSourceClip,currentDestinationClip;  

        public tk2dSpriteAnimator source,destination; 

        public Dictionary<string,string> clips =  new Dictionary<string,string>()
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
            {"Surface Idle","Idle"},
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

        public Dictionary<string, string> NpcClips = new Dictionary<string, string>()
        {
            {"Sit","Den Idle"},
            {"Sit Idle","Den Idle"},
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
            
            
        public override string GetVersion()
        {
            return "0.2";
        }


        public GameObject createChangeling(GameObject go)
        {
            var hero = HeroController.instance.gameObject;
            var changed = go.createCompanionFromPrefab();
            changed.SetActive(true);
            changed.transform.position = hero.transform.position + new Vector3(0f, 0f, 0f);
            changed.transform.SetParent(hero.transform, true);
            return changed;
        }

        public void SetCurrentHornet()
        {

            if (BossGo == null)
            {
                BossGo = createChangeling(BossPrefab);
            }

            if (NpcGo == null)
            {
                NpcGo = createChangeling(NpcPrefab);
            }

            if (CurrentHornet == Hornets.Boss){
                current = BossGo;
            } else if (CurrentHornet == Hornets.NPC) {
                current = NpcGo;
            }

            DisableOtherHornets();
            current.GetComponent<MeshRenderer>().enabled = true;

        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            BossPrefab = preloadedObjects["GG_Hornet_2"]["Boss Holder/Hornet Boss 2"];
            NpcPrefab = preloadedObjects["Deepnest_Spider_Town"]["Hornet Beast Den NPC"];
            Object.DontDestroyOnLoad(BossPrefab);
            Object.DontDestroyOnLoad(NpcPrefab);
            ModHooks.HeroUpdateHook += update;
        }
       
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Hornet_2", "Boss Holder/Hornet Boss 2"),
                ("Deepnest_Spider_Town", "Hornet Beast Den NPC")
            };   
        }

        
        public void Change()
        {
            if (current != null)
            {
                return;
            }
            
            SetCurrentHornet();

            // todo remove later
            HeroController.instance.gameObject.logTk2dAnimationClips();
            BossPrefab.logTk2dAnimationClips();
            NpcPrefab.logTk2dAnimationClips();
        }

        public void ImitateClips()
        {
            if(source == null){
                source = HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>();
            }

            if(destination == null){
                destination = GetCurrentAnimator();
            }
            
            var clip =  source.CurrentClip.name;
            
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
                    destination.Play(currentDestinationClip);
                } else if (NpcClips.TryGetValue(currentSourceClip, out var dclip2)) {
                    if (CurrentHornet != Hornets.NPC)
                    {
                        CurrentHornet = Hornets.NPC;
                        destination = GetCurrentAnimator();
                    }
                    currentDestinationClip = dclip2;
                    destination.Play(currentDestinationClip);
                }

            }
        }

        public tk2dSpriteAnimator GetCurrentAnimator()
        {
            SetCurrentHornet();
            return current.GetComponent<tk2dSpriteAnimator>();
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
        public void update()
        {   
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Change();
            ImitateClips();
        }
    }
}
