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
            //using( UserContext db = new UserContext() )
            //{
            //    // создаем два объекта User

            //    IEnumerable<User> users = db.Users.Where( u => u.Id  < 3 );

            //    Console.WriteLine( "Объекты успешно сохранены" );

            //    Console.WriteLine( "Список объектов:" );
            //    foreach( User u in users )
            //    {
            //        Console.WriteLine( "{0}.{1} - {2}", u.Id, u.Name, u.Age );
            //    }
            //}
            //Console.Read();

            //ICheckersFactory checkersFactory = new CheckersFactory();

            //IUserChecker checker = checkersFactory.CreateDataBaseUserChecker();

            //checker.Check( "maksim.vesnin", "max", "ftp" );

            FtpProxy.Program.Main( new string[] { } );
            Console.ReadKey( true );
            FtpProxy.Program.Stop();
        }
    }
}
