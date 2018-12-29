using UnityEngine;

namespace RPG.InventorySystem
{
    [CreateAssetMenu(menuName = ("RPG/RPG.UI.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        public enum EquipLocation
        {
            Helmet,
            Necklace,
            Body,
            Trousers,
            Boots,
            Weapon,
            Shield,
            Gloves,
        }
        [SerializeField] EquipLocation _allowedEquipLocation;

        public EquipLocation allowedEquipLocation
        {
            get
            {
                return _allowedEquipLocation;
            }
        }

    }
}