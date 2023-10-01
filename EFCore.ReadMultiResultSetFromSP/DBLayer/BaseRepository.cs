using EFCore.ReadMultiResultSetFromSP.DbExtensions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace EFCore.ReadMultiResultSetFromSP.DBLayer
{
    public class BaseRepository : IRepository
    {
        protected readonly EfAppDbContext DbContext;

        public BaseRepository(EfAppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #region SP
        public async System.Threading.Tasks.Task ExecuteSpAsync(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
        {
            var i = 0;
            var sqlCommandSb = new StringBuilder($"EXEC {spName} ");
            var slParamValues = new object[spParams.Count];
            foreach (var spParam in spParams)
            {
                sqlCommandSb.Append(i < spParams.Count - 1 ? $"@{spParam.Key}={{{i}}}," : $"@{spParam.Key}={{{i}}}");
                slParamValues[i++] = spParam.Value;
            }

            var fString = FormattableStringFactory.Create(sqlCommandSb.ToString(), slParamValues);
            await DbContext.Database.ExecuteSqlInterpolatedAsync(fString, token);
        }

        public async Task<Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>> ExecuteMultiResultSetSpAsync<T1, T2, T3, T4, T5, T6>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new()
            where T6 : new()
        {
            List<T1> t1;
            List<T2> t2;
            List<T3> t3;
            List<T4> t4;
            List<T5> t5;
            List<T6> t6;

            await using (var cmd = DbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = spName;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (var spParam in spParams)
                {
                    if (spParam.Value != null)
                    {
                        var param = cmd.CreateParameter();

                        param.ParameterName = spParam.Key;
                        param.Value = spParam.Value;
                        cmd.Parameters.Add(param);
                    }
                }

                await DbContext.Database.OpenConnectionAsync(token);
                await using (var reader = await cmd.ExecuteReaderAsync(token))
                {
                    t1 = await reader.TranslateAsync<T1>(token);
                    t2 = await reader.TranslateAsync<T2>(token);
                    t3 = await reader.TranslateAsync<T3>(token);
                    t4 = await reader.TranslateAsync<T4>(token);
                    t5 = await reader.TranslateAsync<T5>(token);
                    t6 = await reader.TranslateAsync<T6>(token);
                }

                await DbContext.Database.CloseConnectionAsync();
            }

            return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>(t1, t2, t3, t4, t5, t6);
        }

        public async Task<Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>>> ExecuteMultiResultSetSpAsync<T1, T2, T3, T4, T5>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new()
        {
            var result = await ExecuteMultiResultSetSpAsync<T1, T2, T3, T4, T5, object>(spName, spParams, token);

            return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>>(result.Item1, result.Item2, result.Item3, result.Item4, result.Item5);
        }

        public async Task<Tuple<List<T1>, List<T2>, List<T3>, List<T4>>> ExecuteMultiResultSetSpAsync<T1, T2, T3, T4>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
        {
            var result = await ExecuteMultiResultSetSpAsync<T1, T2, T3, T4, object, object>(spName, spParams, token);

            return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(result.Item1, result.Item2, result.Item3, result.Item4);
        }

        public async Task<Tuple<List<T1>, List<T2>, List<T3>>> ExecuteMultiResultSetSpAsync<T1, T2, T3>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
            where T3 : new()
        {
            var result = await ExecuteMultiResultSetSpAsync<T1, T2, T3, object, object, object>(spName, spParams, token);

            return new Tuple<List<T1>, List<T2>, List<T3>>(result.Item1, result.Item2, result.Item3);
        }

        public async Task<Tuple<List<T1>, List<T2>>> ExecuteMultiResultSetSpAsync<T1, T2>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
            where T2 : new()
        {
            var result = await ExecuteMultiResultSetSpAsync<T1, T2, object, object, object, object>(spName, spParams, token);

            return new Tuple<List<T1>, List<T2>>(result.Item1, result.Item2);
        }

        public async Task<Tuple<List<T1>>> ExecuteSingleResultSetSpAsync<T1>(string spName, Dictionary<string, object> spParams, CancellationToken token = default)
            where T1 : new()
        {
            var result = await ExecuteMultiResultSetSpAsync<T1, object, object, object, object, object>(spName, spParams, token);

            return new Tuple<List<T1>>(result.Item1);
        }
        #endregion
    }
}