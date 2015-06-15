using System.Collections.Generic;
using System.Text;

namespace CanonicalEquation.Logic
{
    public class TermCollection
    {
        public TermCollection(bool isMinus)
        {
            Terms = new List<Term>();
            IsMinus = isMinus;
        }

        public List<Term> Terms { get; set; }
        public bool IsMinus { get; set; }

        public int Multiplicator
        {
            get { return IsMinus ? -1 : 1; }
        }

        public void AddTerm(StringBuilder currentTerm)
        {
            if (currentTerm.Length > 0)
            {
                var term = new Term(currentTerm.ToString());
                term.A *= Multiplicator;
                Terms.Add(term);
                currentTerm.Clear();
            }
        }

        public void AddCollection(TermCollection collection)
        {
            Terms.AddRange(collection.Terms);
        }
    }
}