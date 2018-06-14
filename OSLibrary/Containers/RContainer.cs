using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSLibrary.Containers
{
    public class RContainer
    {
        private Dictionary<Type, Type> _types;
        public RContainer()
        {
            _types = new Dictionary<Type, Type>();
        }

        public void Register<T1, T2>() where T2 : T1
        {
            if (!_types.ContainsKey(typeof(T1)))
            {
                _types[typeof(T1)] = typeof(T2);
                //typeof 回傳型別
            }
        }
        public T GetInstance<T>()
        {
            if (_types.ContainsKey(typeof(T)))
            {
                var type = _types[typeof(T)];
                return (T)Activator.CreateInstance(type);
            }
            else
            {
                return default(T);
                //實質型別會回傳0
                //參考型別會回傳null
            }
        }
    }
}
