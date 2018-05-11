using AutoBusiness.Infrastructure.Configuration;
using AutoBusiness.Library;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace AutoBusiness.File
{
    public class WildcardCorsPolicy : ICorsPolicyProvider
    {
        private CorsPolicy policy;
        public WildcardCorsPolicy()
        {
            // 创建跨域策略
            this.policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                PreflightMaxAge = 60 * 60 * 24
            };

            // 增加允许的源
            var origin = ConfigurationHelper.GetAppSettings("Origins");
            if (origin.IsNotEmpty())
            {
                var origins = origin.Split(',');
                foreach (var ori in origins)
                {
                    policy.Origins.Add(ori);
                }
            }
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.policy);
        }
    }

    public class WildcardCorsEngine : ICorsEngine
    {
        public CorsResult EvaluatePolicy(CorsRequestContext context, CorsPolicy policy)
        {
            var origin = context.Origin;

            var domain = string.Empty;
            // 匹配域名
            foreach (var ori in policy.Origins)
            {
                if (RegexHelper.Match(origin, ori))
                {
                    domain = origin;
                    break;
                }
            }

            WildcardCorsResult result = new WildcardCorsResult(policy, context);
            result.AllowedOrigin = domain;
            return result;
        }
    }

    public class WildcardCorsResult : CorsResult
    {
        private CorsPolicy policy;
        private CorsRequestContext context;
        public WildcardCorsResult(CorsPolicy policy, CorsRequestContext context)
        {
            this.policy = policy;
            this.context = context;
        }

        public override IDictionary<string, string> ToResponseHeaders()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Access-Control-Allow-Origin", this.AllowedOrigin);

            if (context.IsPreflight)
            {
                dic.Add("Access-Control-Allow-Methods", context.AccessControlRequestMethod);
                dic.Add("Access-Control-Max-Age", policy.PreflightMaxAge.ToString());
            }

            return dic;
        }
    }
}