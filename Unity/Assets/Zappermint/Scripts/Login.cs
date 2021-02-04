using System;
using System.Web;
using UnityEngine.Events;

namespace Zappermint
{
    public class LoginResult
    {
        public LoginData data { get; }
        public LoginError error { get; }
        public LoginResult(LoginData data)
        {
            this.data = data;
            this.error = null;
        }
        public LoginResult(LoginError error)
        {
            this.data = null;
            this.error = error;
        }
    }

    public class LoginData
    {
        public string wallet { get; }
        public string name { get; }
        public LoginData(string wallet, string name)
        {
            this.wallet = wallet;
            this.name = HttpUtility.UrlDecode(name);
        }
    }

    public class LoginError
    {
        public string message { get; }
        public LoginError(string message)
        {
            this.message = HttpUtility.UrlDecode(message);
        }
    }

    [Serializable] public class LoginEvent : UnityEvent<LoginResult> { };
    [Serializable] public class LoginSuccessEvent : UnityEvent<LoginData> { };
    [Serializable] public class LoginFailEvent : UnityEvent<LoginError> { };
}