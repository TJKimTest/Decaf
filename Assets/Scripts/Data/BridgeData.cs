using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.Data
{
    using System.Linq;

    public class BridgeData : IEqualityComparer<BridgeData>
    {
        public class Data : IEqualityComparer<Data>
        {
            public string Name { get; private set; } = "";
            public object Source { get; private set; }
            public Type DataType { get; private set; }

            public Data(string name, Type dataType, object source)
            {
                Name = name;
                DataType = dataType;
                Source = source;
            }
            public T GetConvertData<T>()
            {
                T result = default;
                if (typeof(T) == DataType)
                    result = (T)Source;

                else
                    Debug.Log("Not Equal Data Type ..");

                return result;
            }

            public bool Equals(Data x, Data y)
                => x.DataType == y.DataType &&
                   x.Name == y.Name &&
                   x.Source == y.Source;

            public int GetHashCode(Data obj)
                => obj.GetHashCode();

            public static implicit operator bool(Data d) => d != null;
        }

        private const string Header = " [ BridgeData ] ";

        public string From { get; private set; } = "";
        public string To { get; private set; } = "";

        public Data Source { get; private set; }

        public BridgeData(string fromPageKey, string toPageKey, Data data)
        {
            From = fromPageKey;
            To = toPageKey;
            Source = data;
        }

        public static T Search<T>(string dataName, params BridgeData[] sources)
        {
            T result = default;
            foreach (var source in sources)
            {
                if (source.Source.Name == dataName)
                {
                    result = source.Source.GetConvertData<T>();
                    break;
                }
            }

            return result;
        }

        public static bool TryExists(string dataName, out BridgeData output, params BridgeData[] sources)
        {
            output = sources.FirstOrDefault(o => o.Source.Name == dataName);
            return output != null;
        }
        public static implicit operator bool(BridgeData b) => b != null;
        public override string ToString()
            => $" [ {From} ] - Contect Key : {To} / DataType : {Source.DataType}";

        public bool Equals(BridgeData x, BridgeData y)
            => x.From == y.From &&
               x.Equals(x, y) &&
               x.To == y.To;

        public int GetHashCode(BridgeData obj)
            => obj.GetHashCode();
    }

}

