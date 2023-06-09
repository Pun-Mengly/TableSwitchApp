using Serilog;
using System.Data;
using System.Data.SqlClient;
using TableSwitchWebApplication.Helper;

namespace TableSwitchWebApplication.Cores
{
    public class AdoOperation : IAdoOperation
    {
        public async Task<DataTable> ExecuteStoreAsync(string name, string parameterKeys, string parameterValues) //Role Key and Value must have the same index
        {
            Log.Information($"EXECUTE Store Procedure Name: '{name}'"); //New Log
            SqlConnection con = new();
            SqlDataAdapter da = new();
            DataTable dt = new();
            SqlCommand cmd = new();
            try
            {
                dt = new DataTable();
                con = new SqlConnection(AppCon.ConStr);
                await con.OpenAsync();

                cmd = new SqlCommand(name, con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameterValues != null || parameterKeys !=null)
                {
                    string[] parameterKey = parameterKeys!.Split('~');
                    string[] parameterValue = parameterValues!.Split('~');
                    for (int i = 0; i < parameterKey.Count(); i++)
                    {
                        cmd.Parameters.AddWithValue(parameterKey[i], parameterValue[i]);
                    }
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //AddLog3("0", sql + " | " + ex.Message.ToString(), "ReturnDT1");
                Log.Error($"{ex.Message}"); //New Log
                throw new Exception(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
                await cmd.DisposeAsync();
                da.Dispose();
            }
            return dt;

        }

        public async Task<DataTable> ReturnDT1(string sql) //copy from old "ReturnDT1"
        {
            Log.Information($"SQL Script: '{sql}'"); //New Log
            SqlConnection con = new();
            SqlDataAdapter da = new();
            DataTable dt = new();
            SqlCommand cmd = new();
            try
            {
                dt = new DataTable();
                con = new SqlConnection(AppCon.ConStr);
                await con.OpenAsync();

                cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //AddLog3("0", sql + " | " + ex.Message.ToString(), "ReturnDT1");
                Log.Error($"{ex.Message}"); //New Log
                throw new Exception(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
                await cmd.DisposeAsync();
                da.Dispose();
            }
            return dt;

        }
        public async Task<DataSet> ReturnDT2_DATASET(string sql)
        {
            Log.Information($"SQL Script: '{sql}'"); //New Log
            SqlConnection con = new();
            SqlDataAdapter da = new();
            DataSet dts = new();
            SqlCommand cmd = new();
            try
            {
                dts = new DataSet();
                con = new SqlConnection(AppCon.ConStr);
                await con.OpenAsync();

                cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dts);
            }
            catch (Exception ex)
            {
                //AddLog3("0", sql + " | " + ex.Message.ToString(), "ReturnDT2_DATASET");
                Log.Error($"{ex.Message}"); //New Log
                throw new Exception(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
                await cmd.DisposeAsync();
                da.Dispose();
            }
            return dts;
        }


        // Usage: Obj was nullable 
        public static async Task<DataTable> ReturnDT1NullableObj(string sql) //copy from old "ReturnDT1"
        {
            Log.Information($"SQL Script: '{sql}'"); //New Log
            SqlConnection con = new();
            SqlDataAdapter da = new();
            DataTable dt = new();
            SqlCommand cmd = new();
            try
            {
                dt = new DataTable();
                con = new SqlConnection(AppCon.ConStr);
                await con.OpenAsync();

                cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //AddLog3("0", sql + " | " + ex.Message.ToString(), "ReturnDT1");
                Log.Error($"{ex.Message}"); //New Log
                throw new Exception(ex.ToString());
            }
            finally
            {
                await con.CloseAsync();
                await con.DisposeAsync();
                await cmd.DisposeAsync();
                da.Dispose();
            }
            return dt;

        }
    }
}
