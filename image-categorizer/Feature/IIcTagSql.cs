using image_categorizer.MVVM.Model;
using System.Collections.Generic;

namespace image_categorizer
{
    public interface IIcTagSql
    {
        Logger SqlLogger { get; set; }

        void DeleteQuary(string attribute, List<string> keys);
        void InsertQuery(InsertQueryModel queryModel);
        void InsertQuery(List<InsertQueryModel> queryModels);
        Dictionary<string, List<string?>>? SelectQuery(string[] select);
        void SQLiteinit();
    }
}