using UnityEngine;
using UnityEngine.UI;

public class HandSlot : MonoBehaviour
{
    [SerializeField] private Text manaCost;

    public void SetMana(string v) => manaCost.text = v;
}