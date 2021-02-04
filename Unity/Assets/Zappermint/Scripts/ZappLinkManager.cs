using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zappermint
{
    [DefaultExecutionOrder(-1000000)]
    public class ZappLinkManager : MonoBehaviour
    {
        public static ZappLinkManager Instance { get; private set; }
        public Account Account { get; internal set; }
        public bool IsLoggedIn => Account != null;
        public Action<DeepLink> Handler { get; internal set; }
        
        private Dictionary<string, string> _parameters = new Dictionary<string, string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Application.deepLinkActivated += OnDeepLinkActivated;
                if (!string.IsNullOrEmpty(Application.absoluteURL)) OnDeepLinkActivated(Application.absoluteURL);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnDeepLinkActivated(string url)
        {
            var link = new DeepLink(url);
            if (link.host.Split('.')[0] != "zappermint") return; // Deep Link source doesn't stem from Zapp Wallet
            ParseParameters(link);
            Handler?.Invoke(link);
        }

        private void ParseParameters(DeepLink link)
        {
            _parameters.Clear();
            var parameters = link.query.Split('&');
            foreach (var parameter in parameters)
            {
                var i = parameter.IndexOf('=');
                if (i < 0) continue;
                _parameters.Add(parameter.Substring(0, i), parameter.Substring(i + 1));
            }
        }

        public string GetParameter(string p)
        {
            if (_parameters.ContainsKey(p)) return _parameters[p];
            return "";
        }
    }
}