using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vlookup_Function
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {   //This is test the commit process
            Console.WriteLine("Please Identify Locale for Export");
            string locale = Console.ReadLine();
            Console.WriteLine("Please Choose EDW File");
            OpenFileDialog filepathedw = new OpenFileDialog();
            filepathedw.ShowDialog();
            string filepathvalueedw = filepathedw.FileName;
            Console.WriteLine("Please Choose eBay Active File");
            OpenFileDialog filepathactive = new OpenFileDialog();
            filepathactive.ShowDialog();
            string filepathvalueactive = filepathactive.FileName;

            //EDW File Read and Breakdown
            List<string> edwapnums = new List<string>();
            foreach(string edwrow in System.IO.File.ReadLines(filepathvalueedw).Skip(2))
                {
                string delimiter = "\t";
                string[] edwrowfields = edwrow.Split(delimiter.ToCharArray());
                if (locale.ToLower() == "art")
                {
                    edwapnums.Add(edwrowfields[1]);
                    ;
                }
                if (locale.ToLower() == "apc")
                {
                    edwapnums.Add(edwrowfields[0]);
                }
                }

            //Active File Read and Breakdown
            List<string> activeapnums = new List<string>();
            foreach (string activerow in System.IO.File.ReadLines(filepathvalueactive).Skip(1))
            {
                string delimiter = ",";
                string[] activerowfields = activerow.Split(delimiter.ToCharArray());
                activeapnums.Add(activerowfields[1].Replace("AP","").Replace("_PC0_FI0_SV0_IN1","").Replace("_PC0_FI0_SV0_IN1",""));
            }

            //Check List(s)
            List<string> FinalListAPNUMs = new List<string>();
            Parallel.ForEach(activeapnums, apnumactive =>
            {
                if (apnumactive.Contains("V_") == false)
                {
                    if (edwapnums.Contains(apnumactive) == true)
                    {
                        FinalListAPNUMs.Add(apnumactive);
                    }
                }
            });

            System.IO.File.WriteAllLines(@"C:\Users\sfox\Desktop\VlookupFinal " + locale + " "+   DateTime.Today.ToShortDateString().Replace("/", "-") + ".txt", FinalListAPNUMs);

        }
    }
}
