namespace EFCore.ReadMultiResultSetFromSP.DBLayer
{
    public interface IRepository
    {
        #region SP
        Task ExecuteSpAsync(string spName, Dictionary<string, object> spParams, CancellationToken token = default);

        Task<Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>>
            ExecuteMultiResultSetSpAsync<T1, T2, T3, T4, T5, T6>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new()
            where T6 : new();

        Task<Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>>> ExecuteMultiResultSetSpAsync<T1, T2, T3, T4, T5>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new();

        Task<Tuple<List<T1>, List<T2>, List<T3>, List<T4>>> ExecuteMultiResultSetSpAsync<T1, T2, T3, T4>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new();

        Task<Tuple<List<T1>, List<T2>, List<T3>>> ExecuteMultiResultSetSpAsync<T1, T2, T3>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new();

        Task<Tuple<List<T1>, List<T2>>> ExecuteMultiResultSetSpAsync<T1, T2>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new();

        Task<Tuple<List<T1>>> ExecuteSingleResultSetSpAsync<T1>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new();
        #endregion
    }
}