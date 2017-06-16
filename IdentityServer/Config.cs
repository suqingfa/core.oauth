using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Config
    {
        /// <summary>
        /// 定义API资源
        /// 要允许客户端请求访问API的访问令牌，您需要定义API资源，例如
        /// 要获取API的访问令牌，还需要将其注册为范围。此时范围类型为资源类型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                // 使用客户端证书保护API
                new ApiResource("api1", "My API")
            };
        }

        /// <summary>
        /// 定义客户
        /// 客户端代表可以从您的身份服务器请求令牌的应用程序。
        /// 细节各不相同，但您通常为客户定义以下常见设置
        /// 
        ///     唯一的客户端ID
        ///     一个秘密，如果需要的话
        ///     允许与令牌服务的交互（称为授权类型）
        ///     身份访问令牌发送到的网络位置（称为重定向URI）
        ///     允许客户端访问的范围列表（也称为资源）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // 使用客户端证书保护API
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },

                // 资源所有者密码授予添加客户端
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile
                    }
                },

                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris =           { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
            };
        }

        /// <summary>
        /// 定义身份资源
        /// 身份资源是用户的用户名，姓名或电子邮件地址等数据。
        /// 身份资源具有唯一的名称，您可以为其分配任意声明类型。
        /// 然后，这些声明将被包含在用户的身份令牌中。
        /// 客户端将使用scope参数来请求访问身份资源。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                // 范围代表您想要保护的内容，客户端希望访问
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            };
        }
    }
}
