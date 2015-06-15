using System.Collections.Generic;
using System.Text;

namespace CanonicalEquation.Logic
{
    /// <summary>
    /// ������������������ ��������� 
    /// </summary>
    public class TermCollection
    {
        /// <summary>
        /// ������� ������������������
        /// </summary>
        /// <param name="isMinus">����� �� ������ ���� ��� ��������� ���� ��������� � ���� ������������������</param>
        public TermCollection(bool isMinus)
        {
            Terms = new List<Term>();
            IsMinus = isMinus;
        }

        /// <summary>
        /// ���������
        /// </summary>
        public List<Term> Terms { get; set; }

        /// <summary>
        /// ����� �� ������ ���� ��� ��������� ���� ��������� � ���� ������������������
        /// </summary>
        public bool IsMinus { get; set; }

        /// <summary>
        /// ��������� ��� ��������� �����
        /// </summary>
        public int Multiplicator
        {
            get { return IsMinus ? -1 : 1; }
        }

        /// <summary>
        /// �������� �������� ����� ��������� � �������������������
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
        /// ��������� ������� ������ ������������������ � �������
        /// </summary>
        /// <param name="collection"></param>
        public void AddCollection(TermCollection collection)
        {
            Terms.AddRange(collection.Terms);
        }
    }
}