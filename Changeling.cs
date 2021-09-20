using System;
using System.Collections;
using System.Collections.Generic;

using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;


using HutongGames.PlayMaker.Actions;

using static Modding.Logger;
using static Satchel.GameObjectUtils;
using static Satchel.FsmUtil;
using static Satchel.EnemyUtils;
using static Satchel.AnimationUtils;

namespace Changeling
{
    public class Changeling : Mod
    {

        internal static Changeling Instance;
        public GameObject Prefab,current; 

        public string currentSourceClip,currentDestinationClip;  

        public tk2dSpriteAnimator source,destination; 

        public Dictionary<string,string> clips =  new Dictionary<string,string>(){
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
            {"Sit","Counter Antic"},
            {"Sit Idle","Counter Stance"},
            {"Sit Lean","Counter Stance"},
            {"Wake","Counter End"},
            {"Get Off","Counter End"},
            {"Sitting Asleep","Counter Stance"},
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
        public override string GetVersion()
        {
            return "0.1";
        }


        public GameObject createChangeling(GameObject ft = null){
            var changed = Prefab.createCompanionFromPrefab();
            //add control and adjust parameters
            //var cc = changed.GetAddComponent<ChangelingControl>();
            //if(ft != null){
            //    cc.followTarget = ft;
            //}
            //fix up collider size
            /*var collider = changed.GetAddComponent<BoxCollider2D>();
            collider.size = new Vector2(1f,2.0f);
            collider.offset = new Vector2(0f,-0.4f);
            // add animations
            gc.Animations.Add("Idle","Idle");
            gc.Animations.Add("Idle","Freed Fake");
            gc.Animations.Add("Idle","Cry Turn");
            gc.Animations.Add("Idle","Idle");
            // extract audios
            var pfsm = grub.GetComponent<PlayMakerFSM>();
            if(pfsm != null){
                gc.teleport = pfsm.GetAction<AudioPlayRandom>("Hero Close",1).audioClips[0];
                gc.yay = pfsm.GetAction<AudioPlayRandom>("Leave",0).audioClips[0];
                gc.walk = pfsm.GetAction<AudioPlay>("Dig",0).oneShotClip.Value as AudioClip;
            }*/
            changed.SetActive(true);
            return changed;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Instance = this;
            Prefab = preloadedObjects["GG_Hornet_2"]["Boss Holder/Hornet Boss 2"];
            UnityEngine.Object.DontDestroyOnLoad(Prefab);
            ModHooks.HeroUpdateHook += update;
        }
       
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Hornet_2", "Boss Holder/Hornet Boss 2")
            };   
        }

        
        public void Change(){
            if(current != null){ return; }
            current = createChangeling();
            current.transform.position = HeroController.instance.gameObject.transform.position + new Vector3(0f,0f,0f);
            current.transform.SetParent(HeroController.instance.gameObject.transform,true);
            var currentr = current.GetComponent<MeshRenderer>();
            currentr.enabled = true;
            // todo remove later
            HeroController.instance.gameObject.logTk2dAnimationClips();
            Prefab.logTk2dAnimationClips();
        }

        public void CheckClips(){
            if(source == null){
                source = HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>();
            }

            if(destination == null){
                destination = current.GetComponent<tk2dSpriteAnimator>();
            }
            var clip =  source.CurrentClip.name;
            if(clip != currentSourceClip){
                currentSourceClip = clip;
                if(clips.TryGetValue(currentSourceClip,out var dclip)){
                    currentDestinationClip = dclip;
                    destination.Play(currentDestinationClip);
                }
            }
        }
        public void update()
        {   
            var r = HeroController.instance.gameObject.GetComponent<MeshRenderer>();
            r.enabled = false;
            Change();
            CheckClips();
        }

    }

}
