using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PloehComments
{
    public class PloehPostLink
    {
        public PloehPostLink(string anchorId)
        {
            if(anchorId == null) throw new ArgumentNullException(nameof(anchorId));

            _anchorId = anchorId;
        }

        private string _anchorId { get; }

        public string Link
        {
            get
            {
                return $" <a href=\"#{_anchorId}\">#</a>";
            }
        }
    }
}
