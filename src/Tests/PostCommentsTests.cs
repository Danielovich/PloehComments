using PloehComments;

namespace Tests;

public class HtmlDocumentPostTests
{
    [Fact]
    public async Task Append_Links_To_Post_And_Save_As_An_Updated_Post()
    {
        // Arrange

        var postName = "2011-11-08-Independency.html";

        // path to orginal post
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts", postName);

        // load it
        var postLoader = new PostLoader(filePath);
        var blogpost = await postLoader.PostToStringAsync();

        // parse it to html
        var htmlPost = new HtmlDocumentPost();
        var htmlDocument = await htmlPost.PostToHtmlDocumentAsync(blogpost);

        // Act || append new links
        await htmlPost.AppendCommentLinkAsync(htmlDocument);
        
        // Assertion

        // updated semnatics and content
        var updatedPost = htmlDocument.DocumentNode.WriteTo();

        // assert we have our links
        Assert.True(updatedPost.IndexOf("<a href=\"#4f0bc473865d40cc95122f028e72dc3a\">#</a>") > 0);
        Assert.True(updatedPost.IndexOf("<a href=\"#52e35411c7774242a965bdb290e4a2eb\">#</a>") > 0);
        Assert.True(updatedPost.IndexOf("<a href=\"#56cec1c8960b43dab4bf58c356e8bb44\">#</a>") > 0);
        Assert.True(updatedPost.IndexOf("<a href=\"#95ce4e4b469847cea860f8a778ed1b6a\">#</a>") > 0);
        Assert.True(updatedPost.IndexOf("<a href=\"#05d451209f714c029c143fd420c6ff99\">#</a>") > 0);
    }

    [Fact]
    public async Task Compare()
    {
        // Arrange

        var postName = "2011-11-08-Independency.html";

        // path to orginal post
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Posts", postName);

        // load it
        var postLoader = new PostLoader(filePath);
        var original = await postLoader.PostToStringAsync();

        // parse it to html
        var htmlPost = new HtmlDocumentPost();
        var htmlDocument = await htmlPost.PostToHtmlDocumentAsync(original);

        // Act || append new links
        await htmlPost.AppendCommentLinkAsync(htmlDocument);
        
        // Assertion

        // updated semantics and content
        var updatedPost = htmlDocument.DocumentNode.WriteTo();
        
        // assert updates to post
        await PostDiff.AssertAnchors(original, updatedPost);
    }
}