using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Helpers;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;
using SmugglerCoin.UpbitModels;
using SmugglerCoin.UpbitModels.Jobs;
using SmugglerCoin.UpbitModels.Models;
using static Quartz.Logging.OperationName;

public class Program
{
    public static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("Program");
        logger.LogInformation("Hello World! Logging is {Description}.", "fun");

        IHostBuilder rtnValue = Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();

                IHostEnvironment env = hostingContext.HostingEnvironment;

                /// 여기에 quartz 스케쥴러를 추가한다.
                /// 등록되는 스케줄러들은 주기적으로 코인들의 캔들을 조회하는 방식으로 진행
                configuration.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);

                string appsettingname = "appsettings.develop.json";
#if DEBUG
                appsettingname = "appsettings.json";
#endif
                //#if DEBUG
                //                configuration.AddJsonFile("appsettings.develop.json", optional: true, reloadOnChange: true);
                //#elif !DEBUG
                //                    configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //#endif
                configuration.AddJsonFile(appsettingname, optional: true, reloadOnChange: true);

                //IConfigurationRoot configurationRoot = configuration.Build();
                //MySettings setting = new();
                //configurationRoot.GetSection(nameof(MySettings)).Bind(setting);
            })
            .ConfigureServices((context, services) =>
            {
                // configs
                services.AddSingleton<ApiKeyReader>();
                services.AddSingleton<UpbitAPICaller>();

                //#endif
                services.AddSingleton<IAPICall>(provider =>
                {
                    // secretkey에 유효한 key가 있는 기준으로 upbit/bithumb/... 등을 구분하여 설정
                    var apiCaller = provider.GetRequiredService<UpbitAPICaller>();
                    return apiCaller;
                });

                // Model
                services.AddSingleton<Smuggler>();
                services.AddSingleton<UpbitManager>();

                // 잡이 complete 될때가지 대기할꺼야?
                bool isWatingForComplete = false;   // no

                Action<IServiceCollectionQuartzConfigurator> CreateScheduler<T>(string cron) where T : BaseJob
                {
                    return q =>
                    {
                        var jobName = typeof(T).Name;
                        JobKey jobKey = new(jobName);
                        q.AddJob<T>(j => j.WithIdentity(jobKey));
                        q.AddTrigger(t => t
                            .ForJob(jobKey)
                            .WithIdentity($"{jobName}_1")
                            .WithCronSchedule(cron));
                    };
                }

#if DEBUG
                ////////////// Debug Mode

                services.AddTransient<TestJob>();

                //services.AddTransient<Job_Init>();
                services.AddSingleton<Job_Init>();

                services.AddQuartz(q =>
                {
                    var scheduleAction = CreateScheduler<Job_StatusWallet>("5 * * * * ?");
                    scheduleAction(q);

                    //                    JobKey jobKey = new(nameof(TestJob));
                    // var jobname = nameof(Job_Init);
                    //var jobname = nameof(Job_StatusWallet);
                    ////JobKey jobKey = new(nameof(Job_Init));
                    //JobKey jobKey = new(jobname);
                    ////q.AddJob<TestJob>(j => j.WithIdentity(jobKey));
                    ////q.AddJob<Job_Init>(j => j.WithIdentity(jobKey));
                    //q.AddJob<Job_StatusWallet>(j => j.WithIdentity(jobKey));
                    //q.AddTrigger(t => t
                    //    .ForJob(jobKey)
                    //    .WithIdentity($"{jobname}_1")
                    //    .WithCronSchedule("5 * * * * ?"));        // 매 10초마다 동작
                    //.WithCronSchedule("0/5 * * * * ?"));        // 매 10초마다 동작
                    //q.AddTrigger(t => t
                    //    .ForJob(jobKey)
                    //    .WithIdentity($"{jobname}_2")
                    //    .WithCronSchedule("0/13 * * * * ?"));       // 매 분 마다 동작
                    //q.AddTrigger(t => t
                    //    .ForJob(jobKey)
                    //    .WithIdentity($"{jobname}_3")
                    //    .WithCronSchedule("0/35 * * * * ?"));       // 매 35초
                    //q.AddTrigger(t => t
                    //    .ForJob(jobKey)
                    //    .WithIdentity($"{jobname}_4")
                    //    .WithCronSchedule("0/3 * 19-20 * * ?")); // 19시 ~ 20시 사이에 2초마다 동작
                });
#elif !DEBUG
                ////////////// Release Mode

                services.AddTransient<DefaultJob>();
                services.AddQuartz(q =>
                {
                    q.UseMicrosoftDependencyInjectionJobFactory();
                    JobKey jobKey = new(nameof(DefaultJob));
                    q.AddJob<DefaultJob>(j => j.WithIdentity(jobKey));
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(DefaultJob)}_1")
                        .WithCronSchedule("0/3 * 0-9 * * ?")); // 0시 ~ 9시 사이에 2초마다 동작
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(DefaultJob)}_2")
                        .WithCronSchedule("0/3 * 12-13 * * ?")); // 12시 ~ 13시 사이에 2초마다 동작
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(DefaultJob)}_3")
                        .WithCronSchedule("0/3 * 19-20 * * ?")); // 19시 ~ 20시 사이에 2초마다 동작
                });
#endif
                // 서비스에 등록된 잡들의 스케줄러 시작
                //
                services.AddQuartzHostedService(q => q.WaitForJobsToComplete = isWatingForComplete);

                // Add My EF Core DbContext
                //services.AddDbContext<MyContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("MyDataBase")));

                // Add My Class
                //CookieContainer cookieContainer = new();
                //services.AddSingleton(_ => cookieContainer);
                //services.AddScoped<MyService>();

                // Add My HttpClientFactory
                //services.AddHttpClient();
                //services.AddHttpClient("CookieContainerHttpClient").ConfigurePrimaryHttpMessageHandler(() =>
                //{
                //    HttpClientHandler handler = new()
                //    {
                //        UseCookies = true,
                //        CookieContainer = cookieContainer,
                //    };

                //    return handler;
                //});
            });

        /// 그리고 1분당 시세를 보고 ai가 판단하여 거래를 진행하게 한다
        /// 지갑의 재무상태를 보고 장기플랜을 계획한다.

        return rtnValue;
    }
}