using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Dapper;
using DTO;
using Newtonsoft.Json;
using Data.Extensions;
using System.Data;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SQSBookReceiver
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
            context.Logger.LogLine($"Processed Book message with Mesasge.Body: {message.Body}");

            var messageDto = JsonConvert.DeserializeObject<SQSMessageDTO>(message.Body);

            context.Logger.LogLine($"messageDto.Message: {messageDto.Message}");

            var bookDto = JsonConvert.DeserializeObject<BookDTO>(messageDto.Message);


            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                context.Logger.LogLine($"Connection established. BookName: {bookDto.Name}");

                var queryParameters = new DynamicParameters();
                var ids = bookDto.AuthorIds.AsDataTableParam();
                queryParameters.Add("@BookName", bookDto.Name);
                queryParameters.Add("@ReleaseDate", bookDto.ReleaseDate);
                queryParameters.Add("@Rate", bookDto.Rate);
                queryParameters.Add("@PageNumber", bookDto.PageNumber);
                queryParameters.Add("@AuthorIds", ids.AsTableValuedParameter("BigIntArrayType"));
                connection.Query<long>("USPCreateBook", queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                connection.Close();
            }
            await Task.CompletedTask;
        }
    }
}
