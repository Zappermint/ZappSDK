# Zapp SDK
Zappermint relies on Deeplinks to smoothly integrate the login system into your app. This is a double deeplink: one from your app to Zapp, and one from Zapp back to your app. Therefore, your Unity project must be setup properly to recognize and catch deeplinks on Android and iOS devices.

## Setup
### All platforms
- In your first scene (this is, the scene at build index 0), add a GameObject with the `ZappLinkManager` script. 
- In your Login scene, add a GameObject with the `ZappLogin` script. Fill in your App name, App icon, Scheme and the Cost of the app.

### iOS
- In the Project Settings > Player > Other Settings, you'll find `Supported URL schemes`. Add your scheme to the list.

### Android
- In the `AndroidManifest.xml` file of your project, add the `VIEW intent-filter` with the scheme set to your scheme.

_You can find an example AndroidManifest in _`Zappermint/Examples`_. You can copy the _`VIEW intent-filter`_ from there into your own manifest file, or you can copy the whole file to _`Plugins/Android`_ if you don't have one yet. Make sure to replace _`YOUR_SCHEME`_ on line 13 with your own scheme._

## Usage
### ZappLinkManager
This is a Singleton that you can access in any script, using `ZappLinkManager.Instance`. The Manager will contain the user's data once logged in. 
```CSharp
string wallet = ZappLinkManager.Instance.Account.wallet; // Wallet's public address
string name = ZappLinkManager.Instance.Account.name; // Name of the Wallet
```
Use the `IsLoggedIn` state to hide ads when logged in. Below is an example of how you could implement this.

```CSharp
// Note: AdManager is just an example. It doesn't exist in the Zapp SDK.

private void Awake() 
{
    if (ZappLinkManager.Instance.IsLoggedIn) // True if login was successful
    {
        AdManager.RemoveAds(); // Remove ads from the UI
    }
}
```

### ZappLogin
This component contains the Zapp configuration of your project.
- App Name: the name of your app. This will be displayed on the login page of the Zapp Wallet app.
- App Icon: the icon of your app. This will be displayed on the login page of the Zapp Wallet app.
- Scheme: the scheme used for deeplinking. This must match the scheme in AndroidManifest and/or Project Settings.
- Cost: the amount of ZAPP users need to stake to play your game ad-free. This will move to the NFT in the future. Read more about the staking concept and effects on [our website](https://zappermint.com).
- OnLogin: event fired when login completes. The `LoginResult` parameter contains the login data if successful, the login error if not.
- OnSuccess: event fired when login completes successfully, with the login data stored in the `LoginData` parameter.
- OnFail: event fired when login completes unsucessfully, with the login error stored in the `LoginError` parameter.

The script has a `Login()` function that calls the Zapp Wallet deeplink. You can use this as a Unity Button onClick event listener, or call it manually from your own script.

The events can be used through the Inspector, or in code. An example of how to use it in code:
```CSharp
// Note: AdManager, Account and ErrorManager are just examples. These don't exist in the Zapp SDK.

[SerializeField] private ZappLogin _login;

private void Start() 
{
    // OnLogin always fires. You need to check whether it's successful or not
    _login.OnLogin += OnLogin;
    // Alternatively, use the separate events
    _login.OnSuccess += OnSuccess;
    _login.OnFail += OnFail;
}

private void OnLogin(LoginResult result) 
{
    if (result.error == null) 
    {
        // Remove ads from the UI
        AdManager.RemoveAds(); 
        // Display the Zapp account name
        Account.SetName(result.data.Name);
    }
    else
    {
        // Display the reason for failed login
        ErrorManager.Popup(result.error.message); 
    }
}

private void OnSuccess(LoginData data) 
{
    // Remove ads from the UI
    AdManager.RemoveAds();
    // Display the Zapp account name
    Account.SetName(data.name);
}

private void OnFail(LoginError error) 
{
    // Display the reason for failed login
    ErrorManager.Popup(error.message); 
}
```

## Testing
The Zapp SDK will add a Menu Item to your Unity Project. In the menu `Zappermint`, select `Debug Deep Link`. In the window that opened, paste the URL below into the `URL` field, run your app and click on `Test`.
```
// Replace YOUR_SCHEME with your app's deeplink scheme!
// Test successful login
YOUR_SCHEME://zappermint.login.success?w=0xd440809b78553896271699EF075717F2bC28B27E&n=Test+Wallet

// Test failed login
YOUR_SCHEME://zappermint.login.fail?e=Canceled
```

## Troubleshooting
### [Your app → Zapp] not working
If your Login button opens the play store instead of the Zapp Wallet, the Zapp Wallet hasn't been installed on the device. If `Login()` opens neither, please contact us.

### [Zapp → Your app] not working
If the Zapp Wallet doesn't go back to your app, the Deeplink of your app is not setup properly. Double check the Scheme of the Deeplink in all locations mentioned in the Setup section. For Android, make sure your manifest is valid. Read more about manifests on the [Android Developer guide](https://developer.android.com/guide/topics/manifest/manifest-intro) and [Unity Manual](https://docs.unity3d.com/Manual/android-manifest.html).

### Other issues
Contact Zappermint:

**[Email](mailto:hello@zappermint.com) | [Discord](https://discord.gg/4R28ZVQgVk) | [Telegram](https://t.me/Zappermint) | [Twitter](https://twitter.com/ZappermintApp) | [GitHub](https://github.com/Zappermint/ZappermintSDK)**
