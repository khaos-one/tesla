using System;
using System.Collections.Generic;
using System.Linq;

namespace Tesla.Union {
    public class ProgramObject
        : Object {
        private string _name;
        private ProgramObject _parent;
        private List<ProgramObject> _children = new List<ProgramObject>();
        private List<Component> _components = new List<Component>();

        public ProgramObject() { }
        public ProgramObject(string name) {
            _name = name;
        }

        public void AddComponent<T>()
            where T : Component, new() {
            _components.Add(new T());
        }

        public T GetComponent<T>()
            where T : Component, new() {
            var t = typeof (T);
            return (T) _components.FirstOrDefault(x => x.GetType() == t);
        }

        public void AddChild(ProgramObject obj) {
            _children.Add(obj);
        }

        public void AddChildren(IEnumerable<ProgramObject> obj) {
            _children.AddRange(obj);
        }

        public IEnumerable<ProgramObject> GetChildren() {
            return new List<ProgramObject>(_children);
        }

        private void ExecuteOnComponentsAndChildren(Action<Object> action) {
            _components.ForEach(action);
            _children.ForEach(action);
        }

        protected override void Awake() {
            base.Awake();
            ExecuteOnComponentsAndChildren(x => x.ExecuteAwake());
        }

        protected override void Start() {
            base.Start();
            ExecuteOnComponentsAndChildren(x => x.ExecuteStart());
        }

        protected override void Update() {
            base.Update();
            ExecuteOnComponentsAndChildren(x => x.ExecuteUpdate());
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            ExecuteOnComponentsAndChildren(x => x.ExecuteFixedUpdate());
        }

        protected override void OnEnable() {
            base.OnEnable();
            ExecuteOnComponentsAndChildren(x => x.ExecuteOnEnable());
        }

        protected override void OnDisable() {
            base.OnDisable();
            ExecuteOnComponentsAndChildren(x => x.ExecuteOnDisable());
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            ExecuteOnComponentsAndChildren(x => x.ExecuteOnDestroy());
        }
    }
}
