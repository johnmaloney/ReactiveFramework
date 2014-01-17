using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Aetos.Messaging.Common.Logging;
using Aetos.Messaging.Interfaces;

namespace Aetos.Messaging.Domain.Clients
{
    public abstract class ClientBase<TClient>
        where TClient : class, IClient
    {
        #region Fields

        private readonly static List<Type> _clientTypes = new List<Type>();
        protected readonly List<TClient> _clients = new List<TClient> { null, null };
        private readonly List<bool> _ensuring = new List<bool> { false, false };
        private bool _heartbeatRunning = false;
        private Timer _heartbeat;
        protected Action<Message> _onMessageReceived;
        
        #endregion

        #region Properties
        
        public TClient Primary 
        { 
            get
            {
                if (_clients[0] == null)
                    EnsureClient(0);
                return _clients.First(); 
            } 
        }

        public TClient Secondary 
        { 
            get
            {
                if (_clients[1] == null)
                    EnsureClient(1);
                return _clients.Last();  
            } 
        }
        
        #endregion

        #region Methods

        protected abstract void LoadClientTypes(List<Type> clientTypes);
        
        protected abstract TClient CreateInstance(Type type);

        public ClientBase()
        {
            LoadClientTypes(_clientTypes);
            _heartbeat = new Timer(100000);
            _heartbeat.Elapsed += OnHeartbeatElapsed;
        }

        public bool HasMessages()
        {
            return ExecutePrimary(x => x.HasMessages()) 
                || ExecuteSecondary(y => y.HasMessages());
        }

        public bool IsListening()
        {
            return ExecutePrimary(x => x.IsListening())
                || ExecuteSecondary(y => y.IsListening());
        }

        protected virtual void OnHeartbeatElapsed(object sender, ElapsedEventArgs e)
        {
            if (_onMessageReceived != null && !_heartbeatRunning)
            {
                _heartbeatRunning = true;
                // heartbeat fires in receiver mode once subscribers have started //
                // if we have lost access to the queue, try tp Subscribe //
                if (!ExecutePrimary(x => x.IsListening()))
                    StartPrimary();

                if (!ExecuteSecondary(x => x.IsListening()))
                    StartSecondary();

                // peek the Queue to confirm connectiavity, unsubscribe if lost: //
                ExecutePrimary(x=> x.HasMessages(), x=> x.Unsubscribe());

                ExecuteSecondary(x => x.HasMessages(), x => x.Unsubscribe());

                _heartbeatRunning = false;
            }
        }

        private void EnsureClient(int index)
        {
            if (_clients[index] == null)
            {
                if (!_ensuring[index])
                {
                    try
                    {
                        _ensuring[index] = true;
                        Log.Debug("** ClientBase.Ensure, creating as index: {0}", index);
                        _clients[index] = CreateInstance(_clientTypes[index]);

                    }
                    finally
                    {
                        _ensuring[index] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Starts all Client instances and begin to listen for new
        /// Queue items to be entered, will remove an item from the 
        /// Queue when it is receieved in this Client.
        /// </summary>
        protected void Subscribe()
        {
            if(_onMessageReceived != null)
            {
                StartSubscriber(0);
                StartSubscriber(1);
                _heartbeat.Start();
            }
        }

        /// <summary>
        /// This is used as a listener for the Queue, it will not
        /// attempt to process the Queue item only listens for new entries.
        /// </summary>
        protected void Listen()
        {
            if (_onMessageReceived != null)
            {
                StartSubscriber(0, false);
                StartSubscriber(1, false);
            }
        }

        public void Unsubscribe()
        {
            foreach(var client in _clients)
            {
                if (client != null)
                {
                    try
                    {
                        client.Unsubscribe();
                    }
                    catch { }
                }
            }
        }

        protected void StartPrimary()
        {
            StartSubscriber(0);
        }

        protected void StartSecondary()
        {
            StartSubscriber(1);
        }
        
        private void StartSubscriber(int index, bool shouldProcessMessages = true)
        {
            EnsureClient(index);
            
            if (_clients[index] != null)
            {
                if (shouldProcessMessages) 
                    // Meaning should remove the message from the Queue Permanently
                    Execute(x => x.Subscribe(_onMessageReceived), index);
                else 
                    // Will leave the messages on the Queue for some other QueueClient
                    Execute(x => x.Listen(_onMessageReceived), index);
            }

            Log.Debug("** ClientBase.StartSubscriber, subscribed at index:{0}", index);
        }

        protected void ExecutePrimary(Action<TClient> action, Action<TClient> onException = null)
        {
            Execute(action, 0, onException);
        }

        protected void ExecuteSecondary(Action<TClient> action, Action<TClient> onException = null)
        {
            Execute(action, 1, onException);
        }

        private void Execute(Action<TClient> action, int index, Action<TClient> onException = null)
        {
            Log.Debug("** ClientBase.Execute, running: {0}, on index: {1}", action, index);

            try
            {
                EnsureClient(index);
                action(_clients[index]);
            }
            catch (Exception ex)
            {
                Log.Error("** ClientBase.Execute, caught exception: {0}", ex.Message);
                if (onException != null)
                {
                    onException(_clients[index]);
                }
            }
        }

        protected T ExecutePrimary<T>(Func<TClient, T> action, Action<TClient> onException = null)
        {
            return Execute<T>(action, 0, onException);
        }

        protected T ExecuteSecondary<T>(Func<TClient, T> action, Action<TClient> onException = null)
        {
            return Execute<T>(action, 1, onException);
        }

        private T Execute<T>(Func<TClient, T> action, int index, Action<TClient> onException)
        {
            Log.Debug("** ClientBase.Execute, running: {0}, on index: {1}", action, index);
            var result = default(T);
            try
            {
                EnsureClient(index);
                result = action(_clients[index]);
            }
            catch (Exception ex)
            {
                Log.Error("** ClientBase.Execute, caught exception: {0}", ex.Message);
                
                if (onException != null)
                {
                    onException(_clients[index]);
                }
            }
            return result;
        }

        #endregion
    }
}
