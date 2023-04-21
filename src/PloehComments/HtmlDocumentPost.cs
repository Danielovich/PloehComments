using HtmlAgilityPack;

namespace PloehComments;

public class HtmlDocumentPost
{
    public async Task<HtmlDocument> PostToHtmlDocumentAsync(string htmlCandidate)
    {
        //ensure </p> is kept as-is. If true </p> is stripped.
        HtmlDocument.DisableBehaviorTagP = false;

        var htmlDocument = new HtmlDocument();
        
        htmlDocument.LoadHtml(htmlCandidate);

        return await Task.FromResult(htmlDocument);
    }

    public async Task SaveDocumentAsync(HtmlDocument htmlDocument, string fileName)
    {
        htmlDocument.Save(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "AnchoredPosts", fileName));

        await Task.CompletedTask;
    }

    public async Task AppendCommentLinkAsync(HtmlDocument htmlDocument)
    {
        foreach (var documentNode in htmlDocument.DocumentNode.Descendants())
        {
            if (documentNode.NodeType != HtmlNodeType.Text)
            {
                // get whatever first class
                var commentClass = documentNode.GetClasses()
                    .FirstOrDefault(string.Empty);

                var postId = documentNode.Id;

                // if we have ourselves a comment!
                if (commentClass.Equals("comment"))
                {
                    // inside the comment there is a comment-author
                    var commentAuthorNode = documentNode.ChildNodes
                        .Where(node => node.Attributes.Any(attribute => attribute.Value == "comment-author"))
                        .FirstOrDefault();

                    // let's append a new link to that comment-author
                    if (commentAuthorNode is not null)
                    {
                        commentAuthorNode.InnerHtml += $" <a href=\"#{postId}\">#</a>";
                    }
                }
            }
        }

        await Task.CompletedTask;
    }

    private List<int> commentIndexes = new List<int>();

    public List<int> CommentIndex(string blogpost, int startIndex = 0)
    {
        var searchFor = blogpost.IndexOf("<div class=\"comment\" id=\"", startIndex);

        if (searchFor == -1 || searchFor == 0)
        {
            return commentIndexes;
        }

        var commentIdStart = (searchFor + 25);

        commentIndexes.Add(commentIdStart);

        return CommentIndex(blogpost, commentIdStart);

    }

    public string GetCommentId(int commentSectionStartIndex, string blogpost)
    {
        if (commentSectionStartIndex <= 0)
            return string.Empty;

        var commentIdLength = 32;

        return blogpost.Substring(commentSectionStartIndex, commentIdLength);
    }
}