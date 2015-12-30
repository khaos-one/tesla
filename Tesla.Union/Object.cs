namespace Tesla.Union {
    public abstract class Object {
        private bool _enabled = true;

        public bool Enabled {
            get { return _enabled; }
            set {
                if (!_enabled && value) {
                    ExecuteOnEnable();
                }
                else if (_enabled && !value) {
                    ExecuteOnDisable();
                }

                _enabled = value;
            }
        }

        internal void ExecuteAwake() {
            if (Enabled) {
                Awake();
            }
        }

        internal void ExecuteStart() {
            if (Enabled) {
                Start();
            }
        }

        internal void ExecuteOnEnable() {
            OnEnable();
        }

        internal void ExecuteOnDisable() {
            OnDisable();
        }

        internal void ExecuteUpdate() {
            if (Enabled) {
                Update();
            }
        }

        internal void ExecuteFixedUpdate() {
            if (Enabled) {
                FixedUpdate();
            }
        }

        internal void ExecuteOnDestroy() {
            OnDestroy();
        }

        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }
        protected virtual void OnDestroy() { }
    }
}
