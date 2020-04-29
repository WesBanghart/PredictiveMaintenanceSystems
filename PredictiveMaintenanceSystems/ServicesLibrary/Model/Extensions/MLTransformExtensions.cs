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
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            int rank = componentObject.Value<int>("Rank");
            int oversampling = componentObject.Value<int>("Oversampling");
            bool ensureZeroMean = componentObject.Value<bool>("EnsureZeroMean");
            return MLContext.AnomalyDetection.Trainers.RandomizedPca(featureColumn, exampleWeightColumn, rank, oversampling, ensureZeroMean);
        }


        //=============== BINARY CLASSIFICATION FUNCTIONS =====================================
        public static IEstimator<ITransformer> _AveragedPerceptronTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            //loss function
            float learningRate = componentObject.Value<float>("LearningRate");
            bool decreaseLearningRate = componentObject.Value<bool>("DecreaseLearningRate");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumn, featureColumn, null,
                                                                              learningRate, decreaseLearningRate,
                                                                              l2Regularization, numIterations);
        }

        public static IEstimator<ITransformer> _SdcaLogisticRegressionBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumn, featureColumn,
                                                                                  exampleWeightColumn, l2Regularization,
                                                                                  l1Regularization, numIterations);
        }

        public static IEstimator<ITransformer> _SdcaNonCalibratedBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            //sdca loss function
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.BinaryClassification.Trainers.SdcaNonCalibrated(labelColumn, featureColumn,
                                                                             exampleWeightColumn, null, l2Regularization,
                                                                             l1Regularization, numIterations);
        }

        public static IEstimator<ITransformer> _SgdCalibratedBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            float learningRate = componentObject.Value<float>("LearningRate");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            return MLContext.BinaryClassification.Trainers.SgdCalibrated(labelColumn, featureColumn, exampleWeightColumn,
                                                                         numIterations, learningRate, l2Regularization);
        }

        public static IEstimator<ITransformer> _SgdNonCalibratedTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            //loss function
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            float learningRate = componentObject.Value<float>("LearningRate");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            return MLContext.BinaryClassification.Trainers.SgdNonCalibrated(labelColumn, featureColumn,
                                                                            exampleWeightColumn, null, numIterations,
                                                                            learningRate, l2Regularization);
        }

        public static IEstimator<ITransformer> _LbfgsLogisticRegressionBinaryTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            float optimizationTolerance = componentObject.Value<float>("OptimizationTolerance");
            int historySize = componentObject.Value<int>("HistorySize");
            bool enforceNonNegativity = componentObject.Value<bool>("EnforceNonNegativity");
            return MLContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumn, featureColumn,
                                                                                   exampleWeightColumn, l1Regularization,
                                                                                   l2Regularization,
                                                                                   optimizationTolerance, historySize,
                                                                                   enforceNonNegativity);
        }

        public static IEstimator<ITransformer> _FieldAwareFactorizationMachineTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            return MLContext.BinaryClassification.Trainers.FieldAwareFactorizationMachine(featureColumn, labelColumn, exampleWeightColumn);
        }

        public static IEstimator<ITransformer> _PriorTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            return MLContext.BinaryClassification.Trainers.Prior(labelColumn, exampleWeightColumn);
        }

        public static IEstimator<ITransformer> _LinearSvmTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.BinaryClassification.Trainers.LinearSvm(labelColumn, featureColumn, exampleWeightColumn, numIterations);
        }


        //=============== CLUSTERING FUNCTIONS ================================================
        public static IEstimator<ITransformer> _KMeansTrainer(this MLContext MLContext, JToken componentObject)
        {
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string weightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            int numClusters = componentObject.Value<int>("NumberOfClusters");
            return MLContext.Clustering.Trainers.KMeans(featureColumn, weightColumn, numClusters);
        }


        //=============== MULTICLASS CLASSIFICATION FUNCTIONS =================================
        public static IEstimator<ITransformer> _SdcaMaximumEntropyMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumn, featureColumn,
                                                                                  exampleWeightColumn, l2Regularization,
                                                                                  l1Regularization, numIterations);
        }

        public static IEstimator<ITransformer> _SdcaNonCalibratedMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            //loss function
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.MulticlassClassification.Trainers.SdcaNonCalibrated(labelColumn, featureColumn,
                                                                                 exampleWeightColumn, null,
                                                                                 l2Regularization, l1Regularization,
                                                                                 numIterations);
        }

        public static IEstimator<ITransformer> _LbfgsMaximumEntropyMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            float optimizationTolerance = componentObject.Value<float>("OptimizationTolerance");
            int historySize = componentObject.Value<int>("HistorySize");
            bool enforceNonNegativity = componentObject.Value<bool>("EnforceNonNegativity");
            return MLContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumn, featureColumn,
                                                                                   exampleWeightColumn, l1Regularization,
                                                                                   l2Regularization,
                                                                                   optimizationTolerance, historySize,
                                                                                   enforceNonNegativity);
        }

        public static IEstimator<ITransformer> _NaiveBayesMulticlassTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            return MLContext.MulticlassClassification.Trainers.NaiveBayes(labelColumn, featureColumn);
        }


        //=============== REGRESSION FUNCTIONS ================================================
        public static IEstimator<ITransformer> _LbfgsPoissonRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            float optimizationTolerance = componentObject.Value<float>("OptimizationTolerance");
            int historySize = componentObject.Value<int>("HistorySize");
            bool enforceNonNegativity = componentObject.Value<bool>("EnforceNonNegativity");
            return MLContext.Regression.Trainers.LbfgsPoissonRegression(labelColumn, featureColumn, exampleWeightColumn,
                                                                        l1Regularization, l2Regularization,
                                                                        optimizationTolerance, historySize,
                                                                        enforceNonNegativity);
        }

        public static IEstimator<ITransformer> _SdcaRegressionTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            string exampleWeightColumn = componentObject.Value<string>("ExampleWeightColumnName");
            //loss function
            float l1Regularization = componentObject.Value<float>("L1Regularization");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.Regression.Trainers.Sdca(labelColumn, featureColumn, exampleWeightColumn, null,
                                                      l2Regularization, l1Regularization, numIterations);
        }

        public static IEstimator<ITransformer> _OnlineGradientDescentTrainer(this MLContext MLContext, JToken componentObject)
        {
            string labelColumn = componentObject.Value<string>("LabelColumnName");
            string featureColumn = componentObject.Value<string>("FeatureColumnName");
            //loss function
            float learningRate = componentObject.Value<float>("LearningRate");
            bool decreaseLearningRate = componentObject.Value<bool>("DecreaseLearningRate");
            float l2Regularization = componentObject.Value<float>("L2Regularization");
            int numIterations = componentObject.Value<int>("NumberOfIterations");
            return MLContext.Regression.Trainers.OnlineGradientDescent(labelColumn, featureColumn, null, learningRate,
                                                                       decreaseLearningRate, l2Regularization,
                                                                       numIterations);
        }
    }
}
