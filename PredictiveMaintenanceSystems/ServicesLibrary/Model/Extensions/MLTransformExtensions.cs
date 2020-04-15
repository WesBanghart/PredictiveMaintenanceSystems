using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class MLTransformExtensions
    {
        //=============== ANOMALY DETECTION FUNCTIONS =========================================
        public static IEstimator<ITransformer> _RandomizedPcaTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }


        //=============== BINARY CLASSIFICATION FUNCTIONS =====================================
        public static IEstimator<ITransformer> _AveragedPerceptronTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SdcaLogisticRegressionBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SdcaNonCalibratedBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SymbolicSgdLogisticRegressionBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LbfgsLogisticRegressionBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LightGbmBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FastTreeBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FastForestBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _GamBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FieldAwareFactorizationMachineTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _PriorTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LinearSvmTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }


        //=============== CLUSTERING FUNCTIONS ================================================
        public static IEstimator<ITransformer> _KMeansTrainer(this MLContext MLContext, JToken componentObject)
        {
            string weightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            int numClusters = componentObject.Value<int>("NumberOfClusters");
            return MLContext.Clustering.Trainers.KMeans(featureColumn, weightColumn, numClusters);
        }


        //=============== MULTICLASS CLASSIFICATION FUNCTIONS =================================
        public static IEstimator<ITransformer> _LightGbmMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SdcaMaximumEntropyMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SdcaNonCalibratedMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LbfgsMaximumEntropyMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _NaiveBayesMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _OneVersusAllTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _PairwiseCouplingTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ImageClassificationTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }


        //=============== RANKING FUNCTIONS ===================================================
        public static IEstimator<ITransformer> _LightGbmRankingTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FastTreeRankingTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }


        //=============== REGRESSION FUNCTIONS ================================================
        public static IEstimator<ITransformer> _LbfgsPoissonRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LightGbmRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _SdcaRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _OlsTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _OnlineGradientDescentTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FastTreeRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FastTreeTweedieTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _FastForestRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _GamRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
