namespace ZepterTest.DataWriter.Interfaces
{
    /// <summary>
    /// Interface for orchestrating multiple data generators
    /// </summary>
    public interface IDataGeneratorOrchestrator
    {
        /// <summary>
        /// Executes all data generators in the correct order
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task ExecuteAllGeneratorsAsync();
    }
}
