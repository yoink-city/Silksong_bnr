using System;
using System.Collections;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Satchel;
using static Silksong.Helpers;

namespace Silksong.Hornet {
    public class Controller : MonoBehaviour
    {
        public GameObject current;
        private GameObject BossGo, NpcGo;
        private string currentSourceClip,currentDestinationClip;
        private tk2dSpriteAnimator source,destination; 
        private Hornets CurrentHornet = Hornets.NPC;
        private bool initialised = false;

        private IEnumerator CreateChangelings(GameObject BossPrefab,GameObject NpcPrefab){
            yield return new WaitWhile(() => {
                return HeroController.instance == null || HeroController.instance.gameObject == null;
            });
            BossGo = CreateChangeling(BossPrefab,"h0rnetB");
            NpcGo = CreateChangeling(NpcPrefab,"h0rnetN");
        }
        public void Init(GameObject BossPrefab,GameObject NpcPrefab){
            if(initialised) { return; }
            initialised = true;

            if(Sounds.AudioClips == null){
                Sounds.AudioClips = BossPrefab.GetComponent<PlayMakerFSM>().getAudioClips();
            }
            StartCoroutine(CreateChangelings(BossPrefab,NpcPrefab));
            ModHooks.HeroUpdateHook += HeroUpdate;
            ModHooks.BeforePlayerDeadHook += BeforePlayerDeadHook;
            On.HeroController.Start += EditFsm;
        }

        private void BeforePlayerDeadHook(){
            CurrentHornet = Hornets.NPC;
        }

        private void EditFsm(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            var fsm = self.transform.Find("Hero Death").gameObject.LocateMyFSM("Hero Death Anim");
            fsm.RemoveAction("Start",2);
            fsm.RemoveAction("Start",1);
            fsm.RemoveAction("Start",0);
            fsm.InsertMethod( "Start", ()=>{
                // probably play death here but somehow also handle the clips
            }, 0);
        }
        private void HeroUpdate()
        {   
            
            HeroController.instance.gameObject.GetComponent<MeshRenderer>().enabled = false;
            ChangeToHornet();
            ImitateClips();

            if(Input.GetKeyDown(KeyCode.K)){
                HeroController.instance.gameObject.logTk2dAnimationClips();
                BossGo.logTk2dAnimationClips();
                NpcGo.logTk2dAnimationClips();
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
            if(CurrentHornet == Hornets.Boss){
                current = BossGo;
            }
            if(CurrentHornet == Hornets.NPC){
                current = NpcGo;
            }
            
            DisableOtherHornets();
            current.GetComponent<MeshRenderer>().enabled = true;
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

        private void ImitateClips()
        {
            if(source == null)
            {
                source = HeroController.instance.gameObject.GetComponent<tk2dSpriteAnimator>();
                if(source == null){
                    return;
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
            
            if(clip != currentSourceClip)
            {
                currentSourceClip = clip;
                if (Animations.BossClips.TryGetValue(currentSourceClip, out var dclip)) {
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
                if (Animations.NpcClips.TryGetValue(currentSourceClip, out var dclip2)) {
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

                Sounds.handleAudioClips(currentSourceClip,currentDestinationClip);
            }
        }

        private tk2dSpriteAnimator GetCurrentAnimator()
        {
            SetCurrentHornet();
            return current.GetComponent<tk2dSpriteAnimator>();
        }
    }
}