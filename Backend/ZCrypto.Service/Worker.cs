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
        private readonly List<IZCryptoTask> _tasksToRun = new List<IZCryptoTask>()
            {new SyncEnchangeForBuy(), new LowPriceCryptoTask()};

        public Worker()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var task in _tasksToRun)
                {
                    try
                    {
                        task.Run();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " " + ex.StackTrace);
                    }
                }

                await Task.Delay(500, stoppingToken);
            }
        }
    }
}