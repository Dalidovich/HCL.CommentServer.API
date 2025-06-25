using HCL.CommentServer.API.DAL;

namespace HCL.CommentServer.API.BackgroundHostedService
{
    public class CheckDBHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private CommentAppDBContext _appDBContext;

        public CheckDBHostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            _appDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();

            if (await _appDBContext.Database.EnsureCreatedAsync())
            {
                _appDBContext.UpdateDatabase();
            }

            return;
        }
    }
}