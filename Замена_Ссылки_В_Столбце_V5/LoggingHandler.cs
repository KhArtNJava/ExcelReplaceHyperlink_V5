using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Замена_Ссылки_В_Столбце_V5
{
    public class LoggingHandler : HttpClientHandler
    {
        NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //public LoggingHandler(HttpMessageHandler innerHandler)
        //    : base(innerHandler)
        //{
        //}

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.Info("Request:");
            logger.Info(request.ToString());
            if (request.Content != null)
            {
                logger.Info(await request.Content.ReadAsStringAsync());
            }
            logger.Info("");

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            logger.Info("Response:");
            logger.Info(response.ToString());
            if (response.Content != null)
            {
                logger.Info(await response.Content.ReadAsStringAsync());
            }
            logger.Info("");

            return response;
        }
    }
}
