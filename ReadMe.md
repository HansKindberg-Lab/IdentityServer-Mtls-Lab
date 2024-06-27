# IdentityServer-Mtls-Lab

This project/solution was initially created with:

	dotnet new isinmem --name IdP

using Duende.IdentityServer.Templates 7.0.4.

## Urls

- https://idp.example.local
- https://mtls.idp.example.local

## Adjustments

- [applicationUrl in launchSettings.json](/Source/IdP/Properties/launchSettings.json#L9)
- [appsettings.json with MTLS settings](/Source/IdP/appsettings.json)
- Added entries in *C:\Windows\System32\drivers\etc\hosts*
	- 127.0.0.1 idp.example.local
	- 127.0.0.2 mtls.idp.example.local
- [Added nuget-package Microsoft.AspNetCore.Authentication.Certificate](/Source/IdP/IdP.csproj#L11)
- [MutualTls settings in HostingExtensions.cs](/Source/IdP/HostingExtensions.cs#L25)
- [Certificate-authentication in HostingExtensions.cs](/Source/IdP/HostingExtensions.cs#L50)
- [Optional MTLS settings](/Source/IdP/Program.cs#L16)

## Certificates

The certificate:

- [https.crt](/.certificates/https.crt)
- [https.key](/.certificates/https.key)

was created with the following command:

	cd {SOLUTION-PATH}\.certificates

	openssl req -x509 -newkey rsa:2048 -sha256 -days 365000 -nodes -keyout https.key -out https.crt -subj "/CN=idp.example.local" -addext "subjectAltName=DNS:idp.example.local,DNS:mtls.idp.example.local,IP:127.0.0.1,IP:127.0.0.2"

Found it here:

- [Specify Subject Alternative Name when generating a self signed certificate](https://stackoverflow.com/questions/33138148/specify-subject-alternative-name-when-generating-a-self-signed-certificate#answer-64344995)

## OpenID Connect Discovery

### https://idp.example.local/.well-known/openid-configuration - correct

	{
		"issuer": "https://idp.example.local",
		"jwks_uri": "https://idp.example.local/.well-known/openid-configuration/jwks",
		"authorization_endpoint": "https://idp.example.local/connect/authorize",
		"token_endpoint": "https://idp.example.local/connect/token",
		"userinfo_endpoint": "https://idp.example.local/connect/userinfo",
		"end_session_endpoint": "https://idp.example.local/connect/endsession",
		"check_session_iframe": "https://idp.example.local/connect/checksession",
		"revocation_endpoint": "https://idp.example.local/connect/revocation",
		"introspection_endpoint": "https://idp.example.local/connect/introspect",
		"device_authorization_endpoint": "https://idp.example.local/connect/deviceauthorization",
		"backchannel_authentication_endpoint": "https://idp.example.local/connect/ciba",
		"pushed_authorization_request_endpoint": "https://idp.example.local/connect/par",
		"require_pushed_authorization_requests": false,
		"mtls_endpoint_aliases": {
			"token_endpoint": "https://mtls.idp.example.local/connect/token",
			"revocation_endpoint": "https://mtls.idp.example.local/connect/revocation",
			"introspection_endpoint": "https://mtls.idp.example.local/connect/introspect",
			"device_authorization_endpoint": "https://mtls.idp.example.local/connect/deviceauthorization"
		},
		"frontchannel_logout_supported": true,
		"frontchannel_logout_session_supported": true,
		"backchannel_logout_supported": true,
		"backchannel_logout_session_supported": true,
		"scopes_supported": [ "openid", "profile", "scope1", "scope2", "offline_access" ],
		"claims_supported": [ "sub", "name", "family_name", "given_name", "middle_name", "nickname", "preferred_username", "profile", "picture", "website", "gender", "birthdate", "zoneinfo", "locale", "updated_at" ],
		"grant_types_supported": [ "authorization_code", "client_credentials", "refresh_token", "implicit", "password", "urn:ietf:params:oauth:grant-type:device_code", "urn:openid:params:grant-type:ciba" ],
		"response_types_supported": [ "code", "token", "id_token", "id_token token", "code id_token", "code token", "code id_token token" ],
		"response_modes_supported": [ "form_post", "query", "fragment" ],
		"token_endpoint_auth_methods_supported": [ "client_secret_basic", "client_secret_post", "tls_client_auth", "self_signed_tls_client_auth" ],
		"id_token_signing_alg_values_supported": [ "RS256" ],
		"subject_types_supported": [ "public" ],
		"code_challenge_methods_supported": [ "plain", "S256" ],
		"request_parameter_supported": true,
		"request_object_signing_alg_values_supported": [ "RS256", "RS384", "RS512", "PS256", "PS384", "PS512", "ES256", "ES384", "ES512", "HS256", "HS384", "HS512" ],
		"prompt_values_supported": [ "none", "login", "consent", "select_account" ],
		"authorization_response_iss_parameter_supported": true,
		"tls_client_certificate_bound_access_tokens": true,
		"backchannel_token_delivery_modes_supported": [ "poll" ],
		"backchannel_user_code_parameter_supported": true,
		"dpop_signing_alg_values_supported": [ "RS256", "RS384", "RS512", "PS256", "PS384", "PS512", "ES256", "ES384", "ES512" ]
	}

### https://mtls.idp.example.local/.well-known/openid-configuration - correct

	{
		"issuer": "https://idp.example.local",
		"jwks_uri": "https://mtls.idp.example.local/.well-known/openid-configuration/jwks",
		"authorization_endpoint": "https://mtls.idp.example.local/connect/authorize",
		"token_endpoint": "https://mtls.idp.example.local/connect/token",
		"userinfo_endpoint": "https://mtls.idp.example.local/connect/userinfo",
		"end_session_endpoint": "https://mtls.idp.example.local/connect/endsession",
		"check_session_iframe": "https://mtls.idp.example.local/connect/checksession",
		"revocation_endpoint": "https://mtls.idp.example.local/connect/revocation",
		"introspection_endpoint": "https://mtls.idp.example.local/connect/introspect",
		"device_authorization_endpoint": "https://mtls.idp.example.local/connect/deviceauthorization",
		"backchannel_authentication_endpoint": "https://mtls.idp.example.local/connect/ciba",
		"pushed_authorization_request_endpoint": "https://mtls.idp.example.local/connect/par",
		"require_pushed_authorization_requests": false,
		"mtls_endpoint_aliases": {
			"token_endpoint": "https://mtls.idp.example.local/connect/token",
			"revocation_endpoint": "https://mtls.idp.example.local/connect/revocation",
			"introspection_endpoint": "https://mtls.idp.example.local/connect/introspect",
			"device_authorization_endpoint": "https://mtls.idp.example.local/connect/deviceauthorization"
		},
		"frontchannel_logout_supported": true,
		"frontchannel_logout_session_supported": true,
		"backchannel_logout_supported": true,
		"backchannel_logout_session_supported": true,
		"scopes_supported": [ "openid", "profile", "scope1", "scope2", "offline_access" ],
		"claims_supported": [ "sub", "name", "family_name", "given_name", "middle_name", "nickname", "preferred_username", "profile", "picture", "website", "gender", "birthdate", "zoneinfo", "locale", "updated_at" ],
		"grant_types_supported": [ "authorization_code", "client_credentials", "refresh_token", "implicit", "password", "urn:ietf:params:oauth:grant-type:device_code", "urn:openid:params:grant-type:ciba" ],
		"response_types_supported": [ "code", "token", "id_token", "id_token token", "code id_token", "code token", "code id_token token" ],
		"response_modes_supported": [ "form_post", "query", "fragment" ],
		"token_endpoint_auth_methods_supported": [ "client_secret_basic", "client_secret_post", "tls_client_auth", "self_signed_tls_client_auth" ],
		"id_token_signing_alg_values_supported": [ "RS256" ],
		"subject_types_supported": [ "public" ],
		"code_challenge_methods_supported": [ "plain", "S256" ],
		"request_parameter_supported": true,
		"request_object_signing_alg_values_supported": [ "RS256", "RS384", "RS512", "PS256", "PS384", "PS512", "ES256", "ES384", "ES512", "HS256", "HS384", "HS512" ],
		"prompt_values_supported": [ "none", "login", "consent", "select_account" ],
		"authorization_response_iss_parameter_supported": true,
		"tls_client_certificate_bound_access_tokens": true,
		"backchannel_token_delivery_modes_supported": [ "poll" ],
		"backchannel_user_code_parameter_supported": true,
		"dpop_signing_alg_values_supported": [ "RS256", "RS384", "RS512", "PS256", "PS384", "PS512", "ES256", "ES384", "ES512" ]
	}