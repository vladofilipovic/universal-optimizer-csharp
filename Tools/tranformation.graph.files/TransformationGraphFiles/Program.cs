using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GraphFileTransformations
{
    class Program
    {
        [Verb("fromGraphmlToPlain", HelpText = "Generate file with plain representation of the graph, where graphml representation is given.")]
        class FromGraphmlToPlainOptions
        {
            [Option('i', "input", Required = true, HelpText = "Path for the input file (with plain data).")]
            public String Input { get; set; }
            [Option('o', "output", Required = true, HelpText = "Path for the output file (with graphml data).")]
            public String Output { get; set; }
            [Option('w', "wait", Required = false, Default=false, HelpText = "Set if program should wait for keystroke.")]
            public bool Wait { get; set; }
        }

        [Verb("fromPlainToGraphml", HelpText = "Generate file with  graphml representation of the graph, where plain representation is given.")]
        class FromPlainToGraphmlOptions
        {
            [Option('i', "input", Required = true, HelpText = "Path for the input file (with graphml data).")]
            public String Input { get; set; }
            [Option('o', "output", Required = true, HelpText = "Path for the output file (with plain data).")]
            public String Output { get; set; }
            [Option('w', "wait", Required = false, Default = false, HelpText = "Set if program should wait for keystroke.")]
            public bool Wait { get; set; }
        }


        static void Main(string[] args) => Parser.Default.ParseArguments<FromGraphmlToPlainOptions, FromPlainToGraphmlOptions>(args)
                .MapResult(
                      (FromGraphmlToPlainOptions opts) => RuFromGraphmlToPlainAndReturnExitCode(opts),
                      (FromPlainToGraphmlOptions opts) => RunFromPlainToGraphmlAndReturnExitCode(opts),
                      errs => 1);

        static int RuFromGraphmlToPlainAndReturnExitCode(FromGraphmlToPlainOptions opts)
        {
            string inputPath = opts.Input;
            string outputPath = opts.Output;
            int ret = FromGraphmlToPlain.transform(inputPath, outputPath);
            if(opts.Wait)
            {
                Console.WriteLine("Press any key to finish...");
                Console.ReadLine();
            }
            return ret;
        }

        static int RunFromPlainToGraphmlAndReturnExitCode(FromPlainToGraphmlOptions opts)
        {
            string inputPath = opts.Input;
            string outputPath = opts.Output;
            int ret = FromPlainToGraphml.transform(inputPath, outputPath);
            if (opts.Wait)
            {
                Console.WriteLine("Press any key to finish...");
                Console.ReadLine();
            }
            return ret;
        }


    }
}
