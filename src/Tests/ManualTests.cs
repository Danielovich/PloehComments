using PloehComments;

namespace Tests;

public class ManualTests
{
    [Fact]
    //[Fact(Skip = "manual")]
    public async Task Append_Links_And_Save_Html_Document_For_All_Posts()
    {
        // posts are located at this dir path
        var postDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts");

        // all posts in the dir
        var postNameRetriver = new PostsNameRetriever(postDirectoryPath);
        var posts = await postNameRetriver.PostsInDirectory() ?? Enumerable.Empty<string>();

        foreach (var postFileName in posts)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts", postFileName);

            // load the original blog post
            var postLoader = new PostLoader(filePath);
            var originalPost = await postLoader.PostToStringAsync();

            // parse the post to html
            var htmlPost = new HtmlDocumentPost();
            var htmlDocument = await htmlPost.PostToHtmlDocumentAsync(originalPost);

            // append links to the blog post
            await htmlPost.AppendCommentLinkAsync(htmlDocument);

            // assert anchors on the blog post before we save a copy
            var updatedPost = htmlDocument.DocumentNode.WriteTo();
            await PostDiff.AssertAnchors(originalPost, updatedPost);

            // save an updated copy of the post. Now with anchors
            await htmlPost.SaveDocumentAsync(htmlDocument, postFileName);
        }
    }
}