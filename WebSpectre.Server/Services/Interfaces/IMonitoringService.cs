namespace WebSpectre.Server.Services.Interfaces
{
    public interface IMonitoringService
    {
        /// <summary>
        /// Чтение указанного потока Redis.
        /// </summary>
        /// <returns></returns>
        public Task SendNetworkFromRequiredAgentAsync(string agent, int? count = null);

        /// <summary>
        /// Чтение потоков Redis.
        /// </summary>
        /// <returns></returns>
        public Task SendNetworkFromAllAgentsAsync(int? count = null);

        /// <summary>
        /// Остановить чтение потока указанного агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public Task StopRequiredAsync(string agent);

        /// <summary>
        /// Остановить чтение всех потоков.
        /// </summary>
        /// <returns></returns>
        public Task StopAllAsync();

        /// <summary>
        /// Получить агентов.
        /// </summary>
        /// <returns></returns>
        public Task SendAgentsAsync();
    }
}
