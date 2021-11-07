using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    #region Singleton

        public static BuffManager Instance => _instance;
        
        private static BuffManager _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

    #endregion

    public List<Buff> AvalibleBuffs;
    public List<Buff> AvalibleDebuffs;
    [Range(0, 100)] public float BuffChance;
    [Range(0, 100)] public float DebuffChange;
}
