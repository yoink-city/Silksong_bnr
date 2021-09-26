using UnityEngine;
using System.Collections.Generic;
using Satchel;
using static Modding.Logger;
namespace Silksong.Hornet
{
    public class Sounds
    {

        public static Dictionary<string,AudioClip> AudioClips;
        //Hornet_Fight_Flourish_02
        //Hornet_Fight_Laugh_01
        //Hornet_Fight_Laugh_02
        //Hornet_Fight_Stun_01
        //Hornet_Fight_Stun_03
        //Hornet_Fight_Yell_04
        //Hornet_Fight_Yell_06
        //Hornet_Fight_Yell_08
        //Hornet_Fight_Yell_09
        public static void handleAudioClips(string currentSourceClip,string currentDestinationClip){
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

            if(currentSourceClip != null && ( currentSourceClip.Contains("NA Big Slash") || currentSourceClip.Contains("NA Dash Slash")  || currentSourceClip.Contains("NA Cyclone") )){
                if(roll < 0.25f){
                    playAudio("Hornet_Fight_Yell_04", false);
                } else if(roll < 0.50f) {
                    playAudio("Hornet_Fight_Yell_06", false);
                } else if(roll < 0.75f) {
                    playAudio("Hornet_Fight_Yell_08", false);
                } else if(roll <= 1f) {
                    playAudio("Hornet_Fight_Yell_09", false);
                } 
            }
        }

        public static void SlashHit(Collider2D otherCollider, GameObject gameObject){
            if(otherCollider.gameObject.layer == 11 && !HeroController.instance.cState.onGround){
                var roll = UnityEngine.Random.Range(0.0f, 1.0f);
                if(roll < 0.2f){
                    playAudio("Hornet_Fight_Laugh_01", false);
                } else if(roll < 0.4f) {
                    playAudio("Hornet_Fight_Laugh_02", false);
                }
            }
        }

        private static AudioSource audioSource;
        public static void playAudio(string clipName, bool force){
            audioSource = Silksong.ControllerScript.current.GetAddComponent<AudioSource>();
            if(audioSource.isPlaying && !force) { return;}
            if(AudioClips != null && AudioClips.TryGetValue(clipName,out var clip)){
                audioSource.PlayOneShot(clip);
            } 
        }
    }
}