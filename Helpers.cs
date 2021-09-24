
using System;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Satchel;

namespace Silksong
{
    public class Helpers{
        public static GameObject CreateChangeling(GameObject go,string name)
        {
            var hero = HeroController.instance.gameObject;
            var changed = go.createCompanionFromPrefab();
            changed.name = name;
            changed.layer = hero.layer;
            changed.SetActive(true);
            changed.transform.position = hero.transform.position + new Vector3(0f, 0f, 0.001f);
            changed.transform.SetParent(hero.transform, true);
            return changed;
        }
    }
}