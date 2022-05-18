using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GroundbreakerRune : InteractiveItem
{
    private bool m_hasBeenPicked = false;
    [SerializeField] DialogueEvent m_dialogue;
    private Material mat;
    private Light2D lig;
    private float brightness_val = 0;
    private bool forward_anim = true;

    protected override void Awake()
    {
        mat = GetComponentInChildren<SpriteRenderer>().material;
        lig = GetComponentInChildren<Light2D>();
    }
    protected override void Update()
    {
        base.Update();
        if (brightness_val > 1.2)
        {
            forward_anim = false;
        } 
        if (brightness_val < 0.2) { forward_anim = true; }

        if (forward_anim) { brightness_val += Time.deltaTime*2f; }else { brightness_val -= Time.deltaTime*2f; }

        AnimateShader();
    }
    private void AnimateShader()
    {
        mat.SetFloat("_Brightness", brightness_val);
        lig.intensity = brightness_val;
    }

    protected override void HandleInteraction()
    {
        if (m_hasBeenPicked){ return; }
        base.HandleInteraction();
        SoundManager.Instance.PlayOnce(AudioClipName.ITEM_PICK_UP);
        m_hasBeenPicked = true;
        m_dialogue.addListenerToDialogueFinish(UnlockGroundBreaker);
    }

    private void UnlockGroundBreaker() {
        GameManager.Instance.SetIsSkillAvailable(SKILLS.GROUNDBREAKER, true);
        m_dialogue.removeListenerToDialogueFinish(UnlockGroundBreaker);
        gameObject.SetActive(false);
    }
}