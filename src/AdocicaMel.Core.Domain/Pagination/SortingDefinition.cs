using System;

namespace AdocicaMel.Core.Domain.Pagination
{
    public class SortingDefinition<T> where T : Enum
    {
        public SortingDefinition()
        {

        }
        public SortingDefinition(string sortExpression)
        {
            var splittedValues = sortExpression.Split(' ');
            if(splittedValues.Length < 2)
            {
                throw new ArgumentException("A expressão de ordenação é inválida");
            }
            Field = (T)Enum.Parse(typeof(T), splittedValues[0], true);
            Order = (ESortingOrder)Enum.Parse(typeof(ESortingOrder), splittedValues[1], true);
        }

        public T Field { get; set; }
        public ESortingOrder Order { get; set; }
    }
}
