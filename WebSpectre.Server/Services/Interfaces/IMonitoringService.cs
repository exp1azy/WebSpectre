namespace WebSpectre.Server.Services.Interfaces
{
    public interface IMonitoringService
    {
        public Task SendAgentsAsync();

        /// <summary>
        /// Чтение указанного потока Redis.
        /// </summary>
        /// <returns></returns>
        public Task SendNetworkFromRequiredAgentAsync(string agent, int? count = null);

        /// <summary>
        /// Остановить чтение потока указанного агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public Task StopRequiredAsync(string agent);

        /// <summary>
        /// Получить агентов.
        /// </summary>
        /// <returns></returns>
        public Task SendHostnamesAsync();
    }
}
