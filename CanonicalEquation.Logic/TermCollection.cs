using System.Collections.Generic;
using System.Text;

namespace CanonicalEquation.Logic
{
    /// <summary>
    /// ѕоследовательность слагаемых 
    /// </summary>
    public class TermCollection
    {
        /// <summary>
        /// —оздаем последовательность
        /// </summary>
        /// <param name="isMinus">Ќужно ли мен€ть знак при обработке всех слагаемых в этой последовательности</param>
        public TermCollection(bool isMinus)
        {
            Terms = new List<Term>();
            IsMinus = isMinus;
        }

        /// <summary>
        /// —лагаемые
        /// </summary>
        public List<Term> Terms { get; set; }

        /// <summary>
        /// Ќужно ли мен€ть знак при обработке всех слагаемых в этой последовательности
        /// </summary>
        public bool IsMinus { get; set; }

        /// <summary>
        /// ”множение дл€ изменени€ знака
        /// </summary>
        public int Multiplicator
        {
            get { return IsMinus ? -1 : 1; }
        }

        /// <summary>
        /// ѕытаемс€ добавить новое слагаемое к последовательносмти
        /// </summary>
        /// <param name="currentTerm"></param>
        public void TryAddTerm(StringBuilder currentTerm)
        {
            if (currentTerm.Length > 0)
            {
                var term = new Term(currentTerm.ToString());
                term.A *= Multiplicator;
                Terms.Add(term);
                currentTerm.Clear();
            }
        }

        /// <summary>
        /// ƒобавл€ем слагаем другой последовательности к текущей
        /// </summary>
        /// <param name="collection"></param>
        public void AddCollection(TermCollection collection)
        {
            Terms.AddRange(collection.Terms);
        }
    }
}