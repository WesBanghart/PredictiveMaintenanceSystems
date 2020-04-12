using Microsoft.ML;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesLibrary.Model.Extensions
{
    public static class ImageTransformExtensions
    {
        //--------------- IMAGE TRANSFORMATIONS -----------------------------------------------
        public static IEstimator<ITransformer> _ConvertToGrayscale(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ConvertToImage(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ExtractPixels(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _LoadImages(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _ResizeImages(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }

        public static IEstimator<ITransformer> _DnnFeaturizeImage(this MLContext MLContext, JToken componentObject)
        {
            throw new NotImplementedException();
        }
    }
}
