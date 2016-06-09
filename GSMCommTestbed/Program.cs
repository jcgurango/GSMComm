using GsmComm.PduConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSMCommTestbed
{
    public class Program
    {
        static void Main(string[] args)
        {
            TextDataConverter.StringTo7Bit("Hello! This is another test. Please text back mhacdev at number");
        }
    }
}
