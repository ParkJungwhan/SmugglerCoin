using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SmugglerCoin.Helpers;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;
using SmugglerCoin.UpbitModels;

public class Program
{
    public static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
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
                //var settingsss = context.Configuration.GetSection("AppSettings");
                //var appname = settingsss["AppName"];

                // key 파일확인
                var reader = new ApiKeyReader("SecretConfig.json");

                services.Configure<ApiKeyOptions>("Upbit", options =>
                {
                    var upbitKeys = reader.GetKeys("upbit");
                    options.AccessKey = upbitKeys.accessKey;
                    options.SecretKey = upbitKeys.secretKey;
                });

                services.AddSingleton<IAPICall>(provider =>
                {
                    var apiCaller = new UpbitAPICaller(reader);
                    apiCaller.SetInitAPI();

                    return apiCaller;
                });

                // appsettings.json Setting
                //   services.Configure<MySettings>(context.Configuration.GetSection(nameof(MySettings)));

                // complete 될때가지 대기할꺼야?
                bool isWatingForComplete = false;   // no
#if DEBUG

                //services.AddTransient<DefaultJob>();
                services.AddTransient<TestJob>();

                services.AddQuartz(q =>
                {
                    JobKey jobKey = new(nameof(TestJob));
                    q.AddJob<TestJob>(j => j.WithIdentity(jobKey));
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(TestJob)}_1")
                        .WithCronSchedule("0/10 * * * * ?")); // 10초마다 동작
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(TestJob)}_1")
                        .WithCronSchedule("0 * * * * ?")); // 매분 마다 동작
                                                           //.WithCronSchedule("0/3 * 0-9 * * ?")); // 0시 ~ 9시 사이에 2초마다 동작
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(TestJob)}_2")
                        .WithCronSchedule("0/3 * 12-13 * * ?")); // 12시 ~ 13시 사이에 2초마다 동작
                    q.AddTrigger(t => t
                        .ForJob(jobKey)
                        .WithIdentity($"{nameof(TestJob)}_3")
                        .WithCronSchedule("0/3 * 19-20 * * ?")); // 19시 ~ 20시 사이에 2초마다 동작
                });
#elif !DEBUG

                //services.AddQuartz(q =>
                //{
                //    q.UseMicrosoftDependencyInjectionJobFactory();
                //    JobKey jobKey = new(nameof(DefaultJob));
                //    q.AddJob<DefaultJob>(j => j.WithIdentity(jobKey));
                //    q.AddTrigger(t => t
                //        .ForJob(jobKey)
                //        .WithIdentity($"{nameof(DefaultJob)}_1")
                //        .WithCronSchedule("0/3 * 0-9 * * ?")); // 0시 ~ 9시 사이에 2초마다 동작
                //    q.AddTrigger(t => t
                //        .ForJob(jobKey)
                //        .WithIdentity($"{nameof(DefaultJob)}_2")
                //        .WithCronSchedule("0/3 * 12-13 * * ?")); // 12시 ~ 13시 사이에 2초마다 동작
                //    q.AddTrigger(t => t
                //        .ForJob(jobKey)
                //        .WithIdentity($"{nameof(DefaultJob)}_3")
                //        .WithCronSchedule("0/3 * 19-20 * * ?")); // 19시 ~ 20시 사이에 2초마다 동작
                //});
#endif
                // 시작
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