namespace TableSwitchWebApplication.Cores
{
    public interface IAdoOperation
    {
        public Task ReturnObjs(string query);
    }
}
