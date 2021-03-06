
using System;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Satchel;
using static Satchel.AssemblyUtils;

namespace Silksong
{
    public class Helpers{

        public static Sprite hornetcard;
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