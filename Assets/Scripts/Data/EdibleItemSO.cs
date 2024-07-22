using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifierData = new List<ModifierData>();
        public string ActionName => "Consume";

        public AudioClip actionSFX {get; private set;}

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach(ModifierData data in modifierData)
            {
                data.statModifier.AffectCharacter(character, data.value, data.stat);
            }
            return true;
        }
    }
    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }
    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
        public int stat;
    }
}