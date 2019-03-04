using Elasticsearch.Net;
using Nest;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static  void Main(string[] args)
        {

            var uris = new[]
{
    new Uri("http://localhost:9200"),
    new Uri("http://localhost:9201"),
    new Uri("http://localhost:9202"),
};

            var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex("people");

            var client = new ElasticClient(settings);
            var person = new Person
            {
                Id = 2,
                FirstName = "joha",
                LastName = "joha1"
            };

            var indexResponse = client.IndexDocument(person);

            var asyncIndexResponse =  client.IndexDocumentAsync(person).Result;
            var searchResponse = client.Search<Person>(s => s
    .From(0)
    .Size(10)
    .Query(q => q
         .Match(m => m
            .Field(f => f.FirstName)
            .Query("Martijn")
         )
    )
);

            var people = searchResponse.Documents;
            //var node = new Uri("http://localhost:9200");
            //var settings = new ConnectionSettings(node);
            //var client = new ElasticClient(settings);
            //var tweet = new Tweet
            //{
            //    Id = 1,
            //    User = "kimchy",
            //    PostDate = new DateTime(2009, 11, 15),
            //    Message = "Trying out NEST, so far so good?"
            //};
            //var response = client.Index(tweet, idx => idx.Index("mytweetindex"));
            //var responses = client.Get<Tweet>(1, idx => idx.Index("mytweetindex")); // returns an IGetResponse mapped 1-to-1 with the Elasticsearch JSON response
            //var tweets = responses.Source; // the original document
            //Console.WriteLine("Hello World!");
        }
    }
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class Skill
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}
