using System.Collections.Generic;

namespace Famosa.FMAnalyzer
{
    public interface FeatureModelAnalyzer
    {
        bool IsValid();
        long NumberOfProducts(int maxNumber=50000);

        bool hasMoreConfigurations();
        Configuration getNextConfiguration();

        List<Configuration> GetAllConfigurations();

        List<Feature> GetCoreFeatures();
        List<Feature> GetDeadFeatures();
        List<Feature> GetVariantFeatures();

        bool IsValidConfiguration(Configuration config);
        bool IsValidPartialConfiguration(Configuration config);

    }

}
