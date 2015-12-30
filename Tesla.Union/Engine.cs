using System;
using System.Collections.Generic;
using System.Threading;

namespace Tesla.Union {
    public abstract class Engine {
        protected readonly ProgramObject RootObject;
        protected float FixedStepMs;
        protected Timer Timer;

        protected Engine(ProgramObject rootObject, float fixedStepMs = 3f) {
            RootObject = rootObject;
            FixedStepMs = fixedStepMs;
        }

        protected Engine(IEnumerable<ProgramObject> objects, float fixedStepMs = 3f) {
            RootObject = new ProgramObject();
            RootObject.AddChildren(objects);
            FixedStepMs = fixedStepMs;
        }

        private void StartFixedStepTimer() {
            throw new NotImplementedException();
        }

        private void AdjustFixedStepTimer() {
            throw new NotImplementedException();
        }

        private void StopFixedStepTimer() {
            throw new NotImplementedException();
        }

        private void FixedStepTick() {
            throw new NotImplementedException();
        }

        private void MainLoop() {
            throw new NotImplementedException();
        }

        public virtual void StartExecution() { }
        public virtual void PauseExecution() { }
        public virtual void StopExecution() { }
    }
}
