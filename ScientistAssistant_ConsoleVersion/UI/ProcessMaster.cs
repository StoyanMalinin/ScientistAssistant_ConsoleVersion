using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.UI
{
    static class ProcessMaster
    {
        public static void loop()
        {
            InputHandler ih = new InputHandler();

            while(true)
            {
                ih.handleInput();
            }
        }
    }
}
