using UnityEngine;
using UnityEngine.UI;
using OmniGiovanni.Web;

namespace OmniGiovanni.Example
{
    public class TwitchTokenRequestHandler : MonoBehaviour
    {
        [SerializeField] private Button authenticateButton;
        [SerializeField] private Authentication appAuthentication;
        public AccessTokenResponse tokenResponse;

        private void Start()
        {
            if (appAuthentication == null)
            {
                appAuthentication = new Authentication();
            }

            appAuthentication.OnAuthorizationCodeReceived += HandleAuthorizationCode;

            if (authenticateButton != null)
            {
                authenticateButton.onClick.AddListener(HandleOAuth);
            }
        }

        void Awake()
        {
            UnityMainThreadDispatcher.Instance();
        }

        private void HandleOAuth()
        {
            authenticateButton.interactable = false;

            string scope = "chat:read";

            appAuthentication.Start(scope);
        }

        private void HandleAuthorizationCode(string code)
        {

            tokenResponse = JsonUtility.FromJson<AccessTokenResponse>(code);
            Debug.Log($"Access Token: {tokenResponse.access_token}");

            //Hide the button after the authorization code is retrieved and do other stuff needed. 
            authenticateButton.gameObject.SetActive(false);
            // other stuff



        }


        private void OnDestroy()
        {
            appAuthentication.OnAuthorizationCodeReceived -= HandleAuthorizationCode;
            appAuthentication.Stop();
        }
    }
}
