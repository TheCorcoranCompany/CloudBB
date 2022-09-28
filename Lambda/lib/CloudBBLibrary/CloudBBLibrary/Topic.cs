using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Net;
using System.Linq;

namespace CloudBBLibrary
{
    public class Topic
    {
        string topic;
        public AmazonDynamoDBClient client;
        public Topic(string topicID)
        {
            topic = topicID;
            client = new AmazonDynamoDBClient();
        }

        public async Task<List<Post?>> GetAllPosts()
        {
            List<String> attributes = new List<String>();
            attributes.Add("messageID");
            attributes.Add("User");
            attributes.Add("messageText");
            var request = new ScanRequest
            {
                TableName = "Posts",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue {
                         S = topic
                     }}
                },
                FilterExpression = "topicID = :val",
            };
            ScanResponse? body = await client.ScanAsync(request);
            List<Post?> posts = new List<Post?>();
            foreach (var items in body.Items)
            {
                var doc = Document.FromAttributeMap(items);
                posts.Add(new DynamoDBContext(client).FromDocument<Post>(doc));
            }
            return posts;
        }
    }
}