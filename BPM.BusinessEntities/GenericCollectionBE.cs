using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace BPM.BusinessEntities
{
    /// <summary>
    /// Represents Generic collection class
    /// </summary>
    /// <typeparam name="GENERICTYPE"></typeparam>
   [Serializable]
    public class GenericCollection<GENERICTYPE>
         : CollectionBase
    {
        public GenericCollection()
        { 
        }
        public void Add(int index, GENERICTYPE GenericObject)
        {
            InnerList.Insert(index,GenericObject);
        }

        public void Remove(int index)
        {
            InnerList.RemoveAt(index);
        }

        // public GENERICTYPE Item(int index)
        //{
        //    return (GENERICTYPE)InnerList[index];
        //}
        public GENERICTYPE this[int index]
        {
            get { return (GENERICTYPE)List[index]; }
            set { List[index] = value; }
        }



        //public GenericCollectionInfo this[int index]
        //{
        //    get { return (GenericCollectionInfo)this.InnerList[index]; }
        //    set { this.InnerList[index] = value; }
        //}

        //public CardBE Find(CardBE obCardBE)
        //{
        //    int idx = 0;
        //    idx = Indexof(obCardBE);
        //    if (idx > -1)
        //    {
        //        return (CardBE)InnerList[idx];
        //    }
        //    return obCardBE;
        //}

        //public int Indexof(CardBE obCardBE)
        //{
        //    int idx = -1;
        //    foreach (CardBE obCard in List)
        //    {
        //        idx++;
        //        if (String.Compare(obCard.CardNumber, obCardBE.CardNumber, true) == 0)
        //        {
        //            return idx;
        //        }
        //    }
        //    return -1;
        //}

        public void Sort(IComparer comparer)
        {
            //IComparer<string> o;
            //o = comparer;
            this.InnerList.Sort(comparer);
        }
        public class GenericComparer : IComparer
        {

            private SortDirection sortDirection;

            public SortDirection SortDirection
            {

                get { return this.sortDirection; }

                set { this.sortDirection = value; }

            }

            private string sortExpression;

            public GenericComparer(string sortExpression, SortDirection sortDirection)
            {

                this.sortExpression = sortExpression;

                this.sortDirection = sortDirection;

            }

            public int Compare(object x, object y)
            {

                PropertyInfo propertyInfo = typeof(GENERICTYPE).GetProperty(sortExpression);

                IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);

                IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);



                if (SortDirection == SortDirection.Ascending)
                {

                    return obj1.CompareTo(obj2);

                }

                else return obj2.CompareTo(obj1);

            }

        }


    }

}
