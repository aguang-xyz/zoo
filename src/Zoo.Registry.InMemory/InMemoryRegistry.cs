using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.Rpc.Abstractions.Extensions;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Registry.InMemory
{
    public class InMemoryRegistry : IRpcRegistry
    {
        private readonly Dictionary<string, ISet<Uri>> _uris = new();
        
        private readonly Dictionary<string, IList<Action<Uri[]>>> _listeners = new();
        
        public InMemoryRegistry(Uri uri)
        {
            Uri = uri;
        }
        
        public Uri Uri { get; }

        private static string GetKey(Uri uri)
        {
            return $"{uri.GetNodeType()}:{uri.GetServiceName()}";
        }

        private void Notify(string key)
        {
            lock (this)
            {
                if (_listeners.ContainsKey(key))
                {
                    var updatedUris = _uris[key].ToArray();
                    
                    foreach (var notify in _listeners[key])
                    {
                        notify(updatedUris);
                    }
                }
            }
        }
        
        public void Register(Uri uri)
        {
            lock (this)
            {
                var key = GetKey(uri);

                if (!_uris.ContainsKey(key))
                {
                    _uris[key] = new HashSet<Uri>();
                }

                _uris[key].Add(uri);
                
                Notify(key);
            }
        }

        public void Unregister(Uri uri)
        {
            lock (this)
            {
                var key = GetKey(uri);

                if (_uris.ContainsKey(key) && _uris[key].Remove(uri))
                {
                    Notify(key);
                }
            }
        }

        public void Subscribe(Uri uri, Action<Uri[]> notify)
        {
            lock (this)
            {
                var key = GetKey(uri);

                if (!_listeners.ContainsKey(key))
                {
                    _listeners[key] = new List<Action<Uri[]>>();
                }
                
                _listeners[key].Add(notify);
            }
        }

        public void Unsubscribe(Uri uri, Action<Uri[]> notify)
        {
            lock (this)
            {
                var key = GetKey(uri);

                if (_listeners.ContainsKey(key))
                {
                    _listeners[key].Remove(notify);
                }
            }
        }

        public Uri[] Lookup(Uri uri)
        {
            lock (this)
            {
                var key = GetKey(uri);
                
                if (_uris.ContainsKey(key))
                {
                    return _uris[key].ToArray();
                }

                return Array.Empty<Uri>();
            }
        }
        
        public void Dispose()
        {
        }
    }
}