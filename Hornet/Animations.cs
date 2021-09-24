using System.Collections.Generic;
namespace Silksong.Hornet {
    public class Animations {
        public static Dictionary<string,string> BossClips =  new Dictionary<string,string>()
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
            {"SlashAlt","Throw"},
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
            {"Collect Normal 1","Stun"},
            {"Collect Normal 2","Stun"},
            {"Collect Normal 3","Flourish"},
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
            {"Surface Idle","Fall"},
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
            {"NA Big Slash","Throw"},
            {"NA Dash Slash","Throw"},
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
        
        public static Dictionary<string, string> NpcClips = new Dictionary<string, string>()
        {
            {"Sit","Den Idle"},
            {"Sit Idle","Den Idle R"},
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
    }
}