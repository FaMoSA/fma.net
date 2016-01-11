using System;
using System.Collections.Generic;

namespace Famosa.FMAnalyzer
{
    public interface FeatureModelConfigurator
    {
        FeatureModel getModel();
        List<ConfigurationStep> getSteps();
        List<Feature> getConfiguration();

        void reset();

        ConfigurationStep select(Feature feature);
        ConfigurationStep disable(Feature feature);

        void undoLastStep();
        void undoUntil(ConfigurationStep step);
        void undo(ConfigurationStep step);

        ConfigurationStep autoComplete();

        bool isDone();

    }

    public enum ConfigurationStepType
    {
        InitialPropagation,
        Select,
        Disable,
        Complete
    }

    public class ConfigurationStep
    {
        public int Id { get; set; }
        public ConfigurationStepType type { get; set; }

        public Feature decision { get; set; }
        public List<Feature> selectedFeatures { get; set; }
        public List<Feature> disabledFeatures { get; set; }

        public ConfigurationStep()
        {
            selectedFeatures = new List<Feature>();
            disabledFeatures = new List<Feature>();
        }        
    }

}
