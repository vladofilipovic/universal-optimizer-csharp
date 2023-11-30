using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFileTransformations
{
    public class CvorGrafa : IComparable<CvorGrafa>
    {
        public int id;
        public string oznaka;

        public CvorGrafa(int id, string oznaka)
        {
            this.id = id;
            this.oznaka = oznaka;
        }

        public int CompareTo(CvorGrafa other)
        {
            if (id - other.id != 0)
                return (id - other.id);
            return oznaka.CompareTo(other.oznaka);
        }
    };
}
