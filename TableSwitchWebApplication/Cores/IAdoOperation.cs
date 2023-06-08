using System.Data;

namespace TableSwitchWebApplication.Cores
{
    public interface IAdoOperation
    {
        public Task<DataTable> ReturnDT1(string sql); // One query
        public Task<DataSet> ReturnDT2_DATASET(string sql); // Multiple Query
        public Task<DataTable> ExecuteStoreAsync(string name, string parameterKey,string parameterValue); // Execute Store Pass By Name

    }
}
