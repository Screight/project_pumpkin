using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUra : AnimatedCharacter
{
    public void LoseHeart(){
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.LIFE_URA_DOWN, false);
    }

    public void GainHeart(){
        AnimationManager.Instance.PlayAnimation(this, ANIMATION.LIFE_URA_UP, false);
    }
}
