using System;
using System.Collections.Generic;
using System.Text;
using epicture.Data;
using epicture.Network;

namespace epicture
{
    public class Engine
    {

        #region "Variables"

        public ClientData Data { get; set; }

        public ClientNetwork Network { get; set; }

        private EngineState _state;

        public EngineState State
        {
            get
            {
                return (_state);
            }

            set
            {
                _state = value;
                if (OnStateChanged != null)
                    OnStateChanged(this, new StateChangedEventArgs(_state));
            }
        }

        public delegate void MessageEventHandler(object sender, StateChangedEventArgs args);

        public event MessageEventHandler OnStateChanged;

        #endregion

        #region "Builder"

        public Engine()
        {
            Data = new ClientData();
            Network = new ClientNetwork(this);
            State = EngineState.Disconnected;
        }

        #endregion

        #region "Methods"

        public void StartEngine()
        {

        }

        public void StopEngine()
        {

        }

        public void Dispose()
        {
            if (Data != null)
            {
                Data.Dispose();
                Data = null;
            }
            if (Network != null)
            {
                Network.Dispose();
                Network = null;
            }
            GC.Collect();
        }

        #endregion

        #region "Events"

        public class StateChangedEventArgs : EventArgs
        {
            EngineState State;

            public StateChangedEventArgs(EngineState _state)
            { State = _state; }
        };
        public void StateChanged(Object obj, StateChangedEventArgs eventArgs)
        {
            if (OnStateChanged != null)
            {
                OnStateChanged(obj, eventArgs);
            }
        }

        #endregion
    }
}
