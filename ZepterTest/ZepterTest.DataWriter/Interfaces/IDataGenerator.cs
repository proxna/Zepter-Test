namespace ZepterTest.DataWriter.Interfaces
{
    /// <summary>
    /// Interface for data generator components
    /// </summary>
    public interface IDataGenerator
    {
        /// <summary>
        /// Generates and saves mock data to the database
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        Task GenerateDataAsync();
    }
}
