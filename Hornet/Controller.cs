using System;
using System.Collections;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Satchel;
using static Silksong.Helpers;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace Silksong.Hornet {
    public class Controller : MonoBehaviour
    {
        public GameObject current;
        private GameObject BossPrefab, NpcPrefab;

        private GameObject BossGo, NpcGo;
        private string currentSourceClip,currentDestinationClip;
        private tk2dSpriteAnimator source,destination; 
        private Hornets CurrentHornet = Hornets.NPC;
        private bool initialised = false;

        private IEnumerator CreateChangelings(){
            yield return new WaitWhile(() => {
                return HeroController.instance == null || HeroController.instance.gameObject == null;
            });
            if(BossGo == null){
                BossGo = CreateChangeling(BossPrefab,"h0rnetB");
            }
            if(NpcGo == null){
                NpcGo = CreateChangeling(NpcPrefab,"h0rnetN");
            }
        }
        public void Init(GameObject BossPrefab,GameObject NpcPrefab){
            if(initialised) { return; }
            initialised = true;
            this.BossPrefab = BossPrefab;
            this.NpcPrefab = NpcPrefab;
            if(Sounds.AudioClips == null){
                Sounds.AudioClips = BossPrefab.GetComponent<PlayMakerFSM>().getAudioClips();
            }
            StartCoroutine(CreateChangelings());
            ModHooks.HeroUpdateHook += HeroUpdate;
            ModHooks.BeforePlayerDeadHook += BeforePlayerDeadHook;
            ModHooks.SlashHitHook += Sounds.SlashHit;
            On.PlayMakerFSM.OnEnable += FsmEdit;
            On.HeroController.DieFromHazard += FixHazardAndAcidDeath;
        }

        private void BeforePlayerDeadHook(){
            CurrentHornet = Hornets.NPC;
        }

        private IEnumerator FixHazardAndAcidDeath(On.HeroController.orig_DieFromHazard orig, HeroController self,
            HazardType hazardtype, float angle)
        {
            HeroController heroController = self;
            if (!heroController.cState.hazardDeath)
            {
                heroController.playerData.disablePause = true;
                typeof(HeroController).GetMethod("StopTilemapTest", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(self, new object[] {});
                typeof(HeroController).GetMethod("SetState", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(self, new object[] {ActorStates.no_input});
                heroController.cState.hazardDeath = true;
                
                
                typeof(HeroController).GetMethod("ResetMotion", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(self, new object[] {});
                
                heroController.ResetHardLandingTimer();
                heroController.AffectedByGravity(false);

                ReflectionHelper.GetField<HeroController, MeshRenderer>(self, "renderer").enabled = false;
                heroController.gameObject.layer = 2;
                yield return (object) null;
                heroController.StartCoroutine(GameManager.instance.PlayerDeadFromHazard(0.0f));

            }
        }

        private void FsmEdit(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            if (self.FsmName == "Knight Death Control")
            {
                self.GetAction<Tk2dPlayAnimation>("Stab", 20).clipName = currentSourceClip;
                self.GetAction<Tk2dPlayAnimation>("Fling", 0).clipName = currentSourceClip;

                self.InsertMethod("Stab", () => source.Play("Spike Death Antic"), 0);
                self.InsertMethod("Fling", () => source.Play("Spike Death"), 0);
            }
            else if (self.FsmName == "Hero Death Anim")
            {
                self.GetAction<Tk2dPlayAnimation>("Start", 0).clipName = currentSourceClip;
                self.GetAction<SetMeshRenderer>("Start", 1).active = false;
                self.RemoveAction("Start", 2);

                self.InsertMethod("Start", () => source.Play("Death"), 0);
            }

            orig(self);
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
            if(BossGo == null){
                BossGo = CreateChangeling(BossPrefab,"h0rnetB");
            }
            if(NpcGo == null){
                NpcGo = CreateChangeling(NpcPrefab,"h0rnetN");
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
