using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class TimeSeriesTransformExtensions
    {
        //--------------- TIME-SERIES TRANSFORMATIONS -----------------------------------------
        public static IEstimator<ITransformer> _DetectAnomalyBySrCnn(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int windowSize = componentObject.Value<int>("WindowSize");
            int backAddWindow = componentObject.Value<int>("BackAddWindowSize");
            int lookAheadWindow = componentObject.Value<int>("LookAheadWindowSize");
            int averagingWindow = componentObject.Value<int>("AveragingWindowSize");
            int judgementWindow = componentObject.Value<int>("JudgementWindowSize");
            double threshold = componentObject.Value<double>("Threshold");
            return MLContext.Transforms.DetectAnomalyBySrCnn(outputColumn, inputColumn, windowSize, backAddWindow,
                                                             lookAheadWindow, averagingWindow, judgementWindow,
                                                             threshold);
        }

        public static IEstimator<ITransformer> _DetectChangePointBySsa(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int confidence = componentObject.Value<int>("Confidence");
            int changeHistory = componentObject.Value<int>("ChangeHistoryLength");
            int trainingWindow = componentObject.Value<int>("TrainingWindowSize");
            int seasonalityWindow = componentObject.Value<int>("SeasonalityWindowSize");
            ErrorFunction errorFunction = Enum.Parse<ErrorFunction>(componentObject.Value<string>("ErrorFunction"));
            MartingaleType martingale = Enum.Parse<MartingaleType>(componentObject.Value<string>("MartingaleType"));
            double eps = componentObject.Value<double>("Eps");
            return MLContext.Transforms.DetectChangePointBySsa(outputColumn, inputColumn, confidence, changeHistory,
                                                               trainingWindow, seasonalityWindow, errorFunction,
                                                               martingale, eps);
        }

        public static IEstimator<ITransformer> _DetectIidChangePoint(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int confidence = componentObject.Value<int>("Confidence");
            int changeHistory = componentObject.Value<int>("ChangeHistoryLength");
            MartingaleType martingale = Enum.Parse<MartingaleType>(componentObject.Value<string>("MartingaleType"));
            double eps = componentObject.Value<double>("Eps");
            return MLContext.Transforms.DetectIidChangePoint(outputColumn, inputColumn, confidence, changeHistory, martingale, eps);
        }

        public static IEstimator<ITransformer> _ForecastBySsa(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int windowSize = componentObject.Value<int>("WindowSize");
            int seriesLength = componentObject.Value<int>("SeriesLength");
            int trainSize = componentObject.Value<int>("TrainSize");
            int horizon = componentObject.Value<int>("Horizon");
            bool isAdaptive = componentObject.Value<bool>("IsAdaptive");
            float discountFactor = componentObject.Value<float>("DiscountFactor");
            RankSelectionMethod rankSelection = Enum.Parse<RankSelectionMethod>(componentObject.Value<string>("RankSelectionMethod"));
            int rank = componentObject.Value<int>("Rank");
            int maxRank = componentObject.Value<int>("MaximumRank");
            bool shouldStabilize = componentObject.Value<bool>("ShouldStabilize");
            bool shouldMaintain = componentObject.Value<bool>("ShouldMaintainInfo");
            GrowthRatio maxGrowth = new GrowthRatio() { Growth = componentObject.Value<double>("Growth"), TimeSpan = componentObject.Value<int>("TimeSpan") };
            string lowerBoundColumn = componentObject.Value<string>("ConfidenceLowerBoundColumn");
            string upperBoundColumn = componentObject.Value<string>("ConfidenceUpperBoundColumn");
            float confidenceLevel = componentObject.Value<float>("ConfidenceLevel");
            bool variableHorizon = componentObject.Value<bool>("VariableHorizon");
            return MLContext.Forecasting.ForecastBySsa(outputColumn, inputColumn, windowSize, seriesLength, trainSize,
                                                       horizon, isAdaptive, discountFactor, rankSelection, rank, maxRank,
                                                       shouldStabilize, shouldMaintain, maxGrowth, lowerBoundColumn,
                                                       upperBoundColumn, confidenceLevel, variableHorizon);
        }

        public static IEstimator<ITransformer> _DetectSpikeBySsa(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int confidence = componentObject.Value<int>("Confidence");
            int pValueHistory = componentObject.Value<int>("PvalueHistoryLength");
            int trainingWindow = componentObject.Value<int>("TrainingWindowSize");
            int seasonalityWindow = componentObject.Value<int>("SeasonalityWindowSize");
            AnomalySide side = Enum.Parse<AnomalySide>(componentObject.Value<string>("AnomalySide"));
            ErrorFunction errorFunction = Enum.Parse<ErrorFunction>(componentObject.Value<string>("ErrorFunction"));
            return MLContext.Transforms.DetectSpikeBySsa(outputColumn, inputColumn, confidence, pValueHistory,
                                                         trainingWindow, seasonalityWindow, side, errorFunction);
        }

        public static IEstimator<ITransformer> _DetectIidSpike(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int confidence = componentObject.Value<int>("Confidence");
            int pValueHistory = componentObject.Value<int>("PvalueHistoryLength");
            AnomalySide side = Enum.Parse<AnomalySide>(componentObject.Value<string>("AnomalySide"));
            return MLContext.Transforms.DetectIidSpike(outputColumn, inputColumn, confidence, pValueHistory, side);
        }
    }
}
