using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UsersLib.Checkers;
using UsersLib.Factories;

namespace DebugApp
{
    class Program
    {
        static void Main( string[] args )
        {
            FtpProxy.Program.Main( new string[] { } );
            Console.ReadKey( true );
            FtpProxy.Program.Stop();
        }
    }
}
