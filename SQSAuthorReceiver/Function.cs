using System.Threading.Tasks;
using System.Data.SqlClient;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;
using DTO;
using System;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SQSAuthorReceiver
{
    public class Function
    {

        private string _connectionString;

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            string server = Environment.GetEnvironmentVariable("DB_ENDPOINT");
            string database = Environment.GetEnvironmentVariable("DATABASE");
            string username = Environment.GetEnvironmentVariable("USER");
            string pwd = Environment.GetEnvironmentVariable("PASSWORD");
            _connectionString = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", server, database, username, pwd);
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
            context.Logger.LogLine($"Processed Author message with Mesasge.Body: {message.Body}");

            var messageDto = JsonConvert.DeserializeObject<SQSMessageDTO>(message.Body);
            var authorDto = JsonConvert.DeserializeObject<AuthorDTO>(messageDto.Message);
            string sql = $@"INSERT INTO Author (FirstName, LastName)
                            VALUES ('{authorDto.FirstName}', '{authorDto.LastName}')";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            await Task.CompletedTask;
        }
    }
}
