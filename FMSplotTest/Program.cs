using System;
using System.IO;
using System.Collections.Generic;
using Famosa.FMAnalyzer;

namespace FMSplotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string SplotFileName = "..\\..\\..\\models\\model_dell.xml";
                string FeatureIdeFileName = "..\\..\\..\\models\\model_dell.fide.xml";

                SplotFeatureModelLoader SplotLoader = new SplotFeatureModelLoader();
                FeatureModel SplotModel = SplotLoader.load(SplotFileName);

                // Use the solver

                FeatureModelCSPAnalyzer SplotSolver = new FeatureModelCSPAnalyzer(SplotModel);

                // Process the model

                bool SplotValid = SplotSolver.IsValid();
                long SplotNumber = SplotSolver.NumberOfProducts();
                List<Feature> SplotDead = SplotSolver.GetDeadFeatures();


                FeatureIDEFeatureModelLoader FeatureIdeLoader = new FeatureIDEFeatureModelLoader();
                FeatureModel FeatureIdeModel = FeatureIdeLoader.load(FeatureIdeFileName);

                FeatureModelCSPAnalyzer FeatureIdeSolver = new FeatureModelCSPAnalyzer(FeatureIdeModel);

                bool FeatureIdeValid = FeatureIdeSolver.IsValid();
                long FeatureIdeNumber = FeatureIdeSolver.NumberOfProducts();
                List<Feature> FeatureIdeDead = FeatureIdeSolver.GetDeadFeatures();

                Console.WriteLine("valid = " + SplotValid + " = " + FeatureIdeValid);
                Console.WriteLine("number = " + SplotNumber + " = " + FeatureIdeNumber);
                Console.WriteLine("dead = ");
                foreach (Feature dead in SplotDead)
                    Console.Write(dead.Name + " ");
                Console.WriteLine();
                foreach (Feature dead in FeatureIdeDead)
                    Console.Write(dead.Name + " ");

                Console.ReadLine();
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("{0}", e.Message);
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
