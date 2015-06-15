using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanonicalEquation.Logic
{
    public class EquationLogic
    {
        public string Process(string equation)
        {
            var currentTerm = new StringBuilder();
            var collection = new Stack<TermCollection>();
            collection.Push(new TermCollection(false));
            var isMinus = false;
            bool exponentaPart = false;
            foreach (var current in equation)
            {
                if (!(Char.IsNumber(current) || (current == '-' && currentTerm.Length > 0 && currentTerm[currentTerm.Length - 1] == '^')))
                {
                    exponentaPart = false;
                }
                switch (current)
                {
                    case ' ':
                        continue;
                    case '(':
                        collection.Push(new TermCollection(isMinus ^ collection.First().IsMinus));
                        currentTerm.Clear();
                        isMinus = false;
                        break;
                    case ')':
                        collection.First().AddTerm(currentTerm);
                        var currentCollection = collection.Pop();
                        collection.First().AddCollection(currentCollection);
                        break;
                    case '+':
                        isMinus = false;
                        collection.First().AddTerm(currentTerm);
                        currentTerm.Append(current);
                        break;
                    case '-':
                        if (exponentaPart)//Этот минус - показатель отрицательной степени K
                        {
                            currentTerm.Append(current);
                        }
                        else
                        {//Это минус в выражении, между Term
                            isMinus = true;
                            collection.First().AddTerm(currentTerm);
                            currentTerm.Append(current);
                        }
                        break;
                    case '=':
                        isMinus = false;
                        collection.First().AddTerm(currentTerm);
                        collection.Push(new TermCollection(true));
                        break;
                    case '^':
                        exponentaPart = true;
                        currentTerm.Append(current);
                        break;
                    default:
                        if (currentTerm.Length == 0)
                        {
                            currentTerm.Append("+");
                        }
                        currentTerm.Append(current);
                        break;
                }
            }
            collection.First().AddTerm(currentTerm);

            List<Term> terms =  new List<Term>();
            for (int i = collection.Count-1; i >= 0; i--)
            {
                foreach (var term in collection.Skip(i).First().Terms)
                {
                    terms.Add(term);
                }
            }

            for (int i = 0; i < terms.Count; i++)
            {
                for (int j = i+1; j < terms.Count; j++)
                {
                    if (terms[i].HasSameVariables(terms[j]))
                    {
                        terms[i].A += terms[j].A;
                        terms.Remove(terms[j]);
                        j--;
                    }
                }
            }


            var result = new StringBuilder();
            foreach (Term term in terms)
            {
                if (term.A != 0)
                {
                    result.Append(term);
                }
            }
            result.Append("=0");

            return result[0] == '+' ? result.ToString(1, result.Length - 1) : result.ToString();
        }
    }
}