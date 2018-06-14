using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace OSLibrary.Utils
{
    public static class DbReaderModelBinder<TModel> where TModel : class, new()
    {
        public static TModel Bind(IDataRecord record)
        {

            var properties = typeof(TModel).GetProperties();
            var model = new TModel();

            for (var i = 0; i < record.FieldCount; i++)
            {
                var fieldName = record.GetName(i);
                var property = properties.FirstOrDefault(
                    p => p.Name == fieldName);

                if (property == null)
                    continue;

                if (!record.IsDBNull(i))
                    property.SetValue(model,
                        record.GetValue(i));
            }

            return model;
        }
        
    }
}
