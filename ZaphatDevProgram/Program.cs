using System;
using Zaphat.Application;

namespace ZaphatDevProgram
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var app = new ZapApp())
            {
                app.Run(60, 60);
            }

        }
    }
}
