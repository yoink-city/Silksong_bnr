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
            {"Idle","Idle"}
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
            current.transform.position = HeroController.instance.gameObject.transform.position + new Vector3(0f,1f,0f);
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
