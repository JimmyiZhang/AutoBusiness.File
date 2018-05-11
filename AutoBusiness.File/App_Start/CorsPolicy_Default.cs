using AutoBusiness.Infrastructure.Configuration;
using AutoBusiness.Library;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace AutoBusiness.File
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private CorsPolicy policy;

        public CorsPolicyProvider()
        {
            // 创建跨域策略
            this.policy = new CorsPolicy();
            policy.Headers.Add("Content-Type, Content-Range, Content-Disposition, Content-Description");
            policy.Methods.Add("OPTIONS, HEAD, GET, POST, DELETE");

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


    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        ICorsPolicyProvider provider = new CorsPolicyProvider();
        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return provider;
        }
    }
}