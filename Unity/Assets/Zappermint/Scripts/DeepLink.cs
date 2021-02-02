using UnityEngine;

namespace Zappermint
{
    public class DeepLink
    {
        public string scheme { get; }
        public string host { get; }
        public string query { get; }
        public DeepLink(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogWarning("Deep Link url is empty.");
                scheme = "";
                host = "";
                query = "";
                return;
            }

            var p = url.IndexOf(':');
            if (p == -1)
            {
                Debug.LogWarning($"Deep Link url is invalid: {url}");
                scheme = "";
                host = "";
                query = "";
                return;
            }

            scheme = url.Substring(0, p);

            var q = url.IndexOf('?');
            if (q == -1)
            {
                host = url.Substring(p + 3);
                query = "";
            }
            else
            {
                host = url.Substring(p + 3, q - p - 3);
                query = url.Substring(q + 1);
            }

            Debug.Log($"Deep Link: {url}");
        }
    }

#if UNITY_EDITOR
    public class DeepLinkDebugger : UnityEditor.EditorWindow
    {
        private string _url;

        [UnityEditor.MenuItem("Zappermint/Debug Deep Link")]
        public static void Open()
        {
            GetWindow<DeepLinkDebugger>().Show();
        }

        private void OnGUI()
        {
            _url = UnityEditor.EditorGUILayout.TextField("URL", _url);
            if (GUILayout.Button("Test"))
            {
                ZappermintLinkManager.Instance.OnDeepLinkActivated(_url);
            }
        }
    }
#endif
}
