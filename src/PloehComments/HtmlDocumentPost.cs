using HtmlAgilityPack;

namespace PloehComments;

public class HtmlDocumentPost
{
    static HtmlDocumentPost()
    {
        //ensure </p> is kept as-is. If true </p> is stripped.
        HtmlDocument.DisableBehaviorTagP = false;
    }

    public async Task<HtmlDocument> PostToHtmlDocumentAsync(string htmlCandidate)
    {
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
}