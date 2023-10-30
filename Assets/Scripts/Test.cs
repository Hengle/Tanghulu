
    using AutoSingleton;
    using UnityEngine;

    public class Test : MonoBehaviour
    {
        private void Start()
        {
            Singleton<GameManager>.Instance.HP = 100f;
        }
    }

