using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChangeLog : ISerializationCallbackReceiver {
    public Dictionary<CellID, Vector2> PlayerLocations = new Dictionary<CellID, Vector2>();
    public Dictionary<Vector2, CellID> ChangedCells = new Dictionary<Vector2, CellID>();

    public List<CellID> _locKeys = new List<CellID>();
    public List<Vector2> _locVals = new List<Vector2>();
    public List<Vector2> _cellKeys = new List<Vector2>();
    public List<CellID> _cellVals = new List<CellID>();
 
    public void OnBeforeSerialize()
    {
        _locKeys.Clear();
        _locVals.Clear();        
        foreach (var kvp in PlayerLocations)
        {
            _locKeys.Add(kvp.Key);
            _locVals.Add(kvp.Value);
        }

        _cellKeys.Clear();
        _cellVals.Clear();
        foreach (var kvp in ChangedCells)
        {
            _cellKeys.Add(kvp.Key);
            _cellVals.Add(kvp.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        PlayerLocations.Clear();
        for (int i = 0; i != Mathf.Min(_locKeys.Count, _locVals.Count); i++)
        {
            PlayerLocations.Add(_locKeys[i], _locVals[i]);
        }

        ChangedCells.Clear();
        for (int i = 0; i != Mathf.Min(_cellKeys.Count, _cellVals.Count); i++)
        {
            ChangedCells.Add(_cellKeys[i], _cellVals[i]);
        }
    }
}
