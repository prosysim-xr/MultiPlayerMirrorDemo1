using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace MirrorDemoSks
{
    public class PlayerNamePopup : MonoBehaviour
    {
        private NetworkIdentity networtIdentity;
        private void Awake()
        {
            
        }
        private void Start()
        {
            networtIdentity = transform.parent.GetComponent<NetworkIdentity>();
            if (networtIdentity.isLocalPlayer)
            {
                var text_tmp = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
                text_tmp.text = PlayerPrefs.GetString("userName");
                Debug.Log(PlayerPrefs.GetString("userName") + "asdg");
            }
            
        }
        // Update is called once per frame
        void Update()
        {
           
        }
    }

}
