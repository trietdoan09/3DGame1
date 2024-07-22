using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatStaminaModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val, int stat)
    {
        PlayerManager player = character.GetComponent<PlayerManager>();
        if (player != null)
            player.AddStat(val, stat);
    }
}
