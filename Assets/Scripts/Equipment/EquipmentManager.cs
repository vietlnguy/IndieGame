using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : SingletonMonoBehaviour<EquipmentManager>
{
    public EquipmentDatabaseSO database;

    private Dictionary<string, EquipmentSO> lookup;

    protected override void Awake()
    {
        base.Awake();
        BuildLookup();
    }

    private void BuildLookup()
    {
        lookup = new Dictionary<string, EquipmentSO>();
        if (database == null || database.equipment == null || database.equipment.Count == 0)
        {
            Debug.LogError("EquipmentManager: no equipment database assigned on " + gameObject.name + ".");
            return;
        }
        foreach (EquipmentSO equipment in database.equipment)
        {
            if (equipment != null)
            {
                lookup[equipment.name] = equipment;
            }
        }
    }

    public static EquipmentSO Get(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        if (Instance == null)
        {
            Debug.LogError("EquipmentManager: no manager loaded, cannot resolve '" + name + "'.");
            return null;
        }
        if (Instance.lookup.TryGetValue(name, out EquipmentSO equipment))
        {
            return equipment;
        }
        Debug.LogWarning("EquipmentManager: no equipment named '" + name + "' in the database.");
        return null;
    }

    public static Equipment Create(string name)
    {
        EquipmentSO equipment = Get(name);
        if (equipment == null)
        {
            return null;
        }
        return equipment.ToEquipment();
    }
}
