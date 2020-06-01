using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using DTO;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SNSReceiver
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {

        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach(var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            string connectionString = @"Data Source=DESKTOP-MLOOPKT\SQLEXPRESS;Initial Catalog=BookStore;Integrated Security=True";
            context.Logger.LogLine($"123 Processed message {message.Body}");

            var authorDto = JsonConverter.DeserializeObject(message.Body) as AuthorDTO;
            using(var connection = new SqlConnection(connectionString))
            {
                string queryString = $@"INSERT INTO Author (FirstName, LastName)
                                        VALUES ('{authorDto.FirstName}', '{authorDto.LastName}')";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.ExecuteNonQuery();
            }

            await Task.CompletedTask;
        }
    }
}
