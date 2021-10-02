
using System;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Satchel;
using static Satchel.AssemblyUtils;
using Silksong.Konpanion;

namespace Silksong
{
    public class Helpers{

        public static Sprite konpanioncard,hornetcard;
        public static GameObject CreateKonpanionCard(GameObject CardPrefab,Vector3 position){
            if(konpanioncard == null){
                konpanioncard = GetSpriteFromResources("konpanioncard.png",128f);
            }
            return CreateCardCustom(CardPrefab, position , konpanioncard);
        }
        public static GameObject CreateCard(GameObject CardPrefab,Vector3 position){
            if(hornetcard == null){
                hornetcard = GetSpriteFromResources("hornetcard.png",128f);
            }
            return CreateCardCustom(CardPrefab, position , hornetcard);
        }
        public static GameObject CreateCardCustom(GameObject CardPrefab,Vector3 position, Sprite sprite){
            var card = CardPrefab.createCompanionFromPrefab(true);
            card.name = "SilksongCard";
            card.layer = 13;
            card.transform.position = position;
            card.SetActive(true);
            var sr = card.GetAddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            return card;
        }
        public static GameObject CreateChangeling(GameObject go,string name)
        {
            var hero = HeroController.instance.gameObject;
            var changed = go.createCompanionFromPrefab();
            while(changed.RemoveComponent<Collider2D>()){};
            changed.name = name;
            changed.layer = hero.layer;
            changed.SetActive(true);
            changed.SetScale(hero.transform.localScale.y,hero.transform.localScale.y);
            changed.transform.position = hero.transform.position + new Vector3(0f, 0f, 0.001f);
            changed.transform.SetParent(hero.transform, true);
            return changed;
        }

        public static GameObject createKnightCompanion(GameObject ft = null){
            HeroController.instance.gameObject.SetActive(false);
            var knight = HeroController.instance.gameObject.createCompanionFromPrefab();
            HeroController.instance.gameObject.SetActive(true);
            knight.RemoveComponent<HeroController>();
            knight.RemoveComponent<HeroAnimationController>();
            knight.RemoveComponent<HeroAudioController>();
            knight.RemoveComponent<ConveyorMovementHero>();

            knight.name = "konpanion";
            knight.GetAddComponent<MeshRenderer>().enabled = true;
            knight.GetAddComponent<Rigidbody2D>().gravityScale = 1f;

            //add control and adjust parameters
            var kc = knight.GetAddComponent<KonpanionController>();
            kc.moveSpeed = 10f;
            kc.followDistance = 2f;
            kc.IdleShuffleDistance = 0.01f;

            if(ft != null){
                kc.followTarget = ft;
            }

            //fix up collider size
            var collider = knight.GetAddComponent<BoxCollider2D>();
            collider.size = new Vector2(1f,2.0f);
            collider.offset = new Vector2(0f,-0.4f);
            collider.enabled = true;

            // add animations
            kc.Animations.Add(State.Idle,"Idle");
            kc.Animations.Add(State.IdleFidget1,"Acid Death");
            kc.Animations.Add(State.IdleFidget2,"Prostrate");
            kc.Animations.Add(State.Walk,"Run");
            kc.Animations.Add(State.Turn,"Map Walk");
            kc.Animations.Add(State.Teleport,"Fall");
            kc.Animations.Add(State.Jump,"Airborne");

            knight.SetActive(true);
            return knight;
        }

        public static GameObject CreateYoinkCityLogo(GameObject tcl){
            var ycl =new GameObject();
            ycl.name = "YoinkCityLogo";
            ycl.transform.position = tcl.transform.position + new Vector3(0.75f, 0.2f, 0f);
            ycl.transform.localScale = new Vector3(0.11f,0.11f,1f);
            ycl.transform.SetParent(tcl.transform, true);
            var sr = ycl.GetAddComponent<SpriteRenderer>();
            sr.sprite = GetSpriteFromResources("YoinkCity.png");
            return ycl;
        }
    }
}