using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.ML.Transforms.LpNormNormalizingEstimatorBase;

namespace ServicesLibrary.Model.Extensions
{
    public static class NormalizeTransformExtensions
    {
        //--------------- NORMALIZE DATA ------------------------------------------------------
        public static IEstimator<ITransformer> _NormalizeMeanVariance(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            long maxExampleCount = componentObject.Value<long>("MaximumExampleCount");
            bool fixZero = componentObject.Value<bool>("FixZero");
            bool useCdf = componentObject.Value<bool>("UseCdf");
            return MLContext.Transforms.NormalizeMeanVariance(outputColumn, inputColumn, maxExampleCount, fixZero, useCdf);
        }

        public static IEstimator<ITransformer> _NormalizeLogMeanVariance(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            long maxExampleCount = componentObject.Value<long>("MaximumExampleCount");
            bool useCdf = componentObject.Value<bool>("UseCdf");
            return MLContext.Transforms.NormalizeLogMeanVariance(outputColumn, inputColumn, maxExampleCount, useCdf);
        }

        public static IEstimator<ITransformer> _NormalizeLpNorm(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            NormFunction normFunction = Enum.Parse<NormFunction>(componentObject.Value<string>("NormFunction"));
            bool ensureZeroMean = componentObject.Value<bool>("EnsureZeroMean");
            return MLContext.Transforms.NormalizeLpNorm(outputColumn, inputColumn, normFunction, ensureZeroMean);
        }

        public static IEstimator<ITransformer> _NormalizeGlobalContrast(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            bool ensureZeroMean = componentObject.Value<bool>("EnsureZeroMean");
            bool ensureUnitStdDev = componentObject.Value<bool>("EnsureUnitStdDev");
            float scale = componentObject.Value<float>("Scale");
            return MLContext.Transforms.NormalizeGlobalContrast(outputColumn, inputColumn, ensureZeroMean, ensureUnitStdDev, scale);
        }

        public static IEstimator<ITransformer> _NormalizeBinning(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            long maxExampleCount = componentObject.Value<long>("MaximumExampleCount");
            bool fixZero = componentObject.Value<bool>("FixZero");
            int maxBinCount = componentObject.Value<int>("MaximumBinCount");
            return MLContext.Transforms.NormalizeBinning(outputColumn, inputColumn, maxExampleCount, fixZero, maxBinCount);
        }

        public static IEstimator<ITransformer> _NormalizeSupervisedBinning(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            long maxExampleCount = componentObject.Value<long>("MaximumExampleCount");
            bool fixZero = componentObject.Value<bool>("FixZero");
            int maxBinCount = componentObject.Value<int>("MaximumBinCount");
            int minExamplesPerBin = componentObject.Value<int>("MinimumExamplesPerBin");
            return MLContext.Transforms.NormalizeSupervisedBinning(outputColumn, inputColumn, labelColumn, maxExampleCount, fixZero, maxBinCount, minExamplesPerBin);
        }

        public static IEstimator<ITransformer> _NormalizeMinMax(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            long maxExampleCount = componentObject.Value<long>("MaximumExampleCount");
            bool fixZero = componentObject.Value<bool>("FixZero");
            return MLContext.Transforms.NormalizeMinMax(outputColumn, inputColumn, maxExampleCount, fixZero);
        }
    }
}
