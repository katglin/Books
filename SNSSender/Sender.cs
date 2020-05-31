using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using System.Configuration;

namespace SNSSender
{
    public static class Sender
    {
        private static string AccessKey { get; set; }
        private static string SecretKey { get; set; }
        private static string Region { get; set; }
        private static string BaseTopicArn { get; set; }

        static Sender()
        {
            AccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            SecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            Region = ConfigurationManager.AppSettings["AWSRegion"];
            BaseTopicArn = ConfigurationManager.AppSettings["AwsBaseTopicArn"];
        }

        public static void Publish(string topic, string jsonObject)
        {
            using (var snsClient = new AmazonSimpleNotificationServiceClient(AccessKey, SecretKey, RegionEndpoint.GetBySystemName(Region)))
            {
                var topicArn = BaseTopicArn + topic;
                PublishRequest publishRequest = new PublishRequest(topicArn, jsonObject);
                snsClient.Publish(publishRequest);
            }
        }

        public static string BuildMessage(object entity)
        {
            var jsonModel = JsonConvert.SerializeObject(entity);
            return jsonModel;
        }
    }
}
