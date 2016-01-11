using System;
using System.IO;
using System.Collections.Generic;
using Famosa.FMAnalyzer;

namespace FMAConfiguratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string FeatureIdeFileName = "..\\..\\..\\models\\model_dell.fide.xml";
                FeatureIDEFeatureModelLoader FeatureIdeLoader = new FeatureIDEFeatureModelLoader();
                FeatureModel FeatureIdeModel = FeatureIdeLoader.load(FeatureIdeFileName);

                FeatureModelCSPAnalyzer FeatureIdeSolver = new FeatureModelCSPAnalyzer(FeatureIdeModel);

                bool FeatureIdeValid = FeatureIdeSolver.IsValid();
                Console.WriteLine("Feature model");
                Console.WriteLine("valid = " + FeatureIdeValid);
                Console.WriteLine();

                FeatureModelConfigurator configurator = new FeatureModelCSPConfigurator(FeatureIdeModel);

                Console.WriteLine("Configurator (at start)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.autoComplete();
                Console.WriteLine("Configurator (after complete)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.undoLastStep();
                Console.WriteLine("Configurator (after undo last)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                ConfigurationStep selectProductCategory = configurator.select(FeatureIdeModel.FeatureMap["ProductCategory"]);
                Console.WriteLine("Configurator (after selecting)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.select(FeatureIdeModel.FeatureMap["Adamo"]);
                Console.WriteLine("Configurator (after selecting)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.autoComplete();
                Console.WriteLine("Configurator (after complete)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.undoLastStep();
                Console.WriteLine("Configurator (after undo last)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.autoComplete();
                Console.WriteLine("Configurator (after complete)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.undoUntil(selectProductCategory);
                Console.WriteLine("Configurator (after undo until ProductCategory)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                selectProductCategory = configurator.select(FeatureIdeModel.FeatureMap["ProductCategory"]);
                Console.WriteLine("Configurator (after selecting)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.select(FeatureIdeModel.FeatureMap["Adamo"]);
                Console.WriteLine("Configurator (after selecting)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.autoComplete();
                Console.WriteLine("Configurator (after complete)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                configurator.undo(selectProductCategory);
                Console.WriteLine("Configurator (after undo ProductCategory)");
                Console.WriteLine("#steps = " + configurator.getSteps().Count);
                Console.Write("config = ");
                foreach (Feature f in configurator.getConfiguration())
                {
                    Console.Write(f.Name + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

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
