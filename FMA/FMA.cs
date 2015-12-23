using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace Famosa.FMAnalyzer
{
    class Test
    {
        static void Main(string[] args)
        {
            try
            {
                // Get the command line arguments

                if (args.Length == 0)
                {
                    // Console.WriteLine("wrong number of parameters");
                    DoTest();

                }
                else
                {
                    string fileName = args[0];
                    string solverName = (args.Length == 2) ? args[1] : "";


                    // load the model

                    FeatureIDEFeatureModelLoader loader = new FeatureIDEFeatureModelLoader();
                    FeatureModel model = loader.load(fileName);

                    // Use the solver

                    FeatureModelCSPAnalyzer solver = new FeatureModelCSPAnalyzer(model);

                    // Process the model

                    Console.Write("{0},{1}", fileName, solverName);

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    bool valid = solver.IsValid();
                    stopwatch.Stop();
                    Console.Write(",{0},{1}", valid, stopwatch.ElapsedMilliseconds);

                    stopwatch.Restart();
                    long number = solver.NumberOfProducts();
                    stopwatch.Stop();
                    Console.Write(",{0},{1}", number, stopwatch.ElapsedMilliseconds);

                    stopwatch.Restart();
                    List<Feature> dead = solver.GetDeadFeatures();
                    stopwatch.Stop();
                    Console.Write(",{0},{1}", dead.Count, stopwatch.ElapsedMilliseconds);


                    Console.WriteLine();
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("{0}", e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}", e.Message);
            }


        }

        static void DoTest()
        {
            try
            {
                FeatureIDEFeatureModelLoader loader = new FeatureIDEFeatureModelLoader();

                // loader.load("..\\..\\examples\\FamaTS-1.2\\error-guessing\\dead-features\\case1\\df-case1.fama");
                FeatureModel model = loader.load("..\\..\\model.xml");

                // Ours

                FeatureModelCSPAnalyzer solver = new FeatureModelCSPAnalyzer(model);

                Stopwatch stopwatch = Stopwatch.StartNew();
                bool valid = solver.IsValid();
                stopwatch.Stop();

                Console.WriteLine("valid = " + valid);
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();
                long number = solver.NumberOfProducts();
                stopwatch.Stop();

                Console.WriteLine("# products = " + solver.NumberOfProducts());
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();
                List<Feature> dead = solver.GetDeadFeatures();
                stopwatch.Stop();

                Console.WriteLine("# dead = " + dead.Count);
                Console.WriteLine(stopwatch.ElapsedMilliseconds);

                /*
                while(solver.hasMoreConfigurations())
                {
                    Console.WriteLine("\t+Config = " + solver.getNextConfiguration());
                }
                */

                /*
                // invalid configuration, valid partial configuration
                Configuration config1 = new Configuration();
                config1.AddFeature(model.FeatureMap["GPL"]);

                Console.WriteLine("valid config1 = " + solver.IsValidConfiguration(config1));
                Console.WriteLine("valid partial config1 = " + solver.IsValidPartialConfiguration(config1));


                // Valid configuration (also valid partial configuration)
                Configuration config2 = new Configuration();
                config2.AddFeaturesByName(model, new[]{ "GPL", "MainGpl", "Driver", "DriverProg", "Prog", "Benchmark", "Alg", "Number", "Src", "BFS", "Wgt", "Weighted", "Gtp", "Directed"});

                Console.WriteLine("valid config2 = " + solver.IsValidConfiguration(config2));
                Console.WriteLine("valid partial config2 = " + solver.IsValidPartialConfiguration(config2));
                */

                Console.ReadKey();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("{0}", e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("{0}", e.Message);
            }
        }

    }
}
