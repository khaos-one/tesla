using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tesla.Union {
    public class WindowsEngine
        : Engine {
        private Control _host;

        public WindowsEngine(Control host, ProgramObject rootObject) 
            : base(rootObject) {
            _host = host;
            HookOnWindowEvents();
        }

        public WindowsEngine(Control host, IEnumerable<ProgramObject> objects) 
            : base(objects) {
            _host = host;
            HookOnWindowEvents();
        }

        private void HookOnWindowEvents() {
            _host.Paint += Host_OnPaint;
        }

        private void Host_OnPaint(object sender, PaintEventArgs paintEventArgs) {
            RootObject.ExecuteUpdate();
        }
    }
}
