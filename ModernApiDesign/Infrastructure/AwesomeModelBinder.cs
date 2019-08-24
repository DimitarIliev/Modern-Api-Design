using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ModernApiDesign.Infrastructure
{
    public class AwesomeModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            const string propertyName = "Photo";
            var valueProviderResult = bindingContext.ValueProvider.GetValue(propertyName);
            var base64Value = valueProviderResult.FirstValue;
            
            if (!string.IsNullOrEmpty(base64Value))
            {
                var bytes = Convert.FromBase64String(base64Value);
                var emotionResult = await GetEmotionResultAsync(bytes);
                var score = emotionResult.First().Scores;
                var result = new EmotionalPhotoDto
                {
                    Contents = bytes,
                    Scores = score
                };
                bindingContext.Result = ModelBindingResult.Success(result);
            }

            await Task.FromResult(Task.CompletedTask);
        }

        private static async Task<EmotionResultDto[]> GetEmotionResultAsync(byte[] bytes)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<SUBSCRIPTION_KEY>");
            var uri = "https://<SUBSCRIPTION_LOCATION>.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            using (var content = new ByteArrayContent(bytes))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EmotionResultDto[]>(responseContent);
                return result;
            }
        }

        //[ModelBinder(typeof(AwesomeModelBinder))] use provider instead of this if you want
        public class EmotionalPhotoDto
        {
            public byte[] Contents { get; set; }
            public EmotionScoresDto Scores { get; set; }
        }

        public class EmotionResultDto
        {
            public EmotionScoresDto Scores { get; set; }
        }

        public class EmotionScoresDto
        {
            public float Anger { get; set; }
            public float Contempt { get; set; }
            public float Disgust { get; set; }
            public float Fear { get; set; }
            public float Happiness { get; set; }
            public float Neutral { get; set; }
            public float Sadness { get; set; }
            public float Surprise { get; set; }
        }
    }
}
