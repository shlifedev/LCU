using LCU.ModelGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LCU
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeGen.Generate();
            return;
            LCUClient client = new LCUClient();
            client.Initialize(); 
            System.Threading.Thread.Sleep(100000);
        }
    } 
}
