using HtmlAgilityPack;
using System.Collections;

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

    public async Task<IEnumerable<PloehPostLink>> AppendCommentLinkAsync(HtmlDocument htmlDocument)
    {
        var appendedAnchors = new List<PloehPostLink>();
        
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
                        var anchorLink = new PloehPostLink(postId);
                        appendedAnchors.Add(anchorLink);

                        commentAuthorNode.InnerHtml += anchorLink.Link;
                    }
                }
            }
        }

        return await Task.FromResult(appendedAnchors.AsEnumerable());
    }
}