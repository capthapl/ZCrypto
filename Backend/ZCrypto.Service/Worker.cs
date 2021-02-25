using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZCrypto.Service.Abstract;
using ZCrypto.Service.Tasks;

namespace ZCrypto.Service
{
    public class Worker : BackgroundService
    {
        private readonly List<IZCryptoTask> _tasksToRun = new List<IZCryptoTask>() {new SyncEnchangeForBuy()};
        
        public Worker()
        {
          
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var task in _tasksToRun)
                {
                    task.Run();
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}