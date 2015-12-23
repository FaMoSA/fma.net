using System;

namespace Famosa.FMAnalyzer
{
    public interface FeatureModelLoader
    {
        FeatureModel load(String filename);
    }
}
