using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Net;
using System.Linq;
using CloudBBLibrary;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GetAllPosts;

public class PostLambda
{
    public AmazonDynamoDBClient client;
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        string topicID;
        if (request.QueryStringParameters != null) topicID = request.QueryStringParameters["topic"];
        else
        {
            topicID = "3";
        }
        Topic topicToBeQueried = new Topic(topicID);
        List<Post?> posts = await topicToBeQueried.GetAllPosts();
        string json = JsonConvert.SerializeObject(posts);
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = json,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, {"Access-Control-Allow-Origin", "*" }, { "access-control-allow-headers", "*" } }
        };

        return response;
    }
}
