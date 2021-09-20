using System.Collections.Generic;
using Modding;
using Satchel;
using UnityEngine;
using static Satchel.EnemyUtils;
using Object = UnityEngine.Object;

namespace Changeling
{
    public class Changeling : Mod
    {

        internal static Changeling Instance;
        public GameObject BossPrefab,current;
        public GameObject NpcPrefab;
        public GameObject BossGo, NpcGo;
        
        public enum Hornets
        {
            Boss = 0,
            NPC
        }

        public int CurrentHornet;

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
            {"Dash","G Dash"},
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
            {"Turn","Evade Antic"},
            {"DN Charge","Sphere Antic G"},
            {"Surface Swim","Fall"},
            {"Surface Idle","Idle"},
            {"Surface In","Land"},
            {"Thorn Attack","Sphere Attack"},
            {"Enter","Sphere Antic G"},
            {"Prostrate","Sphere Recover G"},
            {"Prostrate Rise","Sphere Recover G"},
            {"Challenge Start","Flourish"},
            {"Challenge End","Flourish"},
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
            {"Sit Lean","Den Idle"},
            {"Sitting Asleep","Den Idle"},
            {"Wake","Den Idle"},
            {"Get Off","Den Idle"},
        };
            
            
        public override string GetVersion()
        {
            return "0.1";
        }


        public GameObject createChangeling(GameObject go)
        {
            var changed = go.createCompanionFromPrefab();
            changed.SetActive(true);
            
            changed.transform.position =
                HeroController.instance.gameObject.transform.position + new Vector3(0f, 0f, 0f);
            changed.transform.SetParent(HeroController.instance.gameObject.transform, true);
            
            return changed;
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
            
            CreateHornet();

            // todo remove later
            HeroController.instance.gameObject.logTk2dAnimationClips();
            BossPrefab.logTk2dAnimationClips();
            NpcPrefab.logTk2dAnimationClips();
        }

        public void CheckClips()
        {
            if(source == null)
            {
                source = HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>();
            }

            if(destination == null)
            {
                destination = current.GetComponent<tk2dSpriteAnimator>();
            }
            
            var clip =  source.CurrentClip.name;
            if(clip != currentSourceClip)
            {
                currentSourceClip = clip;
                if (clips.TryGetValue(currentSourceClip, out var dclip))
                {
                    if (CurrentHornet != (int) Hornets.Boss)
                    {
                        CurrentHornet = (int) Hornets.Boss;
                        UpdateHornet();
                    }
                    currentDestinationClip = dclip;
                    destination.Play(currentDestinationClip);
                    
                }
                else if (NpcClips.TryGetValue(currentSourceClip, out var dclip2))
                {
                    if (CurrentHornet != (int) Hornets.NPC)
                    {
                        CurrentHornet = (int) Hornets.NPC;
                        UpdateHornet();
                    }
                    currentDestinationClip = dclip2;
                    destination.Play(currentDestinationClip);
                }
            }
        }

        public void UpdateHornet()
        {
            CreateHornet();
            destination = current.GetComponent<tk2dSpriteAnimator>();
        }
        
        public void CreateHornet()
        {
            if (CurrentHornet == (int) Hornets.Boss)
            {
                if (BossGo == null)
                {
                    BossGo = createChangeling(BossPrefab);
                }
                BossGo.GetComponent<MeshRenderer>().enabled = true;

                DisableOtherHornets();

                current = BossGo;
            }

            if (CurrentHornet == (int) Hornets.NPC)
            {
                if (NpcGo == null)
                {
                    NpcGo = createChangeling(NpcPrefab);
                }
                NpcGo.GetComponent<MeshRenderer>().enabled = true;
                
                DisableOtherHornets();
                
                current = NpcGo;
            }

        }

        private void DisableOtherHornets()
        {
            if (CurrentHornet == (int) Hornets.Boss)
            {
                if (NpcGo != null)
                {
                    NpcGo.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            if (CurrentHornet == (int) Hornets.NPC)
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
            CheckClips();
        }
    }
}
