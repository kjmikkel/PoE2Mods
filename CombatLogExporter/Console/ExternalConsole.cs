using Patchwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatLogExporter
{
    [NewType]
    class ExternalConsole
    {
        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;

        public ExternalConsole()
        {

        }

        public void WriteToConsole()
        {

        }
    }
}
