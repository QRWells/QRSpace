using System;
using System.Collections.Concurrent;
using StackExchange.Redis;

namespace QRSpace.Server.Services
{
    public class RedisHelper:IDisposable
    {
        private readonly string _connectionString;
        
        private readonly string _instanceName;
        
        private readonly int _defaultDb; 
        
        private readonly ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        
        public RedisHelper(string connectionString, string instanceName, int defaultDb = 0)
        {
            _connectionString = connectionString;
            _instanceName = instanceName;
            _defaultDb = defaultDb;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }
        
        private ConnectionMultiplexer GetConnect()
        {
            return _connections.GetOrAdd(_instanceName, p => ConnectionMultiplexer.Connect(_connectionString));
        }

        public IDatabase GetDatabase() => GetDatabase(_defaultDb);

        public IDatabase GetUserStateDb() => GetDatabase(2);

        private IDatabase GetDatabase(int db) => GetConnect().GetDatabase(db);

        public IServer GetServer(string configName = null, int endPointsIndex = 0)
        {
            var configOption = ConfigurationOptions.Parse(_connectionString);
            return GetConnect().GetServer(configOption.EndPoints[endPointsIndex]);
        }

        public ISubscriber GetSubscriber(string configName = null)
        {
            return GetConnect().GetSubscriber();
        }
        
        public void Dispose()
        {
            if (_connections == null || _connections.Count <= 0) return;
            foreach (var item in _connections.Values)
            {
                item.Close();
            }
        }
    }
}