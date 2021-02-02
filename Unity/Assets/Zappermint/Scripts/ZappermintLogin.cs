using System.Collections;
using UnityEngine;

namespace Zappermint
{
    public class ZappermintLogin : MonoBehaviour
    {
        [Tooltip("Deeplink scheme. Must match AndroidManifest and/or Project Settings")]
        public string Scheme = "";
        [Tooltip("Amount of ZAPP user must stake to use app ad-free")]
        public int Cost = 10;

        [Tooltip("Event fired when login completes")]
        public LoginEvent OnLogin = new LoginEvent();
        [Tooltip("Event fired when login completes successfully")]
        public LoginSuccessEvent OnSuccess = new LoginSuccessEvent();
        [Tooltip("Event fired when login completes unsuccessfully")]
        public LoginFailEvent OnFail = new LoginFailEvent();

        private Coroutine _loginFallback;

        private void OnEnable()
        {
            ZappermintLinkManager.Instance.Handler += LinkHandler;
        }

        private void OnDisable()
        {
            ZappermintLinkManager.Instance.Handler -= LinkHandler;

        }

        /// <summary>
        /// Logs in through the Zapp Wallet
        /// </summary>
        public void Login()
        {
            Application.OpenURL($"zappermint://login?c={Cost}&r={Scheme}");
            _loginFallback = StartCoroutine(OpenStore());
        }

        private IEnumerator OpenStore()
        {
            yield return new WaitForSeconds(2f);
#if UNITY_EDITOR || UNITY_ANDROID
            Application.OpenURL($"https://play.google.com/store/apps/details?id=com.ZappermintBV.ZappWallet");
#elif UNITY_IOS
            Application.OpenURL($"https://itunes.apple.com/us/app/zappermint/id1234");
#endif
            _loginFallback = null;
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause && _loginFallback != null)
            {
                StopCoroutine(_loginFallback);
                _loginFallback = null;
            }
        }

        /// <summary>
        /// Handles the Deeplink for logging in
        /// </summary>
        /// <param name="link"></param>
        private void LinkHandler(DeepLink link)
        {
            ZappermintLinkManager manager = ZappermintLinkManager.Instance;
            LoginResult result;
            switch (link.host)
            {
                case "zappermint.login.success":
                    result = new LoginResult(new LoginData(manager.GetParameter("w"), manager.GetParameter("n")));
                    manager.Account = new Account(result.data);
                    OnSuccess.Invoke(result.data);
                    break;
                case "zappermint.login.fail":
                    result = new LoginResult(new LoginError(manager.GetParameter("e")));
                    OnFail.Invoke(result.error);
                    break;
                default:
                    result = default;
                    Debug.LogWarning($"Zappermint Deep Link unrecognized host '{link.host}'.");
                    break;
            }
            OnLogin.Invoke(result);
        }
    }
}