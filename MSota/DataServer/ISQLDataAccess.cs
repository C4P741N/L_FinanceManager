using MSota.Models;

namespace MSota.DataServer
{
    public interface ISQLDataAccess
    {
        //string GetConnectionString();
        AccountLegerModel LoadData(string sql);
        List<T> LoadData<T>(string sql);
        void SaveData(string strvQuery);

        void SaveJsonDataOverStoredProcedure(JsonBodyModel props);
    }
}