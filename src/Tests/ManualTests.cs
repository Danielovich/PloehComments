using PloehComments;

namespace Tests;

public class ManualTests
{
    //[Fact]
    [Fact(Skip = "manual")]
    public async Task Append_Links_And_Save_Html_Document_For_All_Posts()
    {
        // what dir holds the posts 
        var postDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts");

        // all posts in the dir
        var postNameRetriver = new PostsNameRetriever(postDirectoryPath);
        var posts = await postNameRetriver.PostsInDirectory() ?? Enumerable.Empty<string>();

        foreach (var postFileName in posts)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts", postFileName);

            // load it
            var postLoader = new PostLoader(filePath);
            var blogpost = await postLoader.PostToStringAsync();

            // parse it to html
            var htmlPost = new HtmlDocumentPost();
            var htmlDocument = await htmlPost.PostToHtmlDocumentAsync(blogpost);

            await htmlPost.AppendCommentLinkAsync(htmlDocument);
            await htmlPost.SaveDocumentAsync(htmlDocument, postFileName);
        }
    }
}