using PloehComments;

namespace Tests;

public class HtmlDocumentPostTests
{
    [Fact]
    public async Task Append_Links_To_Post_And_Save_As_An_Updated_Post()
    {
        var postName = "2011-11-08-Independency.html";

        // path to orginal post
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts", postName);

        // load it
        var postLoader = new PostLoader(filePath);
        var blogpost = await postLoader.PostToStringAsync();

        // parse it to html
        var htmlPost = new HtmlDocumentPost();
        var htmlDocument = await htmlPost.PostToHtmlDocumentAsync(blogpost);

        // append new links and save a copy
        await htmlPost.AppendCommentLinkAsync(htmlDocument);
        await htmlPost.SaveDocumentAsync(htmlDocument, postName);

        // path to the new post with new links
        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AnchoredPosts", postName);

        // etc...
        postLoader = new PostLoader(filePath);
        blogpost = await postLoader.PostToStringAsync();

        // assert we have our links
        Assert.True(blogpost.IndexOf("<a href=\"#4f0bc473865d40cc95122f028e72dc3a\">#</a>") > 0);
        Assert.True(blogpost.IndexOf("<a href=\"#52e35411c7774242a965bdb290e4a2eb\">#</a>") > 0);
        Assert.True(blogpost.IndexOf("<a href=\"#56cec1c8960b43dab4bf58c356e8bb44\">#</a>") > 0);
        Assert.True(blogpost.IndexOf("<a href=\"#95ce4e4b469847cea860f8a778ed1b6a\">#</a>") > 0);
        Assert.True(blogpost.IndexOf("<a href=\"#05d451209f714c029c143fd420c6ff99\">#</a>") > 0);
    }

    [Fact]
    public async Task Post_Comment_Id_Length_Is_32()
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

            var comments = htmlPost.CommentIndex(blogpost);

            foreach (var commentCandidate in comments)
            {
                var commentId = htmlPost.GetCommentId(commentCandidate, blogpost);

                Assert.True(commentId.Length == 32);
            }
        }
    }

    [Fact]
    public async Task Post_Comment_Id_Length_Is_321()
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

            var comments = htmlPost.CommentIndex(blogpost);

            foreach (var commentCandidate in comments)
            {
                var commentId = htmlPost.GetCommentId(commentCandidate, blogpost);

                Assert.True(commentId.Length == 32);
            }
        }
    }
}