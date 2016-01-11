using System.Collections.Generic;
using Microsoft.SolverFoundation.Solvers;

namespace Famosa.FMAnalyzer
{
    public class FeatureModelCSPConfigurator : FeatureModelConfigurator
    {
        FeatureModel model;
        List<ConfigurationStep> steps { get; set; }

        List<Feature> selectedFeatures = new List<Feature>();
        List<Feature> disabledFeatures = new List<Feature>();

        public FeatureModelCSPConfigurator(FeatureModel model)
        {
            this.model = model;
            this.steps = new List<ConfigurationStep>();
            reset();
        }

        public FeatureModel getModel()
        {
            return this.model;
        }

        public List<ConfigurationStep> getSteps()
        {
            return this.steps;
        }

        public List<Feature> getConfiguration()
        {
            return this.selectedFeatures;
        }

        public void reset()
        {
            FeatureModelToCSPConverter converter = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S = converter.convert(this.model);

            this.steps = new List<ConfigurationStep>();
            this.selectedFeatures = new List<Feature>();
            this.disabledFeatures = new List<Feature>();

            ConfigurationStep step = new ConfigurationStep();
            step.type = ConfigurationStepType.InitialPropagation;
            step.decision = model.Root;
            // step.selectedFeatures = GetSelectedFeatures(model.Root);
            // step.disabledFeatures = GetDisabledFeatures(model.Root);
            this.steps.Add(step);

            this.selectedFeatures.Add(model.Root);
            this.selectedFeatures.AddRange(step.selectedFeatures);
            this.disabledFeatures.AddRange(step.disabledFeatures);

        }

        private ConstraintSystem convert()
        {
            return this.convert(new FeatureModelToCSPMendoncaConverter());
        }

        private ConstraintSystem convert(FeatureModelToCSPConverter converter)
        {
            ConstraintSystem S = converter.convert(this.model);

            foreach(ConfigurationStep step in this.steps)
            {
                switch (step.type)
                {
                    case ConfigurationStepType.Select:
                        S.AddConstraints(converter.convertFeature(step.decision));
                        break;
                    case ConfigurationStepType.Disable:
                        S.AddConstraints(S.Not(converter.convertFeature(step.decision)));
                        break;
                }
            }

            return S;
        }

        private void recalculate()
        {
            List<ConfigurationStep> oldSteps = this.steps;

            reset();

            foreach (ConfigurationStep step in oldSteps)
            {
                switch (step.type)
                {
                    case ConfigurationStepType.Select:
                        select(step.decision);
                        break;
                    case ConfigurationStepType.Disable:
                        disable(step.decision);
                        break;
                }
            }
        }

        public ConfigurationStep select(Feature feature)
        {
            FeatureModelToCSPConverter converter = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S = this.convert(converter);

            CspTerm constraint = converter.convertFeature(feature);
            S.ResetSolver();
            S.AddConstraints(constraint);

            ConstraintSolverSolution sol = S.Solve();

            // is valid ?
            if (sol.HasFoundSolution)
            {
                this.selectedFeatures.Add(feature);
    
                ConfigurationStep step = new ConfigurationStep();
                step.Id = steps.Count;
                step.type = ConfigurationStepType.Select;
                step.decision = feature;
                this.steps.Add(step);

                List<Feature> selectedFeatures = GetSelectedFeatures(feature);
                this.selectedFeatures.AddRange(selectedFeatures);
                step.selectedFeatures.AddRange(selectedFeatures);

                List<Feature> disabledFeatures = GetDisabledFeatures(feature);
                this.disabledFeatures.AddRange(disabledFeatures);
                step.disabledFeatures.AddRange(disabledFeatures);

                return step;
            }
            			
            return null;
        }

        public ConfigurationStep disable(Feature feature)
        {
            FeatureModelToCSPConverter converter = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S = this.convert(converter);

            CspTerm constraint = S.Not(converter.convertFeature(feature));
            S.ResetSolver();
            S.AddConstraints(constraint);

            ConstraintSolverSolution sol = S.Solve();

            // is valid ?
            if (sol.HasFoundSolution)
            {
                this.disabledFeatures.Add(feature);

                ConfigurationStep step = new ConfigurationStep();
                step.Id = steps.Count;
                step.type = ConfigurationStepType.Select;
                step.decision = feature;
                this.steps.Add(step);

                List<Feature> selectedFeatures = GetSelectedFeatures(feature);
                foreach (Feature f in selectedFeatures)
                    if (!this.selectedFeatures.Contains(f))
                    {
                        this.selectedFeatures.Add(f);
                        step.selectedFeatures.Add(f);
                    }

                List<Feature> disabledFeatures = GetDisabledFeatures(feature);
                foreach (Feature f in disabledFeatures)
                    if (!this.disabledFeatures.Contains(f))
                    {
                        this.disabledFeatures.Add(f);
                        step.disabledFeatures.Add(f);
                    }

                this.steps.Add(step);

                return step;
            }

            return null;
        }

        public void undoLastStep()
        {
            this.steps.RemoveAt(this.steps.Count - 1);
            recalculate();
        }

        public void undoUntil(ConfigurationStep step)
        {
            int index = -1;
            for(int i = 0; i < this.steps.Count; i++)
            {
                if (this.steps[i].type == step.type && this.steps[i].decision == step.decision )
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                this.steps.RemoveRange(index, this.steps.Count - index);
                recalculate();
            }
        }

        public void undo(ConfigurationStep step)
        {
            int index = -1;
            for (int i = 0; i < this.steps.Count; i++)
            {
                if (this.steps[i].type == step.type && this.steps[i].decision == step.decision)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                this.steps.RemoveAt(index);
                recalculate();
            }
        }

        public ConfigurationStep autoComplete()
        {
            FeatureModelToCSPConverter converter = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S = this.convert(converter);
            ConstraintSolverSolution sol = S.Solve();

            // is valid ?
            if (sol.HasFoundSolution)
            {
                ConfigurationStep step = new ConfigurationStep();
                step.Id = steps.Count;
                step.type = ConfigurationStepType.Complete;
                step.decision = null;

                foreach (Feature f in model.Features)
                {
                    CspTerm cf = converter.convertFeature(f);
                    if ( ((int) sol[cf]) == 1)
                    {
                        if (!this.selectedFeatures.Contains(f))
                        {
                            this.selectedFeatures.Add(f);
                            step.selectedFeatures.Add(f);
                        }
                    }
                    else
                    {
                        if (!this.disabledFeatures.Contains(f))
                        {
                            this.disabledFeatures.Add(f);
                            step.disabledFeatures.Add(f);
                        }
                    }
                }
                this.steps.Add(step);

                return step;
            }

            return null;
        }

        public bool isDone()
        {
            foreach (Feature f in model.Features)
                if (!this.disabledFeatures.Contains(f) && !this.selectedFeatures.Contains(f))
                    return false;

            return true;
        }

        // 

        private List<Feature> GetDisabledFeatures(Feature feature)
        {
            List<Feature> features = new List<Feature>();

            FeatureModelToCSPConverter converter = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S = this.convert(converter);

            foreach (Feature f in this.model.Features)
            {
                if (f != feature && !selectedFeatures.Contains(f) && !disabledFeatures.Contains(f))
                {
                    CspTerm constraint = converter.convertFeature(f);
                    S.AddConstraints(constraint);

                    ConstraintSolverSolution sol2 = S.Solve();
                    if (!sol2.HasFoundSolution)
                        features.Add(f);

                    S.ResetSolver();
                    S.RemoveConstraints(constraint);
                }

            }

            return features;
        }

        private List<Feature> GetSelectedFeatures(Feature feature)
        {
            List<Feature> features = new List<Feature>();

            FeatureModelToCSPConverter converter = new FeatureModelToCSPMendoncaConverter();
            ConstraintSystem S = this.convert(converter);

            foreach (Feature f in this.model.Features)
            {
                if (f != feature && !selectedFeatures.Contains(f) && !disabledFeatures.Contains(f))
                {
                    CspTerm constraint = S.Not(converter.convertFeature(f));
                    S.AddConstraints(constraint);

                    ConstraintSolverSolution sol2 = S.Solve();
                    if (!sol2.HasFoundSolution)
                        features.Add(f);

                    S.ResetSolver();
                    S.RemoveConstraints(constraint);
                }
            }

            return features;
        }

    }
}
