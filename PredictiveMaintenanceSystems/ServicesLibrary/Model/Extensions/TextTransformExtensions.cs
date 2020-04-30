using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class TextTransformExtensions
    {
        //--------------- TEXT TRANSFORMATIONS ------------------------------------------------
        public static IEstimator<ITransformer> _FeaturizeText(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.Text.FeaturizeText(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _TokenizeIntoWords(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            string separators = componentObject.Value<string>("Separators");
            return MLContext.Transforms.Text.TokenizeIntoWords(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _TokenizeIntoCharactersAsKeys(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.Text.TokenizeIntoCharactersAsKeys(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _NormalizeText(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.Text.NormalizeText(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _ProduceNgrams(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            int ngramLength = componentObject.Value<int>("NgramLength");
            return MLContext.Transforms.Text.ProduceNgrams(outputColumn, inputColumn, ngramLength);
        }

        public static IEstimator<ITransformer> _ProduceWordBags(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //TODO: return MLContext.Transforms.Text.ProduceWordBags();
        }

        public static IEstimator<ITransformer> _ProduceHashedNgrams(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //TODO: return MLContext.Transforms.Text.ProduceHashedNgrams();
        }

        public static IEstimator<ITransformer> _ProduceHashedWordBags(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //TODO: return MLContext.Transforms.Text.ProduceHashedWordBags();
        }

        public static IEstimator<ITransformer> _RemoveDefaultStopWords(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            return MLContext.Transforms.Text.RemoveDefaultStopWords(outputColumn, inputColumn);
        }

        public static IEstimator<ITransformer> _RemoveStopWords(this MLContext MLContext, JToken componentObject)
        {
            string outputColumn = componentObject.Value<string>("OutputColumnName");
            string inputColumn = componentObject.Value<string>("InputColumnName");
            string[] stopWords = componentObject.Value<string>("StopWords").Split(", ");
            return MLContext.Transforms.Text.RemoveStopWords(outputColumn, inputColumn, stopWords);
        }

        public static IEstimator<ITransformer> _LatentDirichletAllocation(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //TODO: return MLContext.Transforms.Text.LatentDirichletAllocation();
        }

        public static IEstimator<ITransformer> _ApplyWordEmbedding(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
            //TODO: return MLContext.Transforms.Text.ApplyWordEmbedding();
        }
    }
}
