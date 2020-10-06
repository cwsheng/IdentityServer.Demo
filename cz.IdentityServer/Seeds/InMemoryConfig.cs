using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cz.IdentityServer
{
    public class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                //必须要添加，否则报无效的scope错误
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        /// <summary>
        /// api资源列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            //可访问的API资源(资源名，资源描述)
            return new List<ApiResource>
            {
                new ApiResource("goods", "Goods Service")
                {
                    Scopes = { "goods"}
                },
                new ApiResource("order", "Order Service")
                {
                    Scopes = { "order", "goods" }
                }
            };
        }

        /// <summary>
        /// 4.0版本需要添加apiscope
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[] {
                 new ApiScope("goods"),
                 new ApiScope("order"),
            };
        }

        /// <summary>
        /// 客户端列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "credentials_client", //访问客户端Id,必须唯一
                    ClientName = "ClientCredentials Client",
                    //使用客户端授权模式，客户端只需要clientid和secrets就可以访问对应的api资源。
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "goods"
                    },
                },
                new Client
                {
                    ClientId = "password_client",
                    ClientName = "Password Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    //这里使用的是通过用户名密码和ClientCredentials来换取token的方式. ClientCredentials允许Client只使用ClientSecrets来获取token. 这比较适合那种没有用户参与的api动作
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "order","goods",
                    }
                },
                new Client
                {
                    ClientId = "main_client",
                    ClientName = "Implicit Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:5020/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5020/signout-callback-oidc" },
                    //是否显示授权提示界面
                    RequireConsent = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "order_client",
                    ClientName = "Order Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {
                        "order","goods",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RedirectUris = { "http://localhost:5021/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5021/signout-callback-oidc" },
                    //是否显示授权提示界面
                    RequireConsent = true,
                },
                new Client
                {
                    ClientId = "goods_client",
                    ClientName = "Goods Client",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:5022/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5022/signout-callback-oidc" },
                    //是否显示授权提示界面
                    RequireConsent = true,
                    AllowedScopes = {
                        "goods",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }

        /// <summary>
        /// 指定可以使用 Authorization Server 授权的 Users（用户）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "cba",
                    Password = "cba"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "chaney",
                    Password = "123"
                }
            };
        }
    }
}
