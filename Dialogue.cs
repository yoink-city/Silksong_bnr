
using System;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using Satchel;

namespace Silksong
{
    public class Dialogue{
        
        public static Dictionary<string,string> KeyMap = new Dictionary<string,string>(){
            {$"HORNET_GREENPATH",$"Come no closer, imposter.<page>I've seen you, creeping through the undergrowth, stalking me.<page>Return back to your nest... And await the fools that will fall for your deciet...<page>I know what you are. I know what you try to do. But i am no prey."},
            {$"HORNET_FOUNTAIN_1",$"Again we meet, imposter.<page>I'm normally quite perceptive. You I do not understand, though I've since sensed part of the truth.<page>You're from beyond this kingdom's bounds. Yours is a resilience born of adversity.<page>It's no surprise then that you've managed to reach the heart of this world. But you now know the sacrifice that keeps it standing."},
            {$"HORNET_FOUNTAIN_2",$"It's also best you know that Hallownest does not require outsiders to meddle with it's affairs, I for one shall not allow it, cease this path at once."},
            {$"HORNET_OUTSKIRTS_1",$"So you still continue to pursue the deeper truth? It is neither one for outsiders, nor something the weak could bear."},
            {$"HORNET_OUTSKIRTS_2",$"I stand in your way and I'll not hold back. My needle is lethal and I'd feel no sadness in a weakling's demise.<page>I accept this Kingdom's past and claim responsibility for its future. Your encroachment on my form, I tolerate. But on my duty I will not."},
            {$"HORNET_OUTSKIRTS_DEFEAT",$"...So strong...<page>The way you fight... The way your wield that needle...<page> But how can it be? That you are me? <page>I know not how it came to be, but I recognize that you and I are one and the same, and that you are wrapped in another power, one not within my reach.<page>So then do it, Daughter of Hallownest! Head onward. Burn that mark upon your shell and claim yourself as Queen."},
            {$"HORNET_ABYSS_ASCENseeT_01",$"I see you've faced the place of their birth, and now drape yourself in the substance of its shadow.<page>Though our strength is born of the same source, that part of you, that crucial emptiness, I do not share.<page>Funny then, that such darkness gives me hope. Within it, I see the chance of change.<page>A difficult journey you would face, but a choice it can create. Prolong our world's stasis or face the heart of its infection."},
            {$"HORNET_ABYSS_REPEAT",$"I'd urge you to take that harder path, but what end may come, the decision rests with you."},
            {$"HORNET_ABYSS_DREAM",$"...She faced the void, and ascends unscathed... Could I unite such vast darkness?.."},
            {$"HORNET_SPIDER_TOWN_01",$"So you've slain our mother... And you head towards that fated goal.<page>I'd not have obstructed this happening, but it caused me some pain to knowingly stand idle.<page>I know it caused you pain to do this, you're not completely cold.<page>We did not choose our mother, or the circumstance into which we were born. Despite all the ills of this world, I'm thankful for the life she granted me.<page>It's quite a debt we owed. Only in allowing her to pass, and taking the burden of the future in her stead, can we begin to repay it."},
            {$"HORNET_SPIDER_TOWN_REPEAT",$"Leave me now. Allow me a moment alone before this bedchamber becomes forever a shrine."},
            {$"HORNET_SPIDER_TOWN_DREAM",$"...Mother... Forgive my inaction... but with her another path may be possible..."},
            {$"HORNET_DOOR_UNOPENED",$"I'm impressed. You've burdened yourself with the fate of this world, yet you still stand strong.<page>To break the Dreamer's seals would alone be considered an impossible task, but to take that void inside yourself, that casts you as something rather exceptional.<page>To do this much, for a world that is not your own, what horrors you must have witnessed before deciding to come here."},
            {$"HORNET_PRE_FINAL_BATTLE",$"The path is opened. One way or another an end awaits inside.<page>I won't be joining you in this. That space is built to sustain their likes. Its bindings would drain me were I to join.<page>Don't be surprised. I'll not risk my own life in your attempt, though if the moment presents I'll aid as I'm able."},
            {$"HORNET_PRE_FINAL_BATTLE_REPEAT",$"Daughter of Hallownest, you possess the strength to enact an end of your choosing. Would you supplant our birth-cursed sibling, or would you transcend it?"},
            {$"HORNET_PRE_FINAL_BATTLE_DREAM",$"...Could she achieve that impossible thing? Could I?"},
            {$"NAME_HORNET",$"Hornet"},
            {$"DESC_HORNET",$"Skilled protector of Hallownest's ruins. Wields a needle and thread. My younger self."},
            {$"SHAMAN_MEET",$"Oho! Who is that creeping out of the darkness? My, you're looking grim! A strange, empty face and a wicked looking weapon!<page>Something important has drawn you back into Hallownest's corpse, but I won't ask what. Perhaps the reason you've found me is because you need my help?<page>Say no more, friend. I'm going to give you a gift, a nasty little spell of my own creation. It's just perfect for one like you! Ohoho!"},
            {$"SHAMAN_RETURN",$"Oho! What brings you back through here, wanderer? Are you lost?<page>Don't worry about me. I don't need anything more from you. Ohohoho!"},
            {$"QUEEN_MEET",$"Oh! The Gendered Child arrives. Far she walks to find me. Does she seek my aid? Or did the path carry her by chance to so pertinant a place?<page>It is true. True, that your arrival is unexpected. But it is true one like you was awaited.<page>I have a gift, held long for one of your kind. Half of a whole. When united, great power is granted, and on the path ahead, great power she will need."},
            {$"QUEEN_DREAM",$"So she can access a mind? Then the seals shall break before her blade."},
            {$"QUEEN_TALK_01",$"Within my roots, the weakening of the Vessel I plainly feel. Only two obvious outcomes there are from such a thing.<page>The first is inevitable on current course, regression, all minds relinquished to that pernicious plague.<page>The second I find preferable, and would seek your aid in its occurance, replacement.<page>I implore you, usurp the Vessel. Its supposed strength was ill-judged. It was tarnished by an idea instilled. But you. You are now free of such blemishes. You could contain that thing inside."},
            {$"QUEEN_REPEAT_SHADECHARM",$"That pulsing emptiness... Truly, she has been transformed by the revelations she found.<page>Does she... feel anything? Triumph? Or hate? If she does, I cannot sense it.<page>The fate of our Kingdom, our Hallownest... That future belongs to you now."},
            {$"QUEEN_HORNET",$"She faced her younger self? She's a fierce foe, already strong in mind and body, striking reflection of her mother, though the two were permitted little time together.<page>I never begrudged the Wyrm's dalliance as bargain. In fact, I feel some affection for the creature birthed.<page>If your paths were ever to align again, I imagine you might gain yourself a powerful ally, in yourself."},
            {$"QUEEN_REPEAT_KINGSOUL",$"Ahh! So she bears our once-fractured soul, now complete. Such strength, such resolve, such dedication! Is she more than simply a traveller? I almost feel like I'm once again in the presence of my beloved Wyrm.<page>The Kingsoul... What is at the heart of it I wonder? If her curiosity wills her, she should seek out that place. That place where they were born, where they died, where it began..."},
            {$"GRIMM_DREAM",$"Masterful! Even through your travels in time you still bear fierce strength.<page>Fine craft dear Wyrm, and perfect tool to prolong the heart of Grimm."},
            {$"QUEEN_GRIMMCHILD_FULL",$"Your companions eyes burn with a familiar flame... Success then for the scarlet heart, and irony, to use the Wyrms spawn to grow its own.<page>I know you creature, and the form time shall bring. You may be all and one, clan and master, but this land shall never bear so foreign a king."},
            {$"DIVINE_DREAM",$"String and fire will dance together so prettily, I think..."},
        };
    }
}
