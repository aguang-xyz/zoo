using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using Zoo.Rpc.Abstractions.Constants;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Extensions;

namespace Zoo.Registry.Redis
{
    /// <summary>
    /// Redis registry.
    /// </summary>
    internal sealed class Registry : IRpcRegistry
    {
        private const int DefaultInterval = 10;

        private static class RegistryParameterNames
        {
            public const string Interval = nameof(Interval);
        }

        private readonly int _interval;
        
        private readonly ConnectionMultiplexer _redis;

        private readonly Dictionary<Action<Uri[]>, (Task, CancellationTokenSource)> _syncTasks = new();

        private readonly Dictionary<Uri, Uri[]> _cachedUris = new();

        public Registry(Uri uri)
        {
            Uri = uri;
            
            _redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints =
                {
                    $"{uri.Host}:{uri.Port}"
                }
            });

            if (!int.TryParse(uri.GetQueryParameter(RegistryParameterNames.Interval), out _interval))
            {
                _interval = DefaultInterval;
            }
        }

        public Uri Uri { get; }

        // TODO We need to support database id here.
        private IDatabase Database => _redis.GetDatabase();

        private static bool ShouldNotify(IReadOnlyList<Uri> cachedUris, IReadOnlyList<Uri> uris)
        {
            if (cachedUris.Count != uris.Count)
            {
                return true;
            }

            for (var i = 0; i < cachedUris.Count; i++)
            {
                if (cachedUris[i] != uris[i])
                {
                    return true;
                }
            }

            return false;
        }
        
        private Task SyncAsync(Uri uri, Action<Uri[]> notify, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var uris = Lookup(uri);

                        lock (_cachedUris)
                        {
                            if (!_cachedUris.ContainsKey(uri))
                            {
                                notify(_cachedUris[uri] = uris);
                            }
                            else if (ShouldNotify(_cachedUris[uri], uris))
                            {
                                notify(_cachedUris[uri] = uris);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // TODO Handle this internal exception.
                    }
                    finally
                    {
                        Thread.Sleep(_interval);
                    }
                }
            }, cancellationToken);
        }

        private static RedisKey GetRedisKey(Uri uri)
        {
            var interfaceName = uri.GetServiceName();
            var nodeType = uri.GetQueryParameter(ParameterNames.NodeType);

            return new RedisKey($"{nodeType}:{interfaceName}");
        }

        public void Register(Uri uri)
        {
            Database.SetAdd(GetRedisKey(uri), new RedisValue(uri.ToString()));
        }

        public void Unregister(Uri uri)
        {
            Database.SetRemove(GetRedisKey(uri), new RedisValue(uri.ToString()));
        }

        public void Subscribe(Uri uri, Action<Uri[]> notify)
        {
            lock (_syncTasks)
            {
                if (_syncTasks.ContainsKey(notify))
                {
                    _syncTasks[notify].Item2.Cancel();
                    _syncTasks.Remove(notify);
                }

                var tokenSource = new CancellationTokenSource();
                var cancellationToken = tokenSource.Token;

                _syncTasks[notify] = (SyncAsync(uri, notify, cancellationToken), tokenSource);
            }
        }

        public void Unsubscribe(Uri uri, Action<Uri[]> notify)
        {
            lock (_syncTasks)
            {
                if (_syncTasks.ContainsKey(notify))
                {
                    _syncTasks[notify].Item2.Cancel();
                    _syncTasks.Remove(notify);
                }
            }
        }

        public Uri[] Lookup(Uri uri)
        {
            return Database
                .SetScan(GetRedisKey(uri))
                .Select(x => new Uri(x.ToString()))
                .OrderBy(x => x)
                .ToArray();
        }
        
        public void Dispose()
        {
            lock (_syncTasks)
            {
                foreach (var pair in _syncTasks)
                {
                    pair.Value.Item2.Cancel();
                }
            }
        }

    }
}