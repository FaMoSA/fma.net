using System;
using System.Collections.Generic;
using Microsoft.SolverFoundation.Solvers;

namespace Famosa.FMAnalyzer
{
    public class FeatureModelCSPAnalyzer : FeatureModelAnalyzer
    {
        private FeatureModel model;

        private bool unprocessed = true;
        private bool valid = true;
        private long count = 0;

        ConstraintSystem S = null;
        ConstraintSolverSolution soln = null;

        List<Configuration> configs = new List<Configuration>();
        FeatureModelToCSPConverter converter = null;

        public FeatureModelCSPAnalyzer(FeatureModel model)
        {
            this.converter = new FeatureModelToCSPMendoncaConverter();
            this.model = model;
            S = converter.convert(this.model);
        }

        private void process()
        {
            if (unprocessed)
            {
                soln = S.Solve();
                unprocessed = false;
                valid = soln.HasFoundSolution;
            }
        }

        public bool IsValid()
        {
            process();
            return valid;
        }

        public bool hasMoreConfigurations()
        {
            process();
            return soln.HasFoundSolution;
        }

        public long NumberOfProducts(int maxNumber = 50000)
        {
            process();
            while (hasMoreConfigurations() && count < maxNumber)
                getNextConfiguration();
            return count;
        }

        public Configuration getNextConfiguration()
        {
            process();
            if (soln.HasFoundSolution)
            {
                Configuration config = new Configuration();
                foreach (Feature feature in model.Features)
                    if (soln[converter.convertFeature(feature)].ToString() == "1")
                        config.AddFeature(feature);
                configs.Add(config);
                count++;

                soln.GetNext();

                return config;
            }
            return null;
        }

        public List<Configuration> GetAllConfigurations()
        {
            process();
            while (hasMoreConfigurations())
                getNextConfiguration();
            return configs;
        }

        public bool IsValidConfiguration(Configuration config)
        {
            FeatureModelToCSPConverter converter2 = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S2 = converter2.convert(this.model);
            foreach (Feature f in model.Features)
                if (config.Contains(f))
                    S2.AddConstraints(converter2.convertFeature(f));
                else
                    S2.AddConstraints(S2.Not(converter2.convertFeature(f)));

            ConstraintSolverSolution sol2 = S2.Solve();
            return (sol2.HasFoundSolution);
        }

        public bool IsValidPartialConfiguration(Configuration config)
        {
            FeatureModelToCSPConverter converter2 = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S2 = converter2.convert(this.model);
            foreach (Feature f in config.features)
                S2.AddConstraints(converter2.convertFeature(f));

            ConstraintSolverSolution sol2 = S2.Solve();
            return (sol2.HasFoundSolution);
        }

        public List<Feature> GetDeadFeatures()
        {
            List<Feature> features = new List<Feature>();

            FeatureModelToCSPConverter converter2 = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S2 = converter2.convert(this.model);

            foreach (Feature f in this.model.Features)
            {

                CspTerm constraint = converter2.convertFeature(f);
                S2.AddConstraints(constraint);

                ConstraintSolverSolution sol2 = S2.Solve();
                if (!sol2.HasFoundSolution)
                    features.Add(f);

                S2.ResetSolver();
                S2.RemoveConstraints(constraint);
            }

            return features;
        }

        public List<Feature> GetCoreFeatures()
        {
            List<Feature> features = new List<Feature>();

            FeatureModelToCSPConverter converter2 = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S2 = converter2.convert(this.model);

            foreach (Feature f in this.model.Features)
            {

                CspTerm constraint = S.Not(converter2.convertFeature(f));
                S2.AddConstraints(constraint);

                ConstraintSolverSolution sol2 = S2.Solve();
                if (!sol2.HasFoundSolution)
                    features.Add(f);

                S2.ResetSolver();
                S2.RemoveConstraints(constraint);
            }

            return features;
        }

        public List<Feature> GetVariantFeatures()
        {
            List<Feature> features = new List<Feature>();

            List<Feature> coreFeatures = GetCoreFeatures();
            List<Feature> deadFeatures = GetCoreFeatures();

            foreach (Feature f in this.model.Features)
                if (!coreFeatures.Contains(f) && !deadFeatures.Contains(f))
                    features.Add(f);

            return features;
        }
    }



    public interface FeatureModelToCSPConverter
    {
        ConstraintSystem convert(FeatureModel model);
        CspTerm convertFeature(Feature feature);
    }



	public abstract class AbstractFeatureModelToCSPConverter : FeatureModelToCSPConverter
    {
        abstract public ConstraintSystem convert(FeatureModel model);

        protected ConstraintSystem S = ConstraintSystem.CreateSolver();
        protected bool debug = false;

        protected Dictionary<string, CspTerm> terms = new Dictionary<string, CspTerm>();

        // Named terms for a CSP problem
        delegate CspTerm NamedTerm(string name);

        // Feature 
        protected CspTerm AddFeature(string name)
        {
            CspTerm x = S.CreateVariable(S.DefaultBoolean, name);
            this.terms.Add(name, x);
            if (debug)
                Console.WriteLine("variable " + name);
            return x;
        }

        public CspTerm convertFeature(Feature feature)
        {
            return terms[feature.Name];
        }

        protected CspTerm[] convertChildren(Feature feature)
        {
            List<CspTerm> children = new List<CspTerm>();
            foreach (Feature child in feature.Children)
            {
                children.Add(terms[child.Name]);
            }
            return children.ToArray();
        }

        protected string childrenNames(Feature feature)
        {
            string children = "";
            foreach (Feature child in feature.Children)
            {
                if (children != "")
                    children += ", ";
                children += child.Name;
            }
            return children;
        }

        protected CspTerm processConstraintTerm(ConstraintTerm term)
        {
            if (term is Var)
                return convertFeature(((Var)term).Feature);
            else if (term is Not)
                return S.Not(processConstraintTerm(((Not)term).Term));
            else if (term is Conj)
                return S.And(
                        processConstraintTerm(((Conj)term).Left),
                        processConstraintTerm(((Conj)term).Right)
                    );
            else if (term is Disj)
                return S.Or(
                        processConstraintTerm(((Disj)term).Left),
                        processConstraintTerm(((Disj)term).Right)
                    );
            else if (term is Implies)
                return S.Implies(
                        processConstraintTerm(((Implies)term).Left),
                        processConstraintTerm(((Implies)term).Right)
                    );
            else if (term is Equals)
                return S.Equal(
                        processConstraintTerm(((Equals)term).Left),
                        processConstraintTerm(((Equals)term).Right)
                    );
            else
                return null;
        }

    }



    // using Mendonca,SPLC09
    public class FeatureModelToCSPMendoncaConverter : AbstractFeatureModelToCSPConverter
    {

        override public ConstraintSystem convert(FeatureModel model)
        {
            // create a CspTerm for each Feature
            foreach (Feature f in model.Features)
                AddFeature(f.Name);

            // process the structure to add Constraints
            S = processRoot(S, model.Root);

            // process the constraints
            foreach (Constraint constraint in model.Constraints)
                S.AddConstraints(processConstraintTerm(constraint.Term));

            return S;
        }

        private ConstraintSystem processRoot(ConstraintSystem S, Feature feature)
        {
            // root  maps to:  root
            S.AddConstraints(terms[feature.Name]);
            if (debug)
                Console.WriteLine(feature.Name);

            S = processFeature(S, feature);
            return S;
        }

        private ConstraintSystem processFeature(ConstraintSystem S, Feature feature)
        {
            if (feature is AlternativeFeatureGroup)
            {
                // altgroup maps to:  parent implies atleast1(..) and atmost1(...)
                S.AddConstraints(
                    S.Implies(
                        convertFeature(feature),
                        S.Equal(1, S.Sum(convertChildren(feature)))
                        )
                    );
                if (debug)
                    Console.WriteLine(feature.Name + " implies Sum(" + childrenNames(feature) + ") = 1");
            }

            if (feature is OrFeatureGroup)
            {
                // orgroup maps to:  parent implies atleast1(..)
                S.AddConstraints(
                    S.Implies(
                        convertFeature(feature),
                        S.Or(convertChildren(feature))
                        )
                    );
                if (debug)
                    Console.WriteLine(feature.Name + " implies Or(" + childrenNames(feature) + ")");
            }

            foreach (Feature child in feature.Children)
            {
                // parent-child   maps to:  child implies parent
                S.AddConstraints(S.Implies(convertFeature(child), convertFeature(feature)));
                if (debug)
                    Console.WriteLine(child.Name + " implies " + feature.Name);

                // mandatory child maps to: parent implies child
                if (child.IsMandatory)
                {
                    S.AddConstraints(S.Implies(convertFeature(feature), convertFeature(child)));
                    if (debug)
                        Console.WriteLine(feature.Name + " implies " + child.Name);
                }

                S = processFeature(S, child);
            }
            return S;
        }
    }

}
