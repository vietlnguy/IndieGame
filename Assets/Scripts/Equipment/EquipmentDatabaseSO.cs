using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "IndieGame/Equipment Database")]
public class EquipmentDatabaseSO : ScriptableObject
{
    public List<EquipmentSO> equipment;
}
