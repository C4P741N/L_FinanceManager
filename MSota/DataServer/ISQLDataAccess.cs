﻿using MSota.JavaScriptObjectNotation;

namespace MSota.DataServer
{
    public interface ISQLDataAccess
    {
        //string GetConnectionString();
        List<T> LoadData<T>(string sql);
        void SaveData(string strvQuery);

        void SaveJsonDataOverStoredProcedure(JsonBodyProps props);
    }
}